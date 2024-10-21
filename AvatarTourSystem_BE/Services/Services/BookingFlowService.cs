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
                // Lấy DailyTour dựa trên dailyTourId
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
                    return new List<string>(); // Hoặc throw một exception nếu bạn muốn
                }

                // Lấy danh sách ServiceId
                var serviceIds = dailyTour.PackageTours?.TourSegments
                    .SelectMany(ts => ts.ServiceByTourSegments)
                    .Where(sbts => sbts.Services?.Status == 1)
                    .Select(se => se.Services?.ServiceId)
                    .Distinct() // Lấy các ServiceId duy nhất
                    .ToList();

                return serviceIds;
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ nếu cần
                return new List<string>(); // Hoặc throw một exception nếu bạn muốn
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
                Console.WriteLine(serviceIds + "ss");

                var newBookingId = Guid.NewGuid();
                var newBooking = new Booking
                {
                    BookingId = newBookingId.ToString(),
                    UserId = zaloAccount.Id,
                    DailyTourId = createModel.DailyTourId,
                    PaymentId = "1",
                    BookingDate = DateTime.Now,
                    ExpirationDate = DateTime.UtcNow.AddDays(2), 
                    TotalPrice = createModel.TotalPrice,
                    Status = 1,  
                    CreateDate = DateTime.UtcNow,
                };
                await _unitOfWork.BookingRepository.AddAsync(newBooking);

                foreach (var ticket in createModel.Tickets)
                {
                    var qrContent = $"đây là id booking nè {newBooking.BookingId}: còn đây là ticketid{ticket.TicketTypeId}";
                    var qrImageUrl = await GenerateQRCode(qrContent);
                    var newTicket = new Ticket
                    {
                        TicketId = Guid.NewGuid().ToString(),
                        BookingId = newBooking.BookingId,
                        TicketTypeId = ticket.TicketTypeId,
                        TicketName = ticket.TicketName,
                        Price = ticket.TotalPrice,
                        QR = qrImageUrl,
                        Quantity = ticket.TotalQuantity,
                        Status = 1,
                        CreateDate = DateTime.UtcNow,
                    };
                    await _unitOfWork.TicketRepository.AddAsync(newTicket);
                    foreach (var serviceId in serviceIds)
                    {
                        var serviceUsedByTicket = new ServiceUsedByTicket
                        {
                            SUBTId = Guid.NewGuid().ToString(),
                            TicketId = newTicket.TicketId,
                            ServiceId = serviceId,
                            Status = 1
                        };
                        await _unitOfWork.ServiceUsedByTicketRepository.AddAsync(serviceUsedByTicket);
                    }
                }
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
                    Height = 300,
                    Width = 300
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

        public Task<APIResponseModel> GetBookingFlowByIdAsync(string id)
        {
            throw new NotImplementedException();
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
