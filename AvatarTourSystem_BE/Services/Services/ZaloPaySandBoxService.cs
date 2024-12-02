using BusinessObjects.Models;
using BusinessObjects.ViewModels.Booking;
using BusinessObjects.ViewModels.Payment;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Repositories.Interfaces;
using Services.Common;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Web;
using ZXing.Common;
using static Org.BouncyCastle.Asn1.Cmp.Challenge;
using static Services.Common.ZaloPayHelper.ZaloPayHelper;
using System.Net.Http;
using static Services.Services.ZaloPaySandBoxService;
using System.Net.Http.Json;
using CloudinaryDotNet;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using BusinessObjects.Enums;
using BusinessObjects.ViewModels.Rate;
using System.Collections.Concurrent;

namespace Services.Services
{
    public class ZaloPaySandBoxService: IZaloPaySandBoxService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly CloudinaryService _cloudinaryService;
        private readonly EncryptionHelperService _encryptionHelperService;
        private readonly HttpClient _httpClient;
        private readonly string appid = "554"; 
        private readonly string key1 = "8NdU5pG5R2spGHGhyO99HN1OhD8IQJBn";
        private readonly string refundUrl = "https://sandbox.zalopay.com.vn/v001/tpe/partialrefund"; 
        public ZaloPaySandBoxService(IUnitOfWork unitOfWork, 
            CloudinaryService cloudinaryService, 
            EncryptionHelperService encryptionHelperService,
            HttpClient httpClient)
        {
            _unitOfWork = unitOfWork;
            _cloudinaryService = cloudinaryService;
            _encryptionHelperService = encryptionHelperService;
            _httpClient = httpClient;
        }

        public async Task<APIResponseModel> HandleCallback(object callbackData)
        {
            try
            {
                if (callbackData == null)
                {
                    return new APIResponseModel { Message = "Callback data is null", IsSuccess = false };
                }
                // Chuyển đổi callback thành object JSON
                var callbackJson = callbackData.ToString();
                var callbackObject = JsonConvert.DeserializeObject<CallbackResponseViewModel>(callbackJson);

                if (callbackObject != null && callbackObject.Data != null)
                {
                    int resultCode = callbackObject.Data.ResultCode;
                    float totalAmount = (float)callbackObject.Data.Amount;
                    int transTime = callbackObject.Data.TransTime;
                    string orderId = callbackObject.Data.OrderId;
                    string tranId = callbackObject.Data.TransId;
                    string appId = callbackObject.Data.AppId;
                    string mechant = callbackObject.Data.MerchantTransId;
                    string overallMac = callbackObject.overallMac;
                    string mac = callbackObject.Mac;

                    var extradataJson = Uri.UnescapeDataString(callbackObject.Data.Extradata);
                    var extradataObject = JsonConvert.DeserializeObject<ExtraData>(extradataJson);
                    if (resultCode == 1)  
                    {
                        // Giải mã extradata
                        //var extradataJson = Uri.UnescapeDataString(callbackObject.Data.Extradata);
                        //var extradataObject = JsonConvert.DeserializeObject<ExtraData>(extradataJson);

                        if (extradataObject != null)
                        {
                            var createBookingResponse = await CreateBookingFromCallbackData(extradataObject,
                                                                                            totalAmount,
                                                                                            transTime,
                                                                                            orderId,
                                                                                            tranId,
                                                                                            appId,
                                                                                            mechant,
                                                                                            overallMac,
                                                                                            mac,
                                                                                            resultCode);



                             return createBookingResponse;



                        }
                        return new APIResponseModel { Message = "Invalid extradata", IsSuccess = false };
                    }
                    else
                    {
                        //var rollBackBooing = await RollbackBookingsFlowAsync(extradataObject,
                        //                                                     totalAmount,
                        //                                                     transTime,
                        //                                                     orderId,
                        //                                                     tranId,
                        //                                                     appId,
                        //                                                     mechant,
                        //                                                     overallMac,
                        //                                                     mac,
                        //                                                     resultCode);
                        //return rollBackBooing;
                        return new APIResponseModel { Message = $"Transaction failed. ResultCode: {resultCode}", IsSuccess = false };
                    }
                }
                return new APIResponseModel { Message = "Invalid callback data", IsSuccess = false };
            }
            catch (Exception ex)
            {
                return new APIResponseModel { Message = "Error handling callback", IsSuccess = false };
            }
        }

        public async Task<APIResponseModel> RollbackBookingsFlowAsync(ExtraData extradata,
                                                                           float totalAmount,
                                                                           int transTime,
                                                                           string orderId,
                                                                           string tranId,
                                                                           string appId,
                                                                           string mechant,
                                                                           string overallMac,
                                                                           string mac,
                                                                           int resultCode)
        {
            try
            {
                var zaloAccount = await _unitOfWork.AccountRepository.GetFirstOrDefaultAsync(query => query
                                  .Where(a => a.ZaloUser == extradata.ZaloId));
                if (zaloAccount == null)
                {
                    return new APIResponseModel
                    {
                        Message = "Account not found.",
                        IsSuccess = false,
                    };
                }

                var serviceIds = await GetServiceIdsByDailyTourId(extradata.DailyTourId);
                if (serviceIds == null || !serviceIds.Any())
                {
                    return new APIResponseModel
                    {
                        Message = "No Service IDs found.",
                        IsSuccess = false,
                    };
                }
                var dailyTourExpirationDate = await _unitOfWork.DailyTourRepository.GetFirstOrDefaultAsync(query => query
                                  .Where(a => a.DailyTourId == extradata.DailyTourId));
               
                var dailyTourDetails = await GetDailyToursDetails(extradata.DailyTourId);
                if (dailyTourDetails == null)
                {
                    return new APIResponseModel
                    {
                        Message = "Tour details not found.",
                        IsSuccess = false,
                    };
                }

                var destinationId = dailyTourDetails?.PackageTour?.TourSegments?
                                        .Select(l => l.DestinationId)
                                        .ToList();
                var destinationName = dailyTourDetails?.PackageTour?.TourSegments?
                                         .Select(l => l.DestinationName)
                                         .ToList();
                var destination = dailyTourDetails.PackageTour?.TourSegments?.ToList();
                var location = destination.SelectMany(l => l.Locations)
                                           .Select(c => c.LocationId).ToList();

                var services = destination.SelectMany(l => l.Locations)
                                           .SelectMany(s => s.Services)
                                           .Select(c => c.ServiceId).ToList();

                // Chuyển các thông tin về Destination, Location, và Services thành JSON
                var destinationIdJson = JsonConvert.SerializeObject(destinationId);
                var destinationNameJson = JsonConvert.SerializeObject(destinationName);
                var locationJson = JsonConvert.SerializeObject(location);
                var servicesJson = JsonConvert.SerializeObject(services);

                // Tạo Booking mới
                var newBookingId = Guid.NewGuid();
                var newBooking = new Booking
                {
                    BookingId = newBookingId.ToString(),
                    UserId = zaloAccount.Id,
                    DailyTourId = extradata.DailyTourId,
                    BookingDate = DateTime.Now,
                    ExpirationDate = dailyTourExpirationDate.ExpirationDate,
                    TotalPrice = totalAmount,
                    Status = -1,
                    CreateDate = DateTime.Now,
                };
                await _unitOfWork.BookingRepository.AddAsync(newBooking);

                List<string> ticketIds = new List<string>();

                foreach (var ticket in extradata.Tickets)
                {
                    for (int i = 0; i < ticket.TotalQuantity; i++)
                    {
                        var newTicketId = Guid.NewGuid().ToString();
                        var priceByTicket = await _unitOfWork.DailyTicketRepository.
                                                              GetFirstOrDefaultAsync(query => query.Where(c => c.DailyTicketId == ticket.DailyTicketId));
                        // Tạo dữ liệu QR code
                        var qrData = new
                        {
                            //BookingId = newBooking.BookingId,
                            //ZaloUser = extradata.ZaloId,
                            DailyTourId = newBooking.DailyTourId,
                            TourName = dailyTourDetails.DailyTourName,
                            //StartDate = dailyTourDetails.StartDate,
                            //EndDate = dailyTourDetails.EndDate,
                            //Discount = dailyTourDetails.Discount,
                            //DailyTourPrice = dailyTourDetails.DailyTourPrice,
                            //City = dailyTourDetails.PackageTour?.CityId,
                            DestinationId = destinationIdJson,
                            DestinationName = destinationNameJson,
                            //LocationId = locationJson,
                            //ServiceId = servicesJson,
                            //PhoneNumber = zaloAccount.PhoneNumber,
                            //BookingDate = newBooking.BookingDate,
                            //PriceOfTicket = priceByTicket.DailyTicketPrice,
                            ExpirationDate = newBooking.ExpirationDate,
                            TotalPrice = newBooking.TotalPrice,
                            TicketTypeId = newTicketId,
                            DailyTicketId = ticket.DailyTicketId,
                            TicketName = ticket.TicketName,
                            Price = ticket.TotalPrice
                        };

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
                            Status = -1,
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
                                Status = 1
                            };
                            await _unitOfWork.ServiceUsedByTicketRepository.AddAsync(serviceUsedByTicket);
                        }
                    }
                    var dailyTicketType = await _unitOfWork.DailyTicketRepository.GetAllAsyncs(query => query.Where(d => d.DailyTicketId == ticket.DailyTicketId));
                    var dailyTicketTypeCapa = dailyTicketType.FirstOrDefault();
                    if (dailyTicketTypeCapa != null)
                    {
                        dailyTicketTypeCapa.Capacity += ticket.TotalQuantity;
                        dailyTicketTypeCapa.UpdateDate = DateTime.Now;
                        await _unitOfWork.DailyTicketRepository.UpdateAsync(dailyTicketTypeCapa);
                    }
                }
                var newPaymentId = Guid.NewGuid();
                var newPayment = new Payment
                {
                    PaymentId = newPaymentId.ToString(),
                    PaymentMethodId = "1",
                    BookingId = newBooking.BookingId,
                    AppId = appId,
                    OrderId = orderId,
                    TransId = tranId,
                    TransTime = transTime,
                    Amount = totalAmount,
                    MerchantTransId = mechant,
                    Description = overallMac,
                    ResultCode = resultCode,
                    Message = mac,
                    ExtraData = JsonConvert.SerializeObject(extradata),
                    Status = -1,
                    CreateDate = DateTime.Now,
                    UpdateDate =DateTime.Now,
                };
                await _unitOfWork.PaymentRepository.AddAsync(newPayment);

                var newTransactionId = Guid.NewGuid();
                var transaction = new TransactionsHistory
                {
                    TransactionId = newTransactionId.ToString(),
                    UserId = zaloAccount.Id,
                    BookingId = newBooking.BookingId,
                    OrderId = orderId,
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    Status = -1
                };
                await _unitOfWork.TransactionsHistoryRepository.AddAsync(transaction);
                _unitOfWork.Save();

                return new APIResponseModel
                {
                    Message = "Booking payment failed.",
                    IsSuccess = true,
                };
            }
            catch (Exception ex)
            {
                return new APIResponseModel
                {
                    Message = $"Error payment booking: {ex.Message}",
                    IsSuccess = false,
                };
            }
        }


        public async Task<APIResponseModel> RollBackBookingsFlowAsync(string bookingId)
        {
            using var transaction = _unitOfWork.BeginTransaction();
            try
            {
                var booking = await _unitOfWork.BookingRepository.GetFirstOrDefaultAsync(query => query.Where(b => b.BookingId == bookingId));
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
        //public async Task<APIResponseModel> UpdateBookingFromCallbackData(ExtraData extradata,
        //                                                                   float totalAmount,
        //                                                                   int transTime,
        //                                                                   string orderId,
        //                                                                   string tranId,
        //                                                                   string appId,
        //                                                                   string mechant,
        //                                                                   string overallMac,
        //                                                                   string mac,
        //                                                                   int resultCode)
        //{
        //    try
        //    {
        //        var booking = await _unitOfWork.BookingRepository.GetFirstsOrDefaultAsync(b => b.BookingId == extradata.BookingId);
        //        if (booking == null)
        //        {
        //            return new APIResponseModel
        //            {
        //                Message = "Booking not found.",
        //                IsSuccess = false,
        //            };
        //        }

        //        booking.Status = 1;
        //        booking.UpdateDate = DateTime.Now;
        //        await _unitOfWork.BookingRepository.UpdateAsync(booking);


        //        var tickets = await _unitOfWork.TicketRepository.GetAllAsyncs(query => query
        //                                                    .Where(t => t.BookingId == booking.BookingId));
        //        foreach (var ticket in tickets)
        //        {
        //            ticket.Status = 1;
        //            ticket.UpdateDate = DateTime.Now;
        //            await _unitOfWork.TicketRepository.UpdateAsync(ticket);

        //            var dailyTicketType = await _unitOfWork.DailyTicketRepository.GetAllAsyncs(query => query.Where(d => d.DailyTicketId == ticket.DailyTicketId));
        //            var dailyTicketTypeCapa = dailyTicketType.FirstOrDefault();
        //            if (dailyTicketTypeCapa != null)
        //            {
        //                dailyTicketTypeCapa.Capacity += ticket.Quantity;
        //                dailyTicketTypeCapa.UpdateDate = DateTime.Now;
        //                await _unitOfWork.DailyTicketRepository.UpdateAsync(dailyTicketTypeCapa);
        //            }
        //        }

        //        foreach (var ticket in tickets)
        //        {
        //            var servicesUsedByTicket = await _unitOfWork.ServiceUsedByTicketRepository.GetAllAsyncs(query => query
        //                                                                                     .Where(s => s.TicketId == ticket.TicketId));
        //            foreach (var serviceUsed in servicesUsedByTicket)
        //            {
        //                serviceUsed.Status = 1;
        //                serviceUsed.UpdateDate = DateTime.Now;
        //                await _unitOfWork.ServiceUsedByTicketRepository.UpdateAsync(serviceUsed);
        //            }
        //        }
        //        var newPaymentId = Guid.NewGuid();
        //        var newPayment = new Payment
        //        {
        //            PaymentId = newPaymentId.ToString(),
        //            PaymentMethodId = "1",
        //            BookingId = booking.BookingId,
        //            AppId = appId,
        //            OrderId = orderId,
        //            TransId = tranId,
        //            TransTime = transTime,
        //            Amount = totalAmount,
        //            MerchantTransId = mechant,
        //            Description = overallMac,
        //            ResultCode = resultCode,
        //            Message = mac,
        //            ExtraData = JsonConvert.SerializeObject(extradata),
        //            Status = 1,
        //            CreateDate = DateTime.Now,
        //        };
        //        await _unitOfWork.PaymentRepository.AddAsync(newPayment);

        //        var zaloId = await _unitOfWork.AccountRepository.GetFirstOrDefaultAsync(query => query.Where(a=>a.ZaloUser == extradata.ZaloId));
        //        var newTransactionId = Guid.NewGuid();
        //        var transaction = new TransactionsHistory
        //        {
        //            TransactionId = newTransactionId.ToString(),
        //            UserId = zaloId.Id,
        //            BookingId = booking.BookingId,
        //            OrderId = orderId,
        //            CreateDate = DateTime.Now,
        //            Status = 1
        //        };
        //        await _unitOfWork.TransactionsHistoryRepository.AddAsync(transaction);
        //        _unitOfWork.Save();

        //        return new APIResponseModel
        //        {
        //            Message = "Update Status updated successfully for booking and related records.",
        //            IsSuccess = true,
        //        };


        //    }
        //    catch (Exception ex)
        //    {
        //        return new APIResponseModel
        //        {
        //            Message = $"Error update booking: {ex.Message}",
        //            IsSuccess = false,
        //        };
        //    }
        //}

        public async Task<APIResponseModel> CreateBookingFromCallbackData(ExtraData extradata, 
                                                                          float totalAmount,
                                                                          int transTime,
                                                                          string orderId,
                                                                          string tranId,
                                                                          string appId,
                                                                          string mechant,
                                                                          string overallMac,
                                                                          string mac,
                                                                          int resultCode)
        {
            try
            {
                // Lấy thông tin zalo account từ ZaloId
                var zaloAccount = await _unitOfWork.AccountRepository.GetFirstOrDefaultAsync(query => query
                                  .Where(a => a.ZaloUser == extradata.ZaloId));
                if (zaloAccount == null)
                {
                    return new APIResponseModel
                    {
                        Message = "Account not found.",
                        IsSuccess = false,
                    };
                }

                // Lấy thông tin service IDs liên quan đến DailyTourId
                var serviceIds = await GetServiceIdsByDailyTourId(extradata.DailyTourId);
                if (serviceIds == null || !serviceIds.Any())
                {
                    return new APIResponseModel
                    {
                        Message = "No Service IDs found.",
                        IsSuccess = false,
                    };
                }
                var dailyTourExpirationDate = await _unitOfWork.DailyTourRepository.GetFirstOrDefaultAsync(query => query
                                  .Where(a => a.DailyTourId == extradata.DailyTourId));
                // Lấy chi tiết về DailyTour
                var dailyTourDetails = await GetDailyToursDetails(extradata.DailyTourId);
                if (dailyTourDetails == null)
                {
                    return new APIResponseModel
                    {
                        Message = "Tour details not found.",
                        IsSuccess = false,
                    };
                }

                var destinationId = dailyTourDetails?.PackageTour?.TourSegments?
                                        .Select(l => l.DestinationId)
                                        .ToList();
                var destinationName = dailyTourDetails?.PackageTour?.TourSegments?
                                         .Select(l => l.DestinationName)
                                         .ToList();
                var destination = dailyTourDetails.PackageTour?.TourSegments?.ToList();
                var location = destination.SelectMany(l => l.Locations)
                                           .Select(c => c.LocationId).ToList();

                var services = destination.SelectMany(l => l.Locations)
                                           .SelectMany(s => s.Services)
                                           .Select(c => c.ServiceId).ToList();

                // Chuyển các thông tin về Destination, Location, và Services thành JSON
                var destinationIdJson = JsonConvert.SerializeObject(destinationId);
                var destinationNameJson = JsonConvert.SerializeObject(destinationName);
                var locationJson = JsonConvert.SerializeObject(location);
                var servicesJson = JsonConvert.SerializeObject(services);

                // Tạo Booking mới
                var newBookingId = Guid.NewGuid();
                var newBooking = new Booking
                {
                    BookingId = newBookingId.ToString(),
                    UserId = zaloAccount.Id,
                    DailyTourId = extradata.DailyTourId,
                    BookingDate = DateTime.Now,
                    ExpirationDate = dailyTourExpirationDate.ExpirationDate,
                    TotalPrice = totalAmount,
                    Status = 1,
                    CreateDate = DateTime.Now,
                };
                await _unitOfWork.BookingRepository.AddAsync(newBooking);

                List<string> ticketIds = new List<string>();

                foreach (var ticket in extradata.Tickets)
                {
                    for (int i = 0; i < ticket.TotalQuantity; i++)
                    {
                        var newTicketId = Guid.NewGuid().ToString();
                        var priceByTicket = await _unitOfWork.DailyTicketRepository.
                                                              GetFirstOrDefaultAsync(query=>query.Where(c=>c.DailyTicketId== ticket.DailyTicketId));
                        // Tạo dữ liệu QR code
                        var qrData = new
                        {
                            //BookingId = newBooking.BookingId,
                            //ZaloUser = extradata.ZaloId,
                            DailyTourId = newBooking.DailyTourId,
                            TourName = dailyTourDetails.DailyTourName,
                            //StartDate = dailyTourDetails.StartDate,
                            //EndDate = dailyTourDetails.EndDate,
                            //Discount = dailyTourDetails.Discount,
                            //DailyTourPrice = dailyTourDetails.DailyTourPrice,
                            //City = dailyTourDetails.PackageTour?.CityId,
                            DestinationId = destinationIdJson,
                            DestinationName = destinationNameJson,
                            //LocationId = locationJson,
                            //ServiceId = servicesJson,
                            //PhoneNumber = zaloAccount.PhoneNumber,
                            //BookingDate = newBooking.BookingDate,
                            //PriceOfTicket = priceByTicket.DailyTicketPrice,
                            ExpirationDate = newBooking.ExpirationDate,
                            TotalPrice = newBooking.TotalPrice,
                            TicketTypeId = newTicketId,
                            DailyTicketId = ticket.DailyTicketId,
                            TicketName = ticket.TicketName,
                            Price = ticket.TotalPrice
                        };

                        // Mã hóa dữ liệu QR và tạo URL hình ảnh QR
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
                            Status = 1,
                            CreateDate = DateTime.Now,
                        };
                        await _unitOfWork.TicketRepository.AddAsync(newTicket);

                        ticketIds.Add(newTicket.TicketId);

                        // Gán các dịch vụ đã sử dụng cho vé
                        foreach (var serviceId in serviceIds)
                        {
                            var serviceUsedByTicket = new ServiceUsedByTicket
                            {
                                SUBTId = Guid.NewGuid().ToString(),
                                TicketId = newTicket.TicketId,
                                ServiceId = serviceId,
                                CreateDate = DateTime.Now,
                                Status = 1
                            };
                            await _unitOfWork.ServiceUsedByTicketRepository.AddAsync(serviceUsedByTicket);
                        }
                    }
                    var dailyTicketType = await _unitOfWork.DailyTicketRepository.GetAllAsyncs(query => query.Where(d=> d.DailyTicketId==ticket.DailyTicketId));
                    var dailyTicketTypeCapa = dailyTicketType.FirstOrDefault();
                    if (dailyTicketTypeCapa != null)
                    {
                        dailyTicketTypeCapa.Capacity -= ticket.TotalQuantity;
                        dailyTicketTypeCapa.UpdateDate = DateTime.Now;
                        await _unitOfWork.DailyTicketRepository.UpdateAsync(dailyTicketTypeCapa);
                    }
                }
                var newPaymentId = Guid.NewGuid();
                var newPayment = new Payment
                {
                    PaymentId =newPaymentId.ToString(),
                    PaymentMethodId ="1",
                    BookingId = newBooking.BookingId,
                    AppId = appId, 
                    OrderId = orderId, 
                    TransId = tranId, 
                    TransTime = transTime, 
                    Amount = totalAmount,
                    MerchantTransId = mechant, 
                    Description = overallMac,
                    ResultCode = resultCode, 
                    Message = mac,
                    ExtraData = JsonConvert.SerializeObject(extradata), 
                    Status = 1,
                    CreateDate = DateTime.Now,
                };
                await _unitOfWork.PaymentRepository.AddAsync(newPayment);

                var newTransactionId = Guid.NewGuid();
                var transaction = new TransactionsHistory
                {
                    TransactionId = newTransactionId.ToString(),
                    UserId = zaloAccount.Id,
                    BookingId = newBooking.BookingId,
                    OrderId= orderId,
                    CreateDate = DateTime.Now,
                    Status=1
                };
                await _unitOfWork.TransactionsHistoryRepository.AddAsync(transaction);

                var notification = new BusinessObjects.Models.Notification
                {
                    NotifyId = Guid.NewGuid().ToString(),
                    UserId = zaloAccount.Id,
                    SendDate = DateTime.Now,
                    Message = $"Bạn đã đặt tour du lịch thành công",
                    Title = "Đặt Thành công",
                    Type = "Success",
                    Status = 1,
                };
                await _unitOfWork.NotificationRepository.AddAsync(notification);
                _unitOfWork.Save();

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
                    Message = $"Error creating booking: {ex.Message}",
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

        public async Task<APIResponseModel> ProcessRefund(RefundModel refundModel)
        {
            try
            {
                string description = "AvatarTour xin phép hoàn tiền cho quý khách, cảm ơn đã sử dụng dịch vụ của chúng tôi";
                var transId = await _unitOfWork.PaymentRepository.GetAllAsyncs(query => query.Where(p => p.MerchantTransId == refundModel.ZptransId));
                var bookingId = transId.FirstOrDefault();
                var checkStatusBooking = await _unitOfWork.BookingRepository.GetByIdStringAsync(bookingId.BookingId);
                if(checkStatusBooking.Status ==9)
                {
                    return new APIResponseModel
                    {
                        IsSuccess = false,
                        Message = "Booking is in progress and cannot be refunded."
                    };
                }
                //else if(checkStatusBooking.Status ==2)
                //{
                //    return new APIResponseModel
                //    {
                //        IsSuccess = false,
                //        Message = "Booking has been cancelled."
                //    };
                //}
                else if (checkStatusBooking.Status == -1)
                {
                    return new APIResponseModel
                    {
                        IsSuccess = false,
                        Message = "Booking has been deleted, cannot be refunded"
                    };
                }else if (checkStatusBooking.Status == 4)
                {
                    return new APIResponseModel
                    {
                        IsSuccess = false,
                        Message = "Booking has been used."
                    };
                }else if (checkStatusBooking.Status == 5)
                {
                    return new APIResponseModel
                    {
                        IsSuccess = false,
                        Message = "Booking has been refunded in advance."
                    };
                } else if (checkStatusBooking.Status == 0)
                {
                    return new APIResponseModel
                    {
                        IsSuccess = false,
                        Message = "Booking has been disabled."
                    };
                } else if (checkStatusBooking.Status == 3)
                {
                    return new APIResponseModel
                    {
                        IsSuccess = false,
                        Message = "Booking has been pending."
                    };
                }
                var currentDate = DateTime.Now.Date;
                var differenceInDays = (checkStatusBooking.ExpirationDate.Value.Date - currentDate).TotalDays;
                if (differenceInDays < 2 || currentDate >= checkStatusBooking.ExpirationDate.Value.Date)
                {
                    return new APIResponseModel
                    {
                        IsSuccess = false,
                        Message = "Refund is only allowed within 1 days before expriateDate."
                    };
                }


                var updateBookingCancel = await UpdateStatusBookingAsync(bookingId.BookingId, 2);

                var timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString();
                var rand = new Random();
                var uid = timestamp + rand.Next(111, 999).ToString();
                double ammountRe = refundModel.Amount * 0.8;
                long ammountReLong = (long)ammountRe;
                var param = new Dictionary<string, string>
        {
            { "appid", appid },
            { "mrefundid", DateTime.Now.ToString("yyMMdd") + "_" + appid + "_" + uid },
            { "zptransid", refundModel.ZptransId }, 
            { "amount", ammountReLong.ToString() }, 
            { "timestamp", timestamp },
            { "description", description } 
        };

                var hmacInput = $"{appid}|{param["zptransid"]}|{param["amount"]}|{param["description"]}|{param["timestamp"]}";
                param.Add("mac", ComputeHMACSHA256(key1, hmacInput));

                var result = await PostFormAsync(refundUrl, param);

                if (result.ContainsKey("returncode") && result["returncode"] == "2")
                {
                  //  var transId = await _unitOfWork.PaymentRepository.GetAllAsyncs(query => query.Where(p=> p.MerchantTransId == zptransid));
                   // var bookingId = transId.FirstOrDefault();
                    var updateBooking = await UpdateStatusBookingAddCapacityAsync(bookingId.BookingId,5, bookingId.PaymentId);
                    return updateBooking;
                    
                }
                else
                {
                    return new APIResponseModel
                    {
                        IsSuccess = false,
                        Message = result.ContainsKey("returnmessage") ? result["returnmessage"] : "Refund failed.",
                        Data = result
                    };
                }
            }
            catch (Exception ex)
            {
                return new APIResponseModel
                {
                    IsSuccess = false,
                    Message = "Exception: " + ex.Message,
                    Data = null
                };
            }
        }

        private string ComputeHMACSHA256(string key, string data)
        {
            using (var hmacsha256 = new HMACSHA256(Encoding.UTF8.GetBytes(key)))
            {
                var hashBytes = hmacsha256.ComputeHash(Encoding.UTF8.GetBytes(data));
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }

        private async Task<Dictionary<string, string>> PostFormAsync(string url, Dictionary<string, string> formData)
        {
            using (var client = new HttpClient())
            {
                var content = new FormUrlEncodedContent(formData);
                var response = await client.PostAsync(url, content);
                var responseString = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<Dictionary<string, string>>(responseString);
            }
        }


        public async Task<APIResponseModel> GetAllPayment()
        {
            try
            {
                var payment = await _unitOfWork.PaymentRepository.GetAllAsync();
                if(payment == null)
                {
                    return new APIResponseModel
                    {
                        IsSuccess = false,
                        Message = "No payment",
                        Data = null
                    };
                }
                return new APIResponseModel
                {
                    IsSuccess = true,
                    Message = "Have payment",
                    Data = payment
                };
            }
            catch (Exception ex)
            {
                return new APIResponseModel
                {
                    IsSuccess = false,
                    Message = "Exception: " + ex.Message,
                    Data = null
                };
            }
        }

        public async Task<APIResponseModel> UpdateStatusBookingAsync(string bookingId, int status)
        {
            try
            {
                var booking = await _unitOfWork.BookingRepository.GetFirstsOrDefaultAsync(b => b.BookingId == bookingId);
                if (booking == null)
                {
                    return new APIResponseModel
                    {
                        Message = "Booking not found.",
                        IsSuccess = false,
                    };
                }

                booking.Status = status;
                booking.UpdateDate = DateTime.Now;
                await _unitOfWork.BookingRepository.UpdateAsync(booking);


                var tickets = await _unitOfWork.TicketRepository.GetAllAsyncs(query => query
                                                            .Where(t => t.BookingId == bookingId));
                foreach (var ticket in tickets)
                {
                    ticket.Status = status;
                    ticket.UpdateDate = DateTime.Now;
                    await _unitOfWork.TicketRepository.UpdateAsync(ticket);

                    var dailyTicketType = await _unitOfWork.DailyTicketRepository.GetAllAsyncs(query => query.Where(d => d.DailyTicketId == ticket.DailyTicketId));
                    var dailyTicketTypeCapa = dailyTicketType.FirstOrDefault();
                    if (dailyTicketTypeCapa != null)
                    {
                        dailyTicketTypeCapa.Capacity += ticket.Quantity;
                        dailyTicketTypeCapa.UpdateDate = DateTime.Now;
                        await _unitOfWork.DailyTicketRepository.UpdateAsync(dailyTicketTypeCapa);
                    }
                }

                foreach (var ticket in tickets)
                {
                    var servicesUsedByTicket = await _unitOfWork.ServiceUsedByTicketRepository.GetAllAsyncs(query => query
                                                                                             .Where(s => s.TicketId == ticket.TicketId));
                    foreach (var serviceUsed in servicesUsedByTicket)
                    {
                        serviceUsed.Status = status;
                        serviceUsed.UpdateDate = DateTime.Now;
                        await _unitOfWork.ServiceUsedByTicketRepository.UpdateAsync(serviceUsed);
                    }
                }

                _unitOfWork.Save();

                return new APIResponseModel
                {
                    Message = "Refund successfully and Status updated successfully for booking and related records.",
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

        public async Task<APIResponseModel> UpdateStatusBookingAddCapacityAsync(string bookingId, int status, string paymentId)
        {
            using var transaction = _unitOfWork.BeginTransaction();
            try
            {
                var booking = await _unitOfWork.BookingRepository.GetFirstsOrDefaultAsync(b => b.BookingId == bookingId);
                if (booking == null)
                {
                    return new APIResponseModel
                    {
                        Message = "Booking not found.",
                        IsSuccess = false,
                    };
                }
                var transactionHis = await _unitOfWork.TransactionsHistoryRepository.GetFirstOrDefaultAsync(query=>query.Where(c=>c.BookingId == booking.BookingId));
                if(transactionHis == null)
                {
                    return new APIResponseModel
                    {
                        Message = "Transaction not found.",
                        IsSuccess = false,
                    };
                }

                transactionHis.Status = status;
                await _unitOfWork.TransactionsHistoryRepository.UpdateAsync(transactionHis);

                booking.Status = status;
                booking.UpdateDate = DateTime.Now;
                await _unitOfWork.BookingRepository.UpdateAsync(booking);


                var tickets = await _unitOfWork.TicketRepository.GetAllAsyncs(query => query
                                                            .Where(t => t.BookingId == bookingId));
                foreach (var ticket in tickets)
                {
                    ticket.Status = status;
                    ticket.UpdateDate = DateTime.Now;
                    await _unitOfWork.TicketRepository.UpdateAsync(ticket);
                }
  
                foreach (var ticket in tickets)
                {
                    var servicesUsedByTicket = await _unitOfWork.ServiceUsedByTicketRepository.GetAllAsyncs(query => query
                                                                                             .Where(s => s.TicketId == ticket.TicketId));
                    foreach (var serviceUsed in servicesUsedByTicket)
                    {
                        serviceUsed.Status = status;
                        serviceUsed.UpdateDate = DateTime.Now;
                        await _unitOfWork.ServiceUsedByTicketRepository.UpdateAsync(serviceUsed);
                    }
                }
                var paymentUpdate = await _unitOfWork.PaymentRepository.GetFirstOrDefaultAsync(query => query.Where(p => p.PaymentId == paymentId));
                if (paymentUpdate == null)
                {
                    return new APIResponseModel
                    {
                        Message = "Payment not found.",
                        IsSuccess = false,
                    };
                }
                paymentUpdate.Status = status;
                paymentUpdate.UpdateDate = DateTime.Now;
                await _unitOfWork.PaymentRepository.UpdateAsync(paymentUpdate);
                _unitOfWork.Save();
                transaction.Commit();

                return new APIResponseModel
                {
                    Message = "Refund successfully and Status updated successfully for booking and related records.",
                    IsSuccess = true,
                };
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return new APIResponseModel
                {
                    Message = ex.Message,
                    IsSuccess = false,
                };
            }
        }

    //    public async Task<APIResponseModel> CreateBookingFromCallbackDatas(
    //ExtraData extradata,
    //float totalAmount,
    //int transTime,
    //string orderId,
    //string tranId,
    //string appId,
    //string mechant,
    //string overallMac,
    //string mac,
    //int resultCode)
    //    {
    //        try
    //        {
    //            using (var transaction = _unitOfWork.BeginTransaction())
    //            {
    //                // Lấy thông tin zalo account từ ZaloId
    //                var zaloAccount = await _unitOfWork.AccountRepository.GetFirstOrDefaultAsync(query => query
    //                    .Where(a => a.ZaloUser == extradata.ZaloId));
    //                if (zaloAccount == null)
    //                {
    //                    return new APIResponseModel
    //                    {
    //                        Message = "Account not found.",
    //                        IsSuccess = false,
    //                    };
    //                }

    //                // Lấy thông tin service IDs liên quan đến DailyTourId
    //                var serviceIds = await GetServiceIdsByDailyTourId(extradata.DailyTourId);
    //                if (serviceIds == null || !serviceIds.Any())
    //                {
    //                    return new APIResponseModel
    //                    {
    //                        Message = "No Service IDs found.",
    //                        IsSuccess = false,
    //                    };
    //                }

    //                // Lấy chi tiết DailyTour
    //                var dailyTour = await _unitOfWork.DailyTourRepository.GetFirstOrDefaultAsync(query => query
    //                    .Include(d => d.PackageTours)
    //                        .ThenInclude(p => p.TourSegments)
    //                        .ThenInclude(ts => ts.Destinations)
    //                        .ThenInclude(l => l.Locations)
    //                        .ThenInclude(l => l.Services)
    //                    .Where(d => d.DailyTourId == extradata.DailyTourId));

    //                if (dailyTour == null)
    //                {
    //                    return new APIResponseModel
    //                    {
    //                        Message = "Tour details not found.",
    //                        IsSuccess = false,
    //                    };
    //                }

    //                // Tạo Booking mới
    //                var newBooking = new Booking
    //                {
    //                    BookingId = extradata.BookingId,
    //                    UserId = zaloAccount.Id,
    //                    DailyTourId = extradata.DailyTourId,
    //                    BookingDate = DateTime.Now,
    //                    ExpirationDate = dailyTour.ExpirationDate,
    //                    TotalPrice = totalAmount,
    //                    Status = 1,
    //                    CreateDate = DateTime.Now,
    //                };
    //                await _unitOfWork.BookingRepository.AddAsync(newBooking);

    //                // Xử lý vé và dịch vụ song song
    //                var tickets = new List<Ticket>();
    //                var servicesByTickets = new ConcurrentBag<ServiceUsedByTicket>();
    //                var dailyTicketsToUpdate = new ConcurrentDictionary<string, DailyTicketType>();

    //                var destinationId = dailyTour?.PackageTours?.TourSegments?
    //                                               .Select(l => l.DestinationId)
    //                                               .ToList();
    //                var destinationName = dailyTour?.PackageTours?.TourSegments?
    //                                        .Select(l => l.Destinations.DestinationName)
    //                                        .ToList();
    //                var destinationIdJson = JsonConvert.SerializeObject(destinationId);
    //                var destinationNameJson = JsonConvert.SerializeObject(destinationName);

    //                // Đồng bộ quá trình tạo vé
    //                var lockObj = new object(); // Lock object để bảo vệ List<Ticket>

    //                await Task.WhenAll(extradata.Tickets.Select(async ticket =>
    //                {
    //                    var dailyTicket = await _unitOfWork.DailyTicketRepository.GetFirstOrDefaultAsync(query =>
    //                        query.Where(c => c.DailyTicketId == ticket.DailyTicketId));

    //                    if (dailyTicket != null)
    //                    {
    //                        dailyTicketsToUpdate.TryAdd(dailyTicket.DailyTicketId, dailyTicket);

    //                        for (int i = 0; i < ticket.TotalQuantity; i++)
    //                        {
    //                            var newTicketId = Guid.NewGuid().ToString();
    //                            var qrData = new
    //                            {
    //                                DailyTourId = newBooking.DailyTourId,
    //                                TourName = dailyTour.DailyTourName,
    //                                DestinationId = destinationIdJson,
    //                                DestinationName = destinationNameJson,
    //                                ExpirationDate = newBooking.ExpirationDate,
    //                                TotalPrice = newBooking.TotalPrice,
    //                                TicketTypeId = newTicketId,
    //                                DailyTicketId = ticket.DailyTicketId,
    //                                TicketName = ticket.TicketName,
    //                                Price = ticket.TotalPrice
    //                            };
    //                            var qrContent = _encryptionHelperService.EncryptString(JsonConvert.SerializeObject(qrData));
    //                            var qrImageUrl = await GenerateQRCode(qrContent);

    //                            var newTicket = new Ticket
    //                            {
    //                                TicketId = newTicketId,
    //                                BookingId = newBooking.BookingId,
    //                                DailyTicketId = ticket.DailyTicketId,
    //                                TicketName = ticket.TicketName,
    //                                Price = ticket.TotalPrice,
    //                                QRImgUrl = qrImageUrl,
    //                                PhoneNumberReference = zaloAccount.PhoneNumber,
    //                                Quantity = 1,
    //                                Status = 1,
    //                                CreateDate = DateTime.Now,
    //                            };

    //                            // Dùng lock để bảo vệ danh sách tickets
    //                            lock (lockObj)
    //                            {
    //                                tickets.Add(newTicket);
    //                            }

    //                            foreach (var serviceId in serviceIds)
    //                            {
    //                                var serviceUsedByTicket = new ServiceUsedByTicket
    //                                {
    //                                    SUBTId = Guid.NewGuid().ToString(),
    //                                    TicketId = newTicket.TicketId,
    //                                    ServiceId = serviceId,
    //                                    CreateDate = DateTime.Now,
    //                                    Status = 1
    //                                };
    //                                servicesByTickets.Add(serviceUsedByTicket);
    //                            }
    //                        }
    //                    }
    //                }));

    //                // Batch insert tickets và services
    //                await _unitOfWork.TicketRepository.AddRangeAsync(tickets);
    //                await _unitOfWork.ServiceUsedByTicketRepository.AddRangeAsync(servicesByTickets.ToList());

    //                // Batch update daily ticket capacities
    //                foreach (var entry in dailyTicketsToUpdate)
    //                {
    //                    entry.Value.Capacity -= extradata.Tickets
    //                        .Where(t => t.DailyTicketId == entry.Key)
    //                        .Sum(t => t.TotalQuantity);
    //                    entry.Value.UpdateDate = DateTime.Now;
    //                }
    //                await _unitOfWork.DailyTicketRepository.UpdateRangeAsync(dailyTicketsToUpdate.Values.ToList());

    //                // Tạo Payment
    //                var newPayment = new Payment
    //                {
    //                    PaymentId = Guid.NewGuid().ToString(),
    //                    PaymentMethodId = "1",
    //                    BookingId = newBooking.BookingId,
    //                    AppId = appId,
    //                    OrderId = orderId,
    //                    TransId = tranId,
    //                    TransTime = transTime,
    //                    Amount = totalAmount,
    //                    MerchantTransId = mechant,
    //                    Description = overallMac,
    //                    ResultCode = resultCode,
    //                    Message = mac,
    //                    ExtraData = JsonConvert.SerializeObject(extradata),
    //                    Status = 1,
    //                    CreateDate = DateTime.Now,
    //                };
    //                await _unitOfWork.PaymentRepository.AddAsync(newPayment);

    //                // Tạo lịch sử giao dịch và thông báo
    //                var transactionHistory = new TransactionsHistory
    //                {
    //                    TransactionId = Guid.NewGuid().ToString(),
    //                    UserId = zaloAccount.Id,
    //                    BookingId = newBooking.BookingId,
    //                    OrderId = orderId,
    //                    CreateDate = DateTime.Now,
    //                    Status = 1
    //                };
    //                await _unitOfWork.TransactionsHistoryRepository.AddAsync(transactionHistory);

    //                var notification = new Notification
    //                {
    //                    NotifyId = Guid.NewGuid().ToString(),
    //                    UserId = zaloAccount.Id,
    //                    SendDate = DateTime.Now,
    //                    Message = "Bạn đã đặt tour du lịch thành công",
    //                    Title = "Success",
    //                    Type = "Thành công",
    //                    Status = 1,
    //                };
    //                await _unitOfWork.NotificationRepository.AddAsync(notification);

    //                _unitOfWork.Save();
    //                transaction.Commit();

    //                return new APIResponseModel
    //                {
    //                    Message = "Booking and tickets created successfully.",
    //                    IsSuccess = true,
    //                };
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            return new APIResponseModel
    //            {
    //                Message = $"Error creating booking: {ex.Message}",
    //                IsSuccess = false,
    //            };
    //        }
    //    }


    }
}
