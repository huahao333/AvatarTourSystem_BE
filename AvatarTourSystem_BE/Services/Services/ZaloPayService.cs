using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Services.Common;
using Services.Interfaces;
using Services.Common.ZaloPayHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using BusinessObjects.Models;
using BusinessObjects.ViewModels.Booking;
using Newtonsoft.Json;
using AutoMapper;
using Repositories.Interfaces;
using Repositories;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using ZXing.Common;

namespace Services.Services
{
    public class ZaloPayService : IZaloPayService
    {
        private readonly string _privateKey = "b0783c8f8d8d55f97e7e1b69552da92e";
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly CloudinaryService _cloudinaryService;
        private readonly EncryptionHelperService _encryptionHelperService;

        public ZaloPayService(IConfiguration configuration, IUnitOfWork unitOfWork, IMapper mapper, CloudinaryService cloudinaryService, EncryptionHelperService encryptionHelperService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            //_privateKey = configuration["ZaloPay:PrivateKey"];
            _encryptionHelperService = encryptionHelperService;
            _cloudinaryService = cloudinaryService;
        }

        public bool ValidateCallback(ZaloMiniAppCallback data)
        {
            var mac = GenerateMac(data);
            var overallMac = GenerateOverallMac(data);

            return true; 
        }

        public string GenerateMac(ZaloMiniAppCallback data)
        {
            var dataStr = $"appId={data.appId}&amount={data.amount}&description={data.description}" +
                         $"&orderId={data.orderid}&message={data.message}&resultCode={data.resultCode}" +
                         $"&transId={data.transId}";

            return ComputeHmacSha256(dataStr, "b0783c8f8d8d55f97e7e1b69552da92e");
        }

        public string GenerateOverallMac(ZaloMiniAppCallback data)
        {
            var properties = data.GetType().GetProperties()
                .OrderBy(p => p.Name)
                .Select(p => $"{p.Name.ToLowerFirst()}={p.GetValue(data)}")
                .ToList();

            var dataStr = string.Join("&", properties);
            return ComputeHmacSha256(dataStr, "b0783c8f8d8d55f97e7e1b69552da92e");
        }

        private string ComputeHmacSha256(string message, string secret)
        {
            var encoding = Encoding.UTF8;
            var keyBytes = encoding.GetBytes(secret);
            var messageBytes = encoding.GetBytes(message);

            using (var hmac = new HMACSHA256(keyBytes))
            {
                var hashBytes = hmac.ComputeHash(messageBytes);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
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
                                        .SelectMany(l => l.DestinationId)
                                        .ToList();
                var destination = dailyTourDetails.PackageTour?.TourSegments?.ToList();
                var location = destination.SelectMany(l => l.Locations)
                                           .Select(c => c.LocationId).ToList();

                var services = destination.SelectMany(l => l.Locations)
                                           .SelectMany(s => s.Services)
                                           .Select(c => c.ServiceId).ToList();

                //var destinationIdString = string.Join("; ", destinationId);
                //var locationId = string.Join("; ", location);
                //var servicesId = string.Join("; ", services);
                var destinationIdJson = JsonConvert.SerializeObject(destinationId);
                var locationJson = JsonConvert.SerializeObject(location);
                var servicesJson = JsonConvert.SerializeObject(services);

                var newBookingId = Guid.NewGuid();
                var newBooking = new Booking
                {
                    BookingId = newBookingId.ToString(),
                    UserId = zaloAccount.Id,
                    DailyTourId = createModel.DailyTourId,
                    BookingDate = DateTime.Now,
                    ExpirationDate = DateTime.Now.AddDays(2),
                    TotalPrice = createModel.TotalPrice,
                    Status = 9,
                    CreateDate = DateTime.Now,
                };
                await _unitOfWork.BookingRepository.AddAsync(newBooking);

                List<string> ticketIds = new List<string>();

                foreach (var ticket in createModel.Tickets)
                {
                    for (int i = 0; i < ticket.TotalQuantity; i++)
                    {
                        var newTicketId = Guid.NewGuid().ToString();
                        //var qrContent = $"BookingId: {newBooking.BookingId}\n" +
                        //                $"ZaloUser: {createModel.ZaloId}\n" +
                        //                $"DailyTourId: {newBooking.DailyTourId}\n" +
                        //                $"TourName: {dailyTourDetails.DailyTourName}\n" +
                        //                $"ExpirationDate: {dailyTourDetails.ExpirationDate}\n" +
                        //                $"StartDate: {dailyTourDetails.StartDate}\n" +
                        //                $"EndDate: {dailyTourDetails.EndDate}\n" +
                        //                $"Discount: {dailyTourDetails.Discount}\n" +
                        //                $"DailyTourPrice: {dailyTourDetails.DailyTourPrice}\n" +
                        //                $"City: {dailyTourDetails.PackageTour?.CityId}\n" +
                        //                $"DestinationId: {destinationIdString}\n" +
                        //                $"LocationId: {locationId}\n" +
                        //                $"ServiceId: {servicesId}\n" +

                        //                $"PhoneNumber: {zaloAccount.PhoneNumber}\n" +
                        //                $"BookingDate: {newBooking.BookingDate}\n" +
                        //                $"ExpirationDate: {newBooking.ExpirationDate}\n" +
                        //                $"TotalPrice: {newBooking.TotalPrice}\n" +
                        //                $"TicketTypeId: {newTicketId} \n" +
                        //                $"DailyTicketId:{ticket.DailyTicketId} \n" +
                        //                $"TicketName:{ticket.TicketName} \n" +
                        //                $"Price:{ticket.TotalPrice} \n";

                        var qrData = new
                        {
                            BookingId = newBooking.BookingId,
                            ZaloUser = createModel.ZaloId,
                            DailyTourId = newBooking.DailyTourId,
                            TourName = dailyTourDetails.DailyTourName,
                            //   ExpirationDate = dailyTourDetails.ExpirationDate,
                            StartDate = dailyTourDetails.StartDate,
                            EndDate = dailyTourDetails.EndDate,
                            Discount = dailyTourDetails.Discount,
                            DailyTourPrice = dailyTourDetails.DailyTourPrice,
                            City = dailyTourDetails.PackageTour?.CityId,
                            DestinationId = destinationIdJson,
                            LocationId = locationJson,
                            ServiceId = servicesJson,
                            PhoneNumber = zaloAccount.PhoneNumber,
                            BookingDate = newBooking.BookingDate,
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
                            Price = ticket.TotalPrice,
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
    }

    public static class StringExtensions
    {
        public static string ToLowerFirst(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return str;

            return char.ToLower(str[0]) + str[1..];
        }
    }
}
