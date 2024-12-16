using AutoMapper;
using BusinessObjects.Enums;
using BusinessObjects.Models;
using BusinessObjects.ViewModels.Booking;
using BusinessObjects.ViewModels.DailyTour;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;
using Services.Common;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using QRCoder;
using System.Drawing;
using System.IO;
using ZXing.QrCode.Internal;
using ZXing;
using ZXing.QrCode;
using ZXing.Common;
using BusinessObjects.ViewModels.Rate;
using BusinessObjects.ViewModels.Account;
using Google.Apis.Storage.v1.Data;
//using static QRCoder.PayloadGenerator;
using BusinessObjects.ViewModels.Location;
using BusinessObjects.ViewModels.PackageTour;
using BusinessObjects.ViewModels.Service;
using BusinessObjects.ViewModels.TicketType;
using BusinessObjects.ViewModels.TourSegment;
using Newtonsoft.Json;
using Microsoft.Extensions.Options;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using BusinessObjects.ViewModels.Payment;
using BusinessObjects.ViewModels.Notification;
//using SixLabors.ImageSharp.Formats.Png;

namespace Services.Services
{
    public class BookingFlowService : IBookingFlowService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly CloudinaryService _cloudinaryService;
        private readonly EncryptionHelperService _encryptionHelperService;
      //  private readonly TwilioSettings _twilioSettings;
        public BookingFlowService(IUnitOfWork unitOfWork, IMapper mapper, CloudinaryService cloudinaryService, EncryptionHelperService encryptionHelperService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cloudinaryService = cloudinaryService;
            _encryptionHelperService = encryptionHelperService;
        //    _twilioSettings = twilioSettings.Value;
        }
        //public async Task<APIResponseModel> CreateBookingFlowAsync(BookingCreateModel createModel)
        //{
        //    try
        //    {
        //        var newBookingId = Guid.NewGuid();
        //        var user = await _unitOfWork.AccountRepository.GetByIdStringAsync(createModel.UserId);
        //        if (user == null || string.IsNullOrEmpty(createModel.UserId))
        //        {
        //            return new APIResponseModel
        //            {
        //                Message = "User must exist.",
        //                IsSuccess = false,
        //            };
        //        }

        //        if (createModel.TotalPrice <= 0)
        //        {
        //            return new APIResponseModel
        //            {
        //                Message = "Price must be greater than 0.",
        //                IsSuccess = false,
        //            };
        //        }

        //        var booking = new Booking
        //        {
        //            BookingId = newBookingId.ToString(),
        //            UserId = createModel.UserId,
        //            BookingDate = DateTime.Now,
        //            DailyTourId = createModel.DailyTourId,
        //            PaymentId = createModel.PaymentId,
        //            TotalPrice = createModel.TotalPrice,
        //            CreateDate = DateTime.Now,
        //            Status = (int?)EStatus.IsPending
        //        };
        //        //var bookingByRevenue = new BookingByRevenue 
        //        //{

        //        //};
        //        await _unitOfWork.BookingRepository.AddAsync(booking);
        //        _unitOfWork.Save();
        //        return new APIResponseModel
        //        {
        //            Message = "Booking created successfully",
        //            IsSuccess = true,
        //            Data = booking
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        return new APIResponseModel
        //        {
        //            Message = ex.Message,
        //            IsSuccess = false,
        //        };
        //    }

        //}
        public async Task<List<string>> GetServiceIdsByDailyTourId(string dailyTourId)
        {
            try
            {
                var dailyTour = await _unitOfWork.DailyTourRepository.GetFirstOrDefaultAsync(query => query
                    .Where(dt => dt.DailyTourId == dailyTourId && dt.Status == 1)
                    .Include(dt => dt.PackageTours)
                        .ThenInclude(pt => pt.TourSegments)
                            .ThenInclude(ts => ts.Destinations)
                                .ThenInclude(d => d.Locations)
                                    .ThenInclude(l => l.Services)
                                        .ThenInclude(sbt => sbt.ServiceByTourSegments)
                );

                if (dailyTour == null)
                {
                    return new List<string>(); 
                }

                var serviceIds = dailyTour.PackageTours?.TourSegments
                    .SelectMany(ts => ts.ServiceByTourSegments)
                    .Where(sbts => sbts.Services?.Status == 1)
                    .Select(se => se.Services?.ServiceId)
                    .Distinct() 
                    .ToList();

                return serviceIds;
            }
            catch (Exception ex)
            {
                return new List<string>(); 
            }
        }

        public async Task<APIResponseModel> RollBackBookingFlowAsync(RollBackBookingFlowModel rollBackBookingFlowModel)
        {
            using var transaction = _unitOfWork.BeginTransaction();
            try
            {
                var orderId = await _unitOfWork.PaymentRepository.GetFirstOrDefaultAsync(query => query.Where(p=>p.OrderId == rollBackBookingFlowModel.OrderId
                                                                                                                 && p.BookingId == rollBackBookingFlowModel.BookingId));
                if (orderId != null)
                {
                    return new APIResponseModel
                    {
                        Message = "Order already exists",
                        IsSuccess = false,
                    };
                }
                var booking = await _unitOfWork.BookingRepository.GetFirstOrDefaultAsync(query => query.Where(b=>b.BookingId==rollBackBookingFlowModel.BookingId));
                if (booking == null)
                {
                    return new APIResponseModel
                    {
                        Message = "BookingId does not exist",
                        IsSuccess = false,
                    };
                }

                var tickets = await _unitOfWork.TicketRepository.GetAllAsyncs(query => query.Where(t => t.BookingId == booking.BookingId));
                if (tickets == null || !tickets.Any())
                {
                    return new APIResponseModel
                    {
                        Message = "No tickets found for the booking.",
                        IsSuccess = false,
                    };
                }

                foreach (var ticket in tickets)
                {
                    var dailyTicket = await _unitOfWork.DailyTicketRepository.GetFirstOrDefaultAsync(query => query.Where(dt => dt.DailyTicketId == ticket.DailyTicketId));
                    if (dailyTicket != null)
                    {
                        dailyTicket.Capacity += ticket.Quantity;
                        await _unitOfWork.DailyTicketRepository.UpdateAsync(dailyTicket);
                    }
                    var servicesUsedByTicket = await _unitOfWork.ServiceUsedByTicketRepository.GetAllAsyncs(query => query.Where(s => s.TicketId == ticket.TicketId));
                    foreach (var service in servicesUsedByTicket)
                    {
                        await _unitOfWork.ServiceUsedByTicketRepository.DeleteAsync(service);
                    }

                    await _unitOfWork.TicketRepository.DeleteAsync(ticket);
                }

                await _unitOfWork.BookingRepository.DeleteAsync(booking);
                _unitOfWork.Save();
                transaction.Commit();

                return new APIResponseModel
                {
                    Message = "Rollback successfully",
                    IsSuccess = true,
                };

            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return new APIResponseModel
                {
                    Message = $"Booking and ticket creation failed: {ex.Message}",
                    IsSuccess = false,
                };
            }
        }

        public async Task<APIResponseModel> CreateBookingFlowAsync(BookingFlowCreateModel createModel)
        {
            using var transaction = _unitOfWork.BeginTransaction();
            try
            {
                var zaloAccount = await _unitOfWork.AccountRepository.GetFirstOrDefaultAsync(query => query
                                  .Where(a => a.ZaloUser == createModel.ZaloId));
                if (zaloAccount == null)
                {
                    return new APIResponseModel
                    {
                        Message = "Account not found.",
                        IsSuccess = false,
                    };
                }

                var serviceIds = await GetServiceIdsByDailyTourId(createModel.DailyTourId);
                if (serviceIds == null || !serviceIds.Any())
                {
                    return new APIResponseModel
                    {
                        Message = "No Service IDs found.",
                        IsSuccess = false,
                    };
                }
                var dailyTourExpirationDate = await _unitOfWork.DailyTourRepository.GetFirstOrDefaultAsync(query => query
                                  .Where(a => a.DailyTourId == createModel.DailyTourId));
                var dailyTourDetails = await GetDailyToursDetails(createModel.DailyTourId);
                if (dailyTourDetails == null)
                {
                    return new APIResponseModel
                    {
                        Message = "Tour details not found.",
                        IsSuccess = false,
                    };
                }
                var destinationId = dailyTourDetails?.PackageTour?.TourSegments?
                                        .Select(l => l.DestinationId.ToString())
                                        .ToList();
                var destinationName = dailyTourDetails?.PackageTour?.TourSegments?
                                        .Select(l => l.DestinationName)
                                        .ToList();
                var destination = dailyTourDetails.PackageTour?.TourSegments?.ToList();
                var location = destination.SelectMany(l => l.Locations)
                                           .Select(c=>c.LocationId).ToList();

                var services = destination.SelectMany(l => l.Locations)
                                           .SelectMany(s => s.Services)
                                           .Select(c=> c.ServiceId).ToList();

                //var destinationIdString = string.Join("; ", destinationId);
                //var locationId = string.Join("; ", location);
                //var servicesId = string.Join("; ", services);
                var destinationIdJson = JsonConvert.SerializeObject(destinationId);
                var destinationNameJson = JsonConvert.SerializeObject(destinationName);
                var locationJson = JsonConvert.SerializeObject(location);
                var servicesJson = JsonConvert.SerializeObject(services);

                var newBookingId = Guid.NewGuid();
                var newBooking = new Booking
                {
                    BookingId = newBookingId.ToString(),
                    UserId = zaloAccount.Id,
                    DailyTourId = createModel.DailyTourId,
                    BookingDate = DateTime.Now,
                    ExpirationDate = dailyTourExpirationDate.ExpirationDate,
                    TotalPrice = createModel.TotalPrice,
                    Status = 9,
                    CreateDate = DateTime.Now,
                };
                await _unitOfWork.BookingRepository.AddAsync(newBooking);
                _unitOfWork.Save();
                List<string> ticketIds = new List<string>();

                foreach (var ticket in createModel.Tickets)
                {
                    var dailyTicketType = await _unitOfWork.DailyTicketRepository.GetFirstOrDefaultAsync(query =>
                                                       query.Where(d => d.DailyTicketId == ticket.DailyTicketId));
                    if (dailyTicketType == null || dailyTicketType.Capacity < ticket.TotalQuantity)
                    {
                        throw new InvalidOperationException($"Not enough capacity for ticket type {dailyTicketType?.TicketTypes?.TicketTypeName}.");
                    }
                    dailyTicketType.Capacity -= ticket.TotalQuantity;
                    dailyTicketType.UpdateDate = DateTime.Now;
                    await _unitOfWork.DailyTicketRepository.UpdateAsync(dailyTicketType);

                    for (int i = 0; i < ticket.TotalQuantity; i++)
                    {
                        var newTicketId = Guid.NewGuid().ToString();
                        var priceByTicket = await _unitOfWork.DailyTicketRepository.
                                                              GetFirstOrDefaultAsync(query => query.Where(c => c.DailyTicketId == ticket.DailyTicketId));
                        var qrData = new
                        {
                         //   BookingId = newBooking.BookingId,
                         //   ZaloUser = createModel.ZaloId,
                            DailyTourId = newBooking.DailyTourId,
                            TourName = dailyTourDetails.DailyTourName,
                        //    StartDate = dailyTourDetails.StartDate,
                         //   EndDate = dailyTourDetails.EndDate,
                       //     Discount = dailyTourDetails.Discount,
                       //     DailyTourPrice = dailyTourDetails.DailyTourPrice,
                     //       City = dailyTourDetails.PackageTour?.CityId,
                            DestinationId = destinationIdJson,
                            DestinationName = destinationNameJson,
                       //     LocationId = locationJson,
                       //     ServiceId = servicesJson,
                      //      PhoneNumber = zaloAccount.PhoneNumber,
                       //     BookingDate = newBooking.BookingDate,
                       //     PriceOfTicket = priceByTicket.DailyTicketPrice,
                            ExpirationDate = newBooking.ExpirationDate,
                            TotalPrice = newBooking.TotalPrice,
                            TicketTypeId = newTicketId,
                            DailyTicketId = ticket.DailyTicketId,
                            TicketName = ticket.TicketName,
                            Price = ticket.TotalPrice
                        };
                        //  var qrContent = JsonConvert.SerializeObject(qrData);
                        var qrContent = _encryptionHelperService.EncryptString(JsonConvert.SerializeObject(qrData));
                        var qrImageUrl = await GenerateQRCode(qrContent);
                        var newTicket = new Ticket
                        {
                            TicketId = newTicketId,
                            BookingId = newBooking.BookingId,
                            DailyTicketId = ticket.DailyTicketId,
                            TicketName = ticket.TicketName,
                            Price = priceByTicket.DailyTicketPrice,
                            QRImgUrl = qrImageUrl,
                            PhoneNumberReference = zaloAccount.PhoneNumber,
                            Quantity = 1,  
                            Status = 9,
                            CreateDate = DateTime.Now,
                        };
                        await _unitOfWork.TicketRepository.AddAsync(newTicket);

                        ticketIds.Add(newTicket.TicketId);

                        foreach (var serviceId in serviceIds)
                        {
                            var serviceUsedByTicket = new ServiceUsedByTicket
                            {
                                SUBTId = Guid.NewGuid().ToString(),
                                TicketId = newTicket.TicketId,
                                ServiceId = serviceId,
                                CreateDate = DateTime.Now,
                                Status = 9
                            };
                            await _unitOfWork.ServiceUsedByTicketRepository.AddAsync(serviceUsedByTicket);
                        }
                    }
                    //var dailyTicketType = await _unitOfWork.DailyTicketRepository.GetFirstOrDefaultAsync(query =>
                    //                                   query.Where(d => d.DailyTicketId == ticket.DailyTicketId));
                    //if (dailyTicketType == null || dailyTicketType.Capacity < ticket.TotalQuantity)
                    //{
                    //    throw new InvalidOperationException($"Not enough capacity for ticket type {dailyTicketType.TicketTypes.TicketTypeName}.");
                    //}
                    //dailyTicketType.Capacity -= ticket.TotalQuantity;
                    //dailyTicketType.UpdateDate = DateTime.Now;
                    //await _unitOfWork.DailyTicketRepository.UpdateAsync(dailyTicketType);
                }
                _unitOfWork.Save();
                transaction.Commit();

                return new APIResponseModel
                {
                    Message = "Booking and tickets created successfully.",
                    IsSuccess = true,
                    Data = new
                    {
                        BookingId = newBookingId
                    }
                };
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return new APIResponseModel
                {
                    Message = $"Booking and ticket creation failed: {ex.Message}",
                    IsSuccess = false,
                };
            }
        }

        public async Task<APIResponseModel> DecryptBookingFlowAsync(DecryptBooking encryptedQrData)
        {
            try
            {
                var decryptedJson = _encryptionHelperService.DecryptString(encryptedQrData.EncryptedQr);

                var bookingFlowData = JsonConvert.DeserializeObject<BookingFlowDataModel>(decryptedJson);
                if (bookingFlowData == null)
                {
                    return new APIResponseModel
                    {
                        Message = "Failed to deserialize booking flow data.",
                        IsSuccess = false
                    };
                }

                // Trả về kết quả thành công
                return new APIResponseModel
                {
                    Data = bookingFlowData,
                    Message = "Decryption successful.",
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                return new APIResponseModel
                {
                    Message = $"Error during decryption: {ex.Message}",
                    IsSuccess = false
                };
            }
        }

        private async Task<string> GenerateQRCode(string data)
        {
            var qrWriter = new ZXing.BarcodeWriterPixelData
            {
                Format = ZXing.BarcodeFormat.QR_CODE,
                Options = new EncodingOptions
                {
                    Height = 500,
                    Width = 500
                }
            };

            var pixelData = qrWriter.Write(data);
            using (var bitmap = new Bitmap(pixelData.Width, pixelData.Height, System.Drawing.Imaging.PixelFormat.Format32bppRgb))
            {
                using (var ms = new MemoryStream())
                {
                    var bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                                                       System.Drawing.Imaging.ImageLockMode.WriteOnly,
                                                       bitmap.PixelFormat);
                    System.Runtime.InteropServices.Marshal.Copy(pixelData.Pixels, 0, bitmapData.Scan0, pixelData.Pixels.Length);
                    bitmap.UnlockBits(bitmapData);
                    bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    ms.Position = 0;

                    var imageUrl = await _cloudinaryService.UploadImageAsync(ms, "qrcode.png");
                    return imageUrl;
                }
            }
        }

        public Task<APIResponseModel> GetBookingFlowAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<APIResponseModel> GetBookingFlowByZaloIdAsync(AccountZaloIdModel accountZaloIdModel)
        {
            try
            {
                var zaloAccount = await _unitOfWork.AccountRepository.GetFirstOrDefaultAsync(query => query
                                  .Where(a => a.ZaloUser == accountZaloIdModel.ZaloUser));
                if (zaloAccount == null)
                {
                    return new APIResponseModel
                    {
                        Message = "Account not found.",
                        IsSuccess = false,
                    };
                }

                var utcNowDate = DateTime.Now.Date;
                var allBookings = await _unitOfWork.BookingRepository.GetAllAsyncs(query => query);
                var bookings = allBookings
                    .Where(b => b.UserId == zaloAccount.Id
                        || _unitOfWork.TicketRepository.GetAllAsyncs(query => query
                            .Where(t => t.BookingId == b.BookingId && t.PhoneNumberReference == zaloAccount.PhoneNumber))
                            .Result.Any())
                    .ToList();

                if (!bookings.Any())
                {
                    return new APIResponseModel
                    {
                        Message = "No bookings found for this user.",
                        IsSuccess = false,
                    };
                }
                foreach (var booking in bookings)
                {
                    if (booking.ExpirationDate.HasValue && booking.ExpirationDate.Value.Date < utcNowDate)
                    {
                        var ticketsToUpdate = await _unitOfWork.TicketRepository.GetAllAsyncs(query => query
                            .Where(t => t.BookingId == booking.BookingId && t.Status == 1));
                        foreach (var ticket in ticketsToUpdate)
                        {
                            ticket.Status = 0;
                            _unitOfWork.TicketRepository.UpdateAsync(ticket);
                        }

                        var servicesToUpdate = await _unitOfWork.ServiceUsedByTicketRepository.GetAllAsyncs(query => query
                            .Where(s => ticketsToUpdate.Select(t => t.TicketId).Contains(s.TicketId) && s.Status == 1));
                        foreach (var service in servicesToUpdate)
                        {
                            service.Status = 0;
                            _unitOfWork.ServiceUsedByTicketRepository.UpdateAsync(service);
                        }

                        if (booking.Status == 1)
                        {
                            booking.Status = 0;
                            _unitOfWork.BookingRepository.UpdateAsync(booking);
                        }
                    }
                }

                _unitOfWork.Save();



                var bookingIds = bookings.Select(b => b.BookingId).ToList();
                var tickets = await _unitOfWork.TicketRepository.GetAllAsyncs(query => query
                                                            .Where(t => bookingIds.Contains(t.BookingId)));
                if (tickets == null || !tickets.Any())
                {
                    return new APIResponseModel
                    {
                        Message = "No tickets found for this user.",
                        IsSuccess = false,
                    };
                }

                var bookingWithTickets = new List<object>();

                foreach (var booking in bookings.OrderByDescending(b => b.CreateDate).ThenByDescending(c => c.Status == 1))
                {
                    var dailyTourDetails = await GetDailyTourDetails(booking.DailyTourId);

                    var ticketsForBooking = tickets
                        .Where(t => t.BookingId == booking.BookingId
                            && (zaloAccount.Id == booking.UserId || t.PhoneNumberReference == zaloAccount.PhoneNumber))
                        .Select(t => new
                        {
                            t.TicketId,
                            t.DailyTicketId,
                            t.TicketName,
                            t.Price,
                            t.Quantity,
                            PhoneNumberReference = t.PhoneNumberReference.StartsWith("84")
                                ? "0" + t.PhoneNumberReference[2..]
                                : t.PhoneNumberReference,
                            t.Status,
                            t.CreateDate,
                            QR = t.QRImgUrl
                        }).ToList();
                    var FullName = await _unitOfWork.AccountRepository.GetFirstOrDefaultAsync(query=>query.Where(a=>a.Id == booking.UserId));
                    var hasFeedbacks = await _unitOfWork.FeedbackRepository.GetAllAsyncs(query => query
                                            .Where(f=>f.BookingId == booking.BookingId &&f.Status==1));
                    var hasRates = await _unitOfWork.RateRepository.GetAllAsyncs(query => query
                                            .Where(r => r.BookingId == booking.BookingId && r.Status == 1));
                    var hasFeedback = hasFeedbacks.Any();
                    var hasRate = hasRates.Any();

                    if (ticketsForBooking.Any())
                    {
                        bookingWithTickets.Add(new
                        {
                            booking.BookingId,
                            booking.UserId,
                            FullName = FullName.FullName,
                            PhoneOwner = FullName.PhoneNumber.StartsWith("84")
                                                             ? "0" + FullName.PhoneNumber[2..]
                                                             : FullName.PhoneNumber,
                            booking.DailyTourId,
                            booking.BookingDate,
                            booking.ExpirationDate,
                            booking.TotalPrice,
                            booking.Status,
                            booking.CreateDate,
                            HasFeedbackAndRate = hasFeedback && hasRate,
                            IsOwner = zaloAccount.Id == booking.UserId,
                            DailyTourDetails = dailyTourDetails,
                            Tickets = ticketsForBooking,
                        });
                    }
                }

                if (!bookingWithTickets.Any())
                {
                    return new APIResponseModel
                    {
                        Message = "No tickets found for the specified phone number.",
                        IsSuccess = false,
                    };
                }

                return new APIResponseModel
                {
                    Message = "Found booking",
                    IsSuccess = true,
                    Data = new
                    {
                        Bookings = bookingWithTickets,
                    }
                };
            }
            catch (Exception ex)
            {
                return new APIResponseModel
                {
                    Message = ex.Message,
                    IsSuccess = false,
                };
            }
        }


        public async Task<APIResponseModel> ShareTicketByPhoneNumber(BookingPhoneNumberShareTicket updateModel)
        {
            try
            {
                var ticket = await _unitOfWork.TicketRepository.GetFirstOrDefaultAsync(query => query
                                 .Where(a => a.TicketId == updateModel.TicketId));
                if (ticket == null)
                {
                    return new APIResponseModel
                    {
                        Message = "Ticket not found.",
                        IsSuccess = false,
                    };
                }else if (ticket.Status == 9)
                {
                    return new APIResponseModel
                    {
                        Message = "Ticket is in process so cannot be updated.",
                        IsSuccess = false,
                    };
                }

                string formattedPhoneNumber = updateModel.PhoneNumber.Trim();
                if (formattedPhoneNumber.StartsWith("0"))
                {
                    formattedPhoneNumber = "84" + formattedPhoneNumber.Substring(1);
                }

                var createDate = ticket.CreateDate;

                ticket.PhoneNumberReference = formattedPhoneNumber;
                ticket.CreateDate = createDate;
                ticket.UpdateDate = DateTime.Now;

                var result = await _unitOfWork.TicketRepository.UpdateAsync(ticket);

                var userId = await _unitOfWork.TicketRepository.GetFirstOrDefaultAsync(query => query.Where(t=>t.TicketId==ticket.TicketId).
                                                           Include(b=>b.Bookings).ThenInclude(a=>a.Accounts));

                var notification = new BusinessObjects.Models.Notification
                {
                    NotifyId = Guid.NewGuid().ToString(),
                    UserId = userId?.Bookings?.Accounts?.Id,
                    SendDate = DateTime.Now,
                    Message = $"Bạn đã chia sẻ thành công vé cho số điện thoại {updateModel.PhoneNumber}" ,
                    Title= "Success",
                    Type = "Thành công",
                    Status = 1,
                };
                await _unitOfWork.NotificationRepository.AddAsync(notification);

                var userIdReceive = await _unitOfWork.AccountRepository.GetFirstOrDefaultAsync(query => query.Where(a=>a.PhoneNumber== formattedPhoneNumber));
                var notificationRe = new BusinessObjects.Models.Notification
                {
                    NotifyId = Guid.NewGuid().ToString(),
                    UserId = userIdReceive.Id,
                    SendDate = DateTime.Now,
                    Message = $"Bạn đã nhận được vé do số điện thoại {userId?.Bookings?.Accounts?.PhoneNumber} chia sẻ",
                    Title = "Chia sẻ thành công",
                    Type = "Success",
                    Status = 1,
                };
                await _unitOfWork.NotificationRepository.AddAsync(notificationRe);


                _unitOfWork.Save();

                //var userId = await _unitOfWork.BookingRepository.GetFirstOrDefaultAsync(query => 
                //                                                 query.Include(a=>a.Accounts).Include(d => d.DailyTours)
                //                                                      .Where(c=>c.BookingId== ticket.BookingId));

              //  SendSMSNotification(formattedPhoneNumber, userId.Accounts.PhoneNumber, userId.Accounts.FullName, userId.DailyTours.DailyTourName);

                return new APIResponseModel
                {
                    Message = "Ticket Updated Successfully",
                    IsSuccess = true,
                };
            }
            catch (Exception ex)
            {
                return new APIResponseModel
                {
                    Message = ex.Message,
                    IsSuccess = false,
                };
            }
        }
        //private void SendSMSNotification(string phoneNumberReceive, string phoneNumberSend,string fullName, string dailyTourName)
        //{
        //    var accountSid = _twilioSettings.AccountSID;
        //    var authToken = _twilioSettings.AuthToken;
        //    var twilioPhoneNumber = _twilioSettings.TwilioPhoneNumber;

        //    TwilioClient.Init(accountSid, authToken);

        //    var message = MessageResource.Create(
        //        to: new PhoneNumber("+"+phoneNumberReceive),
        //        from: new PhoneNumber(twilioPhoneNumber),
        //        body: $"AvatarTour xin thông báo cho bạn đã có anh {fullName} với số điện thoại {phoneNumberSend} vừa gửi cho bạn vé đi tham quan du lịch{dailyTourName}");

        //    Console.WriteLine($"SMS sent to {phoneNumberReceive}: {message.Sid}");
        //}

        public async Task<APIResponseModel> UpdateBookingStatusFailPaymentAsync(BookingFlowModel updateModel)
        {
            try
            {
                var booking = await _unitOfWork.BookingRepository.GetFirstsOrDefaultAsync(b => b.BookingId == updateModel.BookingId);
                if (booking == null)
                {
                    return new APIResponseModel
                    {
                        Message = "Booking not found.",
                        IsSuccess = false,
                    };
                }

                booking.Status = -1;
                booking.UpdateDate = DateTime.Now;
                await _unitOfWork.BookingRepository.UpdateAsync(booking);


                var tickets = await _unitOfWork.TicketRepository.GetAllAsyncs(query => query
                                                            .Where(t => t.BookingId == updateModel.BookingId));
                foreach (var ticket in tickets)
                {
                    ticket.Status = -1;
                    ticket.UpdateDate = DateTime.Now;
                    await _unitOfWork.TicketRepository.UpdateAsync(ticket);
                }

                foreach (var ticket in tickets)
                {
                    var servicesUsedByTicket = await _unitOfWork.ServiceUsedByTicketRepository.GetAllAsyncs(query => query
                                                                                             .Where(s => s.TicketId == ticket.TicketId));
                    foreach (var serviceUsed in servicesUsedByTicket)
                    {
                        serviceUsed.Status = -1;
                        serviceUsed.UpdateDate = DateTime.Now;
                        await _unitOfWork.ServiceUsedByTicketRepository.UpdateAsync(serviceUsed);
                    }
                }

                _unitOfWork.Save();

                return new APIResponseModel
                {
                    Message = "Status updated successfully for booking and related records.",
                    IsSuccess = true,
                };
            }
            catch (Exception ex)
            {
                return new APIResponseModel
                {
                    Message = ex.Message,
                    IsSuccess = false,
                };
            }
        }

        public async Task<APIResponseModel> UpdateBookingStatusAsync(BookingFlowModel updateModel)
        {
            try
            {
                var booking = await _unitOfWork.BookingRepository.GetFirstsOrDefaultAsync(b => b.BookingId == updateModel.BookingId);
                if (booking == null)
                {
                    return new APIResponseModel
                    {
                        Message = "Booking not found.",
                        IsSuccess = false,
                    };
                }

                booking.Status = 1;
                await _unitOfWork.BookingRepository.UpdateAsync(booking);

                
                var tickets = await _unitOfWork.TicketRepository.GetAllAsyncs(query => query
                                                            .Where(t => t.BookingId == updateModel.BookingId));
                foreach (var ticket in tickets)
                {
                    ticket.Status = 1;
                    await _unitOfWork.TicketRepository.UpdateAsync(ticket);
                }

                foreach (var ticket in tickets)
                {
                    var servicesUsedByTicket = await _unitOfWork.ServiceUsedByTicketRepository.GetAllAsyncs(query => query
                                                                                             .Where(s=> s.TicketId == ticket.TicketId));
                    foreach (var serviceUsed in servicesUsedByTicket)
                    {
                        serviceUsed.Status = 1;
                        await _unitOfWork.ServiceUsedByTicketRepository.UpdateAsync(serviceUsed);
                    }
                }

                _unitOfWork.Save();

                return new APIResponseModel
                {
                    Message = "Status updated successfully for booking and related records.",
                    IsSuccess = true,
                };
            }
            catch (Exception ex)
            {
                return new APIResponseModel
                {
                    Message = ex.Message,
                    IsSuccess = false,
                };
            }
        }

        public async Task<DailyTourDetailModel> GetDailyToursDetails(string dailyTourId)
        {
            try
            {
                // Truy vấn lấy DailyTour với các điều kiện lọc ban đầu
                var dailyTour = await _unitOfWork.DailyTourRepository.GetFirstOrDefaultAsync(query => query
                    .Where(dt => dt.DailyTourId == dailyTourId && dt.Status == 1)
                    .Include(dt => dt.PackageTours)
                        .ThenInclude(pt => pt.TourSegments)
                            .ThenInclude(des => des.Destinations)
                                .ThenInclude(lo => lo.Locations)
                                    .ThenInclude(l => l.Services)
                                        .ThenInclude(sbt => sbt.ServiceByTourSegments)
                    .Include(dt => dt.DailyTickets)
                        .ThenInclude(dt => dt.TicketTypes)
                );

                if (dailyTour == null)
                {
                    return null;
                }

                // Khởi tạo model để trả về, áp dụng điều kiện lọc cho từng phần tử
                var dailyTourDetails = new DailyTourDetailModel
                {
                    DailyTourId = dailyTour.DailyTourId,
                    DailyTourName = dailyTour.DailyTourName,
                    Description = dailyTour.Description,
                    DailyTourPrice = dailyTour.DailyTourPrice,
                    ImgUrl = dailyTour.ImgUrl,
                    ExpirationDate = dailyTour.ExpirationDate,
                    StartDate = dailyTour.StartDate,
                    EndDate = dailyTour.EndDate,
                    Discount = dailyTour.Discount,
                    PackageTour = new PackageToursModel
                    {
                        PackageTourId = dailyTour.PackageTours?.PackageTourId,
                        PackageTourName = dailyTour.PackageTours?.PackageTourName,
                        PackageTourImgUrl = dailyTour.PackageTours?.PackageTourImgUrl,
                        CityId = dailyTour.PackageTours?.CityId,
                        StatusPackageTour = dailyTour.PackageTours?.Status,
                        CityName = dailyTour.PackageTours?.Cities?.CityName,
                        TourSegments = dailyTour.PackageTours?.TourSegments
                            .Where(ts => ts.Status == 1 && ts.Destinations?.Status == 1)
                            .Select(ts => new TourSegmentsModel
                            {
                                TourSegmentId = ts.TourSegmentId,
                                DestinationId = ts.DestinationId,
                                DestinationName = ts.Destinations?.DestinationName,
                                DestinationAddress = ts.Destinations?.DestinationAddress,
                                DestinationImgUrl = ts.Destinations?.DestinationImgUrl,
                                DestinationHotline = ts.Destinations?.DestinationHotline,
                                DestinationGoogleMap = ts.Destinations?.DestinationGoogleMap,
                                DestinationOpeningDate = ts.Destinations?.DestinationOpeningDate,
                                DestinationClosingDate = ts.Destinations?.DestinationClosingDate,
                                DestinationOpeningHours = ts.Destinations?.DestinationOpeningHours,
                                DestinationClosingHours = ts.Destinations?.DestinationClosingHours,
                                Locations = ts.Destinations?.Locations
                                    .Where(c => c.Status == 1 &&
                                                c.DestinationId == ts.DestinationId &&
                                                dailyTour.PackageTours?.PackageTourId == ts.PackageTourId &&
                                                ts.ServiceByTourSegments.Any(sbts => sbts.Services?.LocationId == c.LocationId && sbts.Status == 1))
                                    .Select(lo => new LocationsModel
                                    {
                                        LocationId = lo.LocationId,
                                        LocationName = lo.LocationName,
                                        LocationImgUrl = lo.LocationImgUrl,
                                        LocationHotline = lo.LocationHotline,
                                        LocationOpeningHours = lo.LocationOpeningHours,
                                        LocationClosingHours = lo.LocationClosingHours,
                                        LocationGoogleMap = lo.LocationGoogleMap,
                                        DestinationId = lo.DestinationId,
                                        Services = ts.ServiceByTourSegments
                                            .Where(sbts => sbts.Services?.LocationId == lo.LocationId && sbts.Services?.Status == 1)
                                            .Select(se => new ServicesModel
                                            {
                                                ServiceId = se.Services?.ServiceId,
                                                ServiceName = se.Services?.ServiceName,
                                                ServicePrice = se.Services?.ServicePrice,
                                                SupplierName = se.Services?.Suppliers?.SupplierName,
                                                LocationId = se.Services?.LocationId,
                                                ServiceTypeName = se.Services?.ServiceTypes?.ServiceTypeName,
                                            })
                                            .ToList()
                                    })
                                    .ToList()
                            })
                            .ToList()
                    }
                };

                return dailyTourDetails;
            }
            catch
            {
                return null;
            }
        }

        public async Task<APIResponseModel> UpdateTicketByQR(TicketUsageViewModel ticketUsageViewModel)
        {
            try
            {
                var ticket = await _unitOfWork.TicketRepository.GetFirstsOrDefaultAsync(b => b.TicketId == ticketUsageViewModel.TicketId);
                if (ticket == null)
                {
                    return new APIResponseModel
                    {
                        Message = "Ticket not found.",
                        IsSuccess = false,
                    };
                } else if (ticket.Status ==9)
                {
                    return new APIResponseModel
                    {
                        Message = "Tickets are pending payment.",
                        IsSuccess = false,
                    };
                } else if (ticket.Status == 4)
                {
                    return new APIResponseModel
                    {
                        Message = "Ticket has been used.",
                        IsSuccess = false,
                    };
                } else if (ticket.Status == 2)
                {
                    return new APIResponseModel
                    {
                        Message = "Ticket has been cancelled.",
                        IsSuccess = false,
                    };
                } else if (ticket.Status == -1)
                {
                    return new APIResponseModel
                    {
                        Message = "Ticket cannot be used because booking payment failed.",
                        IsSuccess = false,
                    };
                } else if (ticket.Status == 5)
                {
                    return new APIResponseModel
                    {
                        Message = "Ticket has been refunded.",
                        IsSuccess = false,
                    };
                } else if (ticket.Status == 0)
                {
                    return new APIResponseModel
                    {
                        Message = "Ticket has been disabled.",
                        IsSuccess = false,
                    };
                } else if (ticket.Status == 3)
                {
                    return new APIResponseModel
                    {
                        Message = "Ticket has been disabled.",
                        IsSuccess = false,
                    };
                }

                bool isDestinationValid = ticketUsageViewModel.Destination
                                         .Any(d => d.DestinationIds == ticketUsageViewModel.MobileDestinationId);
                if (!isDestinationValid)
                {
                    return new APIResponseModel
                    {
                        Message = "Tickets do not exist at this destination.",
                        IsSuccess = false,
                    };
                }

                if (ticketUsageViewModel.ExpirationDate.HasValue)
                {
                    var expirationDate = ticketUsageViewModel.ExpirationDate.Value.Date; 
                    var currentDate = DateTime.Now.Date;

                    if (expirationDate < currentDate)
                    {
                        return new APIResponseModel
                        {
                            Message = "Ticket has expired.",
                            IsSuccess = false,
                        };
                    }
                    else if (expirationDate > currentDate)
                    {
                        return new APIResponseModel
                        {
                            Message = "Ticket is not yet valid for use.",
                            IsSuccess = false,
                        };
                    }
                }
                else
                {
                    return new APIResponseModel
                    {
                        Message = "Expiration date is not set.",
                        IsSuccess = false,
                    };
                }

                var bookingStatus = await _unitOfWork.BookingRepository.GetAllAsyncs(query=>query.Where(s=>s.BookingId== ticket.BookingId));
                if (bookingStatus.All(s => s.Status == 4))
                {
                    return new APIResponseModel
                    {
                        Message = "Ticket has been used.",
                        IsSuccess = false,
                    };
                }

                var serviceIds = await GetServiceIdByDailyTourIdAndDestinationId(ticketUsageViewModel.DailyTourId, ticketUsageViewModel.MobileDestinationId);
                if (serviceIds == null || !serviceIds.Any())
                {
                    return new APIResponseModel
                    {
                        Message = "No services found for the provided Daily Tour and Destination.",
                        IsSuccess = false,
                    };
                }
                var servicesUsed = await _unitOfWork.ServiceUsedByTicketRepository
                                           .GetAllAsyncs(query => query.Where(s =>s.TicketId == ticketUsageViewModel.TicketId && serviceIds.Contains(s.ServiceId)));
             //   bool hasServiceUsedStatus4 = servicesUsed.All(s => s.Status == 4);
                if (servicesUsed.All(s => s.Status == 4))
                {
                    return new APIResponseModel
                    {
                        Message = "Ticket has been used in this destination",
                        IsSuccess = false,
                    };
                }
                if (servicesUsed.Any())
                {
                    foreach (var service in servicesUsed)
                    {
                        var createDate = service.CreateDate;
                        service.Status = 4; 
                        service.UpdateDate = DateTime.Now;
                        service.CreateDate = createDate;
                        _unitOfWork.ServiceUsedByTicketRepository.UpdateAsync(service); 
                    }
                    _unitOfWork.Save();
                }
                
                               
                var hasServiceUsedStatus4 = await _unitOfWork.ServiceUsedByTicketRepository
                                                .GetAllAsyncs(query => query.Where(s => s.TicketId == ticket.TicketId));

                if (hasServiceUsedStatus4.All(s=> s.Status==4))
                {
                    var createDate = ticket.CreateDate;
                    ticket.Status = 4;
                    ticket.UpdateDate = DateTime.Now;
                    ticket.CreateDate = createDate;
                    _unitOfWork.TicketRepository.UpdateAsync(ticket); 
                    _unitOfWork.Save();

                    var bookingTickets = await _unitOfWork.TicketRepository.GetAllAsyncs(query => query.Where(b => b.BookingId == ticket.BookingId));
                    if (bookingTickets.All(t => t.Status == 4))
                    {
                        var booking = await _unitOfWork.BookingRepository.GetFirstsOrDefaultAsync(b => b.BookingId == ticket.BookingId);
                        if (booking != null)
                        {
                            var createDateBooking = booking.CreateDate;
                            booking.Status = 4;
                            booking.UpdateDate = DateTime.Now;
                            booking.CreateDate = createDateBooking;
                            _unitOfWork.BookingRepository.UpdateAsync(booking);
                            _unitOfWork.Save();
                        }
                           
                    }

                    return new APIResponseModel
                    {
                        Message = "Status updated successfully for tickets.",
                        IsSuccess = true,
                    };
                }

                return new APIResponseModel
                {
                    Message = "Status updated successfully for tickets.",
                    IsSuccess = true,
                };
            }
            catch (Exception ex)
            {
                return new APIResponseModel
                {
                    Message = ex.Message,
                    IsSuccess = false,
                };
            }
        }
        public async Task<List<string>> GetServiceIdByDailyTourIdAndDestinationId(string dailyTourId, string destinationId)
        {
            try
            {
                var dailyTour = await _unitOfWork.DailyTourRepository.GetFirstOrDefaultAsync(query => query
                    .Where(dt => dt.DailyTourId == dailyTourId )
                    .Include(dt => dt.PackageTours)
                        .ThenInclude(pt => pt.TourSegments)
                            .ThenInclude(ts => ts.Destinations)
                                .ThenInclude(d => d.Locations)
                                    .ThenInclude(l => l.Services)
                                        .ThenInclude(sbt => sbt.ServiceByTourSegments)
                    .Include(dt => dt.DailyTickets)
                        .ThenInclude(dt => dt.TicketTypes)
                );

                if (dailyTour == null)
                {
                    return null;
                }

                var pointOfI = await _unitOfWork.PointOfInterestRepository.GetAllAsyncs(query => query);

                var servicesForDestination = dailyTour.PackageTours?.TourSegments
                   // .Where(ts => ts.Status == 1 && ts.Destinations?.Status == 1)
                    .SelectMany(ts => ts.Destinations?.Locations
                        .Where(l => l.DestinationId == destinationId)
                        .SelectMany(l => l.Services
                            //.Where(s => s.Status == 1)
                            .Select(s => s.ServiceId))
                        ?? Enumerable.Empty<string>())
                    .Distinct()
                    .ToList();

                return servicesForDestination; // Trả về danh sách ServiceId
            }
            catch
            {
                return null;
            }
        }

        public async Task<object> GetDailyTourDetails(string dailyTourId)
        {
            try
            {
                var dailyTour = await _unitOfWork.DailyTourRepository.GetFirstOrDefaultAsync(query => query
                    .Where(dt => dt.DailyTourId == dailyTourId )
                    .Include(dt => dt.PackageTours)
                        .ThenInclude(pt => pt.TourSegments)
                            .ThenInclude(des => des.Destinations)
                                .ThenInclude(lo => lo.Locations)
                                    .ThenInclude(l => l.Services)
                                        .ThenInclude(sbt => sbt.ServiceByTourSegments)
                    .Include(dt => dt.DailyTickets)
                        .ThenInclude(dt => dt.TicketTypes)
                );

                if (dailyTour == null)
                {
                    return null;
                }

                var pointOfI = await _unitOfWork.PointOfInterestRepository.GetAllAsyncs(query => query);

                return new
                {
                    DailyTour = new
                    {
                        dailyTour.DailyTourId,
                        dailyTour.PackageTourId,
                        dailyTour.DailyTourName,
                        dailyTour.Description,
                        dailyTour.DailyTourPrice,
                        dailyTour.ImgUrl,
                        dailyTour.ExpirationDate,
                        dailyTour.StartDate,
                        dailyTour.EndDate,
                        dailyTour.Discount,
                        StatusDailyTour = dailyTour.Status,
                        TicketTypes = dailyTour.DailyTickets
                            .Where(c => c.DailyTourId == dailyTour.DailyTourId
                                       && c.TicketTypes?.PackageTourId == dailyTour.PackageTourId
                                       && c.Capacity > 0)
                            .Select(tt => new
                            {
                                tt.DailyTicketId,
                                tt.Capacity,
                                tt.TicketTypes?.TicketTypeName,
                                tt.TicketTypes?.MinBuyTicket,
                                tt.DailyTicketPrice,
                                tt.CreateDate
                            }).ToList()
                    },
                    PackageTour = new
                    {
                        dailyTour.PackageTours?.PackageTourId,
                        dailyTour.PackageTours?.PackageTourName,
                        dailyTour.PackageTours?.PackageTourImgUrl,
                        dailyTour.PackageTours?.CityId,
                        StatusPackageTour = dailyTour.PackageTours?.Status,
                        dailyTour.PackageTours?.Cities?.CityName,
                        TourSegments = dailyTour.PackageTours?.TourSegments
                        //    .Where(ts => ts.Status == 1 && ts.Destinations?.Status == 1)
                            .Select(ts => new
                            {
                                ts.TourSegmentId,
                                ts.DestinationId,
                                ts.Destinations?.DestinationName,
                                ts.Destinations?.DestinationAddress,
                                ts.Destinations?.DestinationImgUrl,
                                ts.Destinations?.DestinationHotline,
                                ts.Destinations?.DestinationGoogleMap,
                                ts.Destinations?.DestinationOpeningDate,
                                ts.Destinations?.DestinationClosingDate,
                                ts.Destinations?.DestinationOpeningHours,
                                ts.Destinations?.DestinationClosingHours,
                                StatusDestinations = ts.Destinations?.Status,
                                Locations = ts.Destinations?.Locations
                                    .Where(c => //c.Status == 1 &&
                                                c.DestinationId == ts.DestinationId &&
                                                dailyTour.PackageTours?.PackageTourId == ts.PackageTourId &&
                                                ts.ServiceByTourSegments.Any(sbts => sbts.Services?.LocationId == c.LocationId))
                                    .Select(lo => new
                                    {
                                        lo.LocationId,
                                        lo.LocationName,
                                        lo.LocationImgUrl,
                                        lo.LocationHotline,
                                        lo.LocationOpeningHours,
                                        lo.LocationClosingHours,
                                        lo.LocationGoogleMap,
                                        lo.DestinationId,
                                        PointOfInterests = pointOfI.Where(poi => poi.LocationId == lo.LocationId).Select(type => new
                                        {
                                            type.PointId,
                                            type.PointName,
                                        }),
                                        StatusLocation = lo.Status,
                                        Services = ts.ServiceByTourSegments
                                            .Where(sbts => sbts.Services?.LocationId == lo.LocationId
                                                           )
                                            .Select(se => new
                                            {
                                                se.Services?.ServiceId,
                                                se.Services?.ServiceName,
                                                se.Services?.ServicePrice,
                                                se.Services?.Suppliers?.SupplierName,
                                                se.Services?.LocationId,
                                                se.Services?.ServiceTypes?.ServiceTypeName,
                                                StatusServices = se.Services?.Status,
                                            }).ToList(),
                                    }).ToList(),
                            }).ToList(),
                    }
                };
            }
            catch
            {
                return null;
            }
        }



        public async Task<APIResponseModel> UpdateBookingByZaloIdFlowAsync(BookingModel updateModel)
        {
            var zaloAccount = await _unitOfWork.AccountRepository.GetFirstOrDefaultAsync(query => query
                    .Where(a => a.Id == updateModel.UserId && (a.Status == 1 && a.ZaloUser != null ) ));
                
            if (zaloAccount == null)
            {
                return new APIResponseModel
                {
                    Message = "Account not found.",
                    IsSuccess = false,
                };
            }
            var booking = await _unitOfWork.BookingRepository.GetFirstOrDefaultAsync(query => query
                    .Where(b => b.UserId == zaloAccount.Id && b.Status == 1));
            if (booking == null)
            {
                return new APIResponseModel
                {
                    Message = "Booking not found for the given account.",
                    IsSuccess = false
                };
            }

            booking.BookingDate = updateModel.BookingDate;
            booking.ExpirationDate = updateModel.ExpirationDate;
            booking.TotalPrice = updateModel.TotalPrice;
            booking.Status = updateModel.Status;
            booking.UpdateDate = DateTime.Now;

            _unitOfWork.BookingRepository.UpdateAsync(booking);
            _unitOfWork.Save();

            return new APIResponseModel
            {
                Message = "Booking updated successfully.",
                IsSuccess = true,
                //Data = booking 
            };
        }

        public Task<APIResponseModel> UpdateBookingFlowAsync(BookingUpdateModel updateModel)
        {
            throw new NotImplementedException();
        }

        public async Task<APIResponseModel> CheckBookingQuantity(BookingFlowCreateModel createModel)
        {
            using var transaction = _unitOfWork.BeginTransaction();
            try
            {
                var dailyTickets = await _unitOfWork.DailyTicketRepository.GetAllAsyncs(query =>
                                  query.Where(dt => dt.DailyTourId == createModel.DailyTourId));

                if (dailyTickets == null || !dailyTickets.Any())
                {
                    return new APIResponseModel
                    {
                        Message = "No tickets found for the specified DailyTourId.",
                        IsSuccess = false
                    };
                }

                foreach (var ticketModel in createModel.Tickets)
                {
                    var dailyTicket = dailyTickets.FirstOrDefault(dt => dt.DailyTicketId == ticketModel.DailyTicketId);
                    if (dailyTicket == null)
                    {
                        return new APIResponseModel
                        {
                            Message = $"Ticket with ID {ticketModel.DailyTicketId} does not exist.",
                            IsSuccess = false
                        };
                    }
                    var bookedTickets = await _unitOfWork.TicketRepository.GetAllAsyncs(query =>
                        query.Where(t => t.DailyTicketId == ticketModel.DailyTicketId && t.Status == 1));

                    int totalBookedQuantity =(int) bookedTickets.Sum(t => t.Quantity);

                    // Kiểm tra số lượng khả dụng
                    if (totalBookedQuantity + ticketModel.TotalQuantity > dailyTicket.Capacity)
                    {
                        return new APIResponseModel
                        {
                            Message = $"Not enough tickets available for {dailyTicket.TicketTypes.TicketTypeName}.",
                            IsSuccess = false
                        };
                    }
                }

                transaction.Commit();
                return new APIResponseModel
                {
                    Message = "Booking quantity is valid.",
                    IsSuccess = true
                };

            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return new APIResponseModel
                {
                    Message = $"Booking and ticket creation failed: {ex.Message}",
                    IsSuccess = false,
                };
            }
        }
    }

    //public class TwilioSettings
    //{
    //    public string AccountSID { get; set; }
    //    public string AuthToken { get; set; }
    //    public string TwilioPhoneNumber { get; set; }
    //}
}
