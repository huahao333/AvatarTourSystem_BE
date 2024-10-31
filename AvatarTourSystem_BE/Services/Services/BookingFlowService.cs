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
using QRCoder;
using System.Drawing;
using System.IO;
using ZXing.QrCode.Internal;
using ZXing;
using ZXing.QrCode;
using ZXing.Common;
using BusinessObjects.ViewModels.Rate;
using BusinessObjects.ViewModels.Account;
using Google.Apis.Storage.v1.Data;
using static QRCoder.PayloadGenerator;

namespace Services.Services
{
    public class BookingFlowService : IBookingFlowService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly CloudinaryService _cloudinaryService;
        public BookingFlowService(IUnitOfWork unitOfWork, IMapper mapper, CloudinaryService cloudinaryService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cloudinaryService = cloudinaryService;
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

        public async Task<APIResponseModel> CreateBookingFlowAsync(BookingFlowCreateModel createModel)
        {
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

                var newBookingId = Guid.NewGuid();
                var newBooking = new Booking
                {
                    BookingId = newBookingId.ToString(),
                    UserId = zaloAccount.Id,
                    DailyTourId = createModel.DailyTourId,
                    BookingDate = DateTime.Now,
                    ExpirationDate = DateTime.UtcNow.AddDays(2),
                    TotalPrice = createModel.TotalPrice,
                    Status = 9,
                    CreateDate = DateTime.UtcNow,
                };
                await _unitOfWork.BookingRepository.AddAsync(newBooking);

                List<string> ticketIds = new List<string>();

                foreach (var ticket in createModel.Tickets)
                {
                    for (int i = 0; i < ticket.TotalQuantity; i++)
                    {
                        var qrContent = $"BookingId: {newBooking.BookingId}, TicketTypeId: {ticket.DailyTicketId}, TicketIndex: {i + 1}";
                        var qrImageUrl = await GenerateQRCode(qrContent);
                        var newTicket = new Ticket
                        {
                            TicketId = Guid.NewGuid().ToString(),
                            BookingId = newBooking.BookingId,
                            DailyTicketId = ticket.DailyTicketId,
                            TicketName = ticket.TicketName,
                            Price = ticket.TotalPrice,
                            QRImgUrl = qrImageUrl,
                            PhoneNumberReference = zaloAccount.PhoneNumber,
                            Quantity = 1,  
                            Status = 9,
                            CreateDate = DateTime.UtcNow,
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
                                Status = 9
                            };
                            await _unitOfWork.ServiceUsedByTicketRepository.AddAsync(serviceUsedByTicket);
                        }
                    }
                }
                _unitOfWork.Save();

                //var qrContent = $"BookingId: {newBooking.BookingId}, TicketIds: {string.Join(",", ticketIds)}";
                //var qrImageUrl = await GenerateQRCode(qrContent);

                //foreach (var ticketId in ticketIds)
                //{
                //    var ticket = await _unitOfWork.TicketRepository.GetFirstsOrDefaultAsync(t => t.TicketId == ticketId);
                //    if (ticket != null)
                //    {
                //        ticket.QRImgUrl = qrImageUrl;
                //        await _unitOfWork.TicketRepository.UpdateAsync(ticket);
                //    }
                //}
                //_unitOfWork.Save();

                return new APIResponseModel
                {
                    Message = "Booking and tickets created successfully.",
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

                //var bookings = await _unitOfWork.BookingRepository.GetAllAsyncs(query => query
                //           .Where(b => b.UserId == zaloAccount.Id));

                //if (bookings == null || !bookings.Any())
                //{
                //    return new APIResponseModel
                //    {
                //        Message = "No bookings found for this user.",
                //        IsSuccess = false,
                //    };
                //}

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

                foreach (var booking in bookings)
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

                    if (ticketsForBooking.Any())
                    {
                        bookingWithTickets.Add(new
                        {
                            booking.BookingId,
                            booking.UserId,
                            booking.DailyTourId,
                            booking.BookingDate,
                            booking.ExpirationDate,
                            booking.TotalPrice,
                            booking.Status,
                            booking.CreateDate,
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
                _unitOfWork.Save();

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

        public async Task<object> GetDailyTourDetails(string dailyTourId)
        {
            try
            {
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
                            .Where(ts => ts.Status == 1 && ts.Destinations?.Status == 1)
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
                                    .Where(c => c.Status == 1 &&
                                                c.DestinationId == ts.DestinationId &&
                                                dailyTour.PackageTours?.PackageTourId == ts.PackageTourId &&
                                                ts.ServiceByTourSegments.Any(sbts => sbts.Services?.LocationId == c.LocationId &&
                                                                                      sbts.Status == 1))
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
                                                           && sbts.Services?.Status == 1)
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
            booking.UpdateDate = DateTime.UtcNow;

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
    }
}
