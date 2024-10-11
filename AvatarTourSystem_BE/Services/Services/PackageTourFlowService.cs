using AutoMapper;
using BusinessObjects.Enums;
using BusinessObjects.Models;
using BusinessObjects.ViewModels.PackageTour;
using BusinessObjects.ViewModels.PackageTourFlow;
using BusinessObjects.ViewModels.PackageTourFlow.PackageTourUpdate;
using BusinessObjects.ViewModels.Rate;
using MailKit.Net.Smtp;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;
using Services.Common;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class PackageTourFlowService : IPackageTourFlowService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public PackageTourFlowService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<APIResponseModel> CreatePackageTourFlowAsync(FPackageTourCreateModel createModel)
        {
            // Initialize the package tour
            var packageTour = new PackageTour
            {
                PackageTourId = Guid.NewGuid().ToString(),
                PackageTourName = createModel.PackageTourName,
                PackageTourPrice = 0,
                CreateDate = DateTime.Now,
                Status = 1

            };
            foreach (var ticketTypeModel in createModel.TicketTypesCreate)
            {
                var ticketType = new TicketType
                {
                    TicketTypeId = Guid.NewGuid().ToString(),
                    TicketTypeName = ticketTypeModel.TicketTypeName,
                    PackageTourId = packageTour.PackageTourId,
                    CreateDate = DateTime.Now,
                    Status = 1,
                };
                packageTour.TicketTypes.Add(ticketType);
            }
            #region
            //// Initialize total price
            //float? totalPrice = 0;

            //// Iterate through each destination
            //foreach (var destinationModel in createModel.Destinations)
            //{
            //    var destination = new Destination
            //    {
            //        DestinationId = Guid.NewGuid().ToString(), // Generate new ID for Destination
            //        DestinationName = destinationModel.DestinationName,
            //        CityId = destinationModel.CityId,
            //        CreateDate = DateTime.Now,
            //        UpdateDate = DateTime.Now,
            //        Locations = new List<Location>() // Initialize Locations list
            //    };

            //    // Iterate through each location in the destination
            //    foreach (var locationModel in destinationModel.Locations)
            //    {
            //        var location = new Location
            //        {
            //            LocationId = Guid.NewGuid().ToString(), // Generate new ID for Location
            //            LocationName = locationModel.LocationName,
            //            LocationType = locationModel.LocationType,
            //            DestinationId = destination.DestinationId, // Link to the destination
            //            CreateDate = DateTime.Now,
            //            UpdateDate = DateTime.Now,
            //            Services = new List<Service>() // Initialize Services list
            //        };

            //        // Iterate through each service in the location
            //        foreach (var serviceModel in locationModel.Services)
            //        {
            //            var service = new Service
            //            {
            //                ServiceId = Guid.NewGuid().ToString(), // Generate new ID for Service
            //                ServiceName = serviceModel.ServiceName,
            //                ServicePrice = serviceModel.ServicePrice,
            //                CreateDate = DateTime.Now,
            //                UpdateDate = DateTime.Now,
            //            };

            //            // Add service price to total price
            //            totalPrice += service.ServicePrice;

            //            // Add service to the location
            //            location.Services.Add(service);
            //        }

            //        // Add location to the destination
            //        destination.Locations.Add(location);
            //    }

            //    // Add TourSegment for the destination
            //    var tourSegment = new TourSegment
            //    {
            //        TourSegmentId = Guid.NewGuid().ToString(), // Generate new ID for TourSegment
            //        DestinationId = destination.DestinationId, // Set the destination ID
            //        PackageTourId = packageTour.PackageTourId, // Set the package tour ID
            //        CreateDate = DateTime.Now,
            //        UpdateDate = DateTime.Now,
            //        Status = 1 // Default status
            //    };

            //    packageTour.TourSegments.Add(tourSegment);

            //    await _unitOfWork.DestinationRepository.AddAsync(destination);
            //}

            //packageTour.PackageTourPrice = totalPrice;
            #endregion
            await _unitOfWork.PackageTourRepository.AddAsync(packageTour);

            _unitOfWork.Save();
            var packageTourResponse = new FPackageTourModel
            {
                PackageTourId = packageTour.PackageTourId,
                PackageTourName = packageTour.PackageTourName,
                createDate = packageTour.CreateDate,
                updateDate = packageTour.UpdateDate,
                Status = "1",
                TicketTypes = packageTour.TicketTypes.Select(ticket => new FTicketType
                {
                    TicketTypeId = Guid.NewGuid().ToString(),
                    TicketTypeName = ticket.TicketTypeName,
                    Status = ticket.Status
                }).ToList()
            };
            // Return the API response
            return new APIResponseModel
            {
                Message = "PackageTour created successfully",
                IsSuccess = true,
                Data = packageTourResponse
            };
        }

        public Task<APIResponseModel> GetPackageTourFlowAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<APIResponseModel> GetPackageTourFlowByIdAsync(string id)
        {
            #region
            //// Lấy gói tour theo ID
            //var packageTour = await _unitOfWork.PackageTourRepository.GetByIdStringAsync(id);
            //var toursegment = await _unitOfWork.TourSegmentRepository.GetByConditionAsync(i => i.PackageTourId == packageTour.PackageTourId && i.Status != -1);
            //var ticketType = await _unitOfWork.TicketTypeRepository.GetByConditionAsync(i => i.PackageTourId == packageTour.PackageTourId && i.Status != -1);
            //if (packageTour == null)
            //{
            //    return new APIResponseModel
            //    {
            //        Message = "PackageTour not found",
            //        IsSuccess = false
            //    };

            //}

            //// Ánh xạ dữ liệu
            //var response = new FPackageTourResponseModel
            //{
            //    PackageTourId = packageTour.PackageTourId,
            //    PackageTourName = packageTour.PackageTourName,
            //    PackageTourPrice = packageTour.PackageTourPrice ?? 0,
            //    PackageTourImgUrl = packageTour.PackageTourImgUrl,
            //    Destinations = new List<FDestinationResponseModel>(),
            //    TicketTypes = new List<FTicketType>()
            //};
            //foreach (var ticket in ticketType)
            //{
            //    var ticketTypeResponse = new FTicketType
            //    {
            //        TicketTypeId = ticket.TicketTypeId,
            //        TicketTypeName = ticket.TicketTypeName,
            //        Status = (EStatus)ticket.Status
            //    };
            //    response.TicketTypes.Add(ticketTypeResponse);
            //}
            //foreach (var tourSegment in toursegment)
            //{
            //   var serviceByTourSegments = await _unitOfWork.ServiceByTourSegmentRepository.GetByConditionAsync(i => i.TourSegmentId == tourSegment.TourSegmentId && i.Status != -1);
            //    foreach (var service in tourSegment.ServiceByTourSegments)
            //    {
            //        var serviceByTourSegment = new FServiceByTourSegment
            //        {
            //            SBTSId = service.SBTSId,
            //            TourSegmentId = service.TourSegmentId,
            //            ServiceId = service.ServiceId,
            //            Status = (EStatus)service.Status
            //        };


            //    }
            //    var destination = await _unitOfWork.DestinationRepository.GetByIdStringAsync(tourSegment.DestinationId);

            //    if (destination != null)
            //    {
            //        var destinationResponse = new FDestinationResponseModel
            //        {
            //            DestinationId = destination.DestinationId,
            //            DestinationName = destination.DestinationName,
            //            Locations = new List<FLocationResponseModel>()
            //        };
            //        var locations = await _unitOfWork.LocationRepository.GetByConditionAsync(i => i.DestinationId == destination.DestinationId);

            //        foreach (var location in locations)
            //        {
            //            var locationResponse = new FLocationResponseModel
            //            {
            //                LocationId = location.LocationId,
            //                LocationName = location.LocationName,
            //                Services = new List<FServiceResponseModel>()
            //            };
            //            var services = await _unitOfWork.ServiceRepository.GetByConditionAsync(i => i.LocationId == location.LocationId);
            //            foreach (var service in services)
            //            {

            //                var serviceResponse = new FServiceResponseModel
            //                {
            //                    ServiceId = service.ServiceId,
            //                    ServiceName = service.ServiceName,                             
            //                    ServicePrice = service.ServicePrice ?? 0
            //                };
            //                //var serviceByTourSegmentss = await _unitOfWork.ServiceByTourSegmentRepository.GetByConditionAsync(i => i.ServiceId == service.ServiceId && i.Status != -1);

            //                //foreach (var serviceByTourSegment in serviceByTourSegmentss)
            //                //{
            //                //    var serviceByTourSegmentResponse = new FServiceByTourSegment
            //                //    {
            //                //        SBTSId = serviceByTourSegment.SBTSId,
            //                //        TourSegmentId = serviceByTourSegment.TourSegmentId,
            //                //        ServiceId = serviceByTourSegment.ServiceId,
            //                //        Status = (EStatus)serviceByTourSegment.Status
            //                //    };

            //                //    // Thêm serviceByTourSegmentResponse vào serviceResponse
            //                //    serviceResponse.ServiceByTourSegments.Add(serviceByTourSegmentResponse);
            //                //}

            //                locationResponse.Services.Add(serviceResponse);
            //            }


            //            destinationResponse.Locations.Add(locationResponse);
            //        }

            //        response.Destinations.Add(destinationResponse);

            //    }
            //}
            //return new APIResponseModel
            //{
            //    Message = "PackageTour found",
            //    IsSuccess = true,
            //    Data = response
            //};
            #endregion
            try
            {
                var packageToursRespone = await _unitOfWork.PackageTourRepository.GetAllAsyncs(query => query
     .Where(pt => pt.PackageTourId == id && pt.Status == 1)
     .Include(pt => pt.TourSegments)
         .ThenInclude(ts => ts.Destinations)
         .ThenInclude(d => d.Locations)
         .ThenInclude(l => l.Services)
         .ThenInclude(sbt => sbt.ServiceByTourSegments)
     .Include(pt => pt.TicketTypes)
 );

                if (packageToursRespone == null || !packageToursRespone.Any())
                {
                    return new APIResponseModel
                    {
                        Message = "No Package Tours Found",
                        IsSuccess = false,
                    };
                }

                var resultList = new List<object>();

                foreach (var packageTour in packageToursRespone)
                {
                    // Tính tổng giá của tất cả dịch vụ hợp lệ có statusInServiceByTourSegment != -1
                    float totalServicePrice = packageTour.TourSegments
                        .SelectMany(ts => ts.ServiceByTourSegments)
                        .Where(sbts => sbts.Status != -1 && sbts.Services?.Status == 1) // Chỉ tính dịch vụ có StatusInServiceByTourSegment != -1
                        .Sum(sbts => sbts.Services?.ServicePrice ?? 0); // Tính tổng giá

                    // Cập nhật PackageTourPrice với tổng giá dịch vụ
                    packageTour.PackageTourPrice = totalServicePrice;

                    // Gọi phương thức cập nhật của repository để lưu thay đổi
                   await _unitOfWork.PackageTourRepository.UpdateAsync(packageTour);

                    var result = new
                    {
                        PackageTour = new
                        {
                            packageTour.PackageTourId,
                            packageTour.PackageTourName,
                            packageTour.PackageTourPrice, // Giá đã được cập nhật
                            packageTour.PackageTourImgUrl,
                            StatusPackageTour = packageTour.Status,
                            packageTour.Cities?.CityName,
                            TourSegments = packageTour.TourSegments
                                .Where(ts => ts.Status == 1)
                                .Select(ts => new
                                {
                                    ts.TourSegmentId,
                                    ts.DestinationId,
                                    ts.Destinations?.DestinationName,
                                    StatusDestinations = ts.Destinations?.Status,
                                    Locations = ts.Destinations?.Locations
                                        .Where(l => l.Status == 1 &&
                                                    l.DestinationId == ts.DestinationId &&
                                                    packageTour.PackageTourId == ts.PackageTourId &&
                                                    ts.ServiceByTourSegments.Any(sbts => sbts.Services?.LocationId == l.LocationId))
                                        .Select(l => new
                                        {
                                            l.LocationId,
                                            l.LocationName,
                                            l.LocationImgUrl,
                                            //l.LocationType,
                                            l.DestinationId,
                                            Services = ts.ServiceByTourSegments
                                                .Where(sbts => sbts.Services?.LocationId == l.LocationId)
                                                .Select(s => new
                                                {
                                                    s.Services?.ServiceId,
                                                    s.Services?.ServiceName,
                                                    s.Services?.ServicePrice,
                                                    s.Services?.Suppliers?.SupplierName,
                                                    s.Services?.LocationId,
                                                    s.Services?.ServiceTypes?.ServiceTypeName,
                                                    StatusServices = s.Services?.Status,

                                                    // Lấy thêm status từ bảng ServiceByTourSegment
                                                    StatusInServiceByTourSegment = s.Status,

                                                    // Cộng giá dịch vụ có StatusInServiceByTourSegment != -1
                                                    CalculatedServicePrice = s.Status != -1 ? s.Services?.ServicePrice ?? 0 : 0
                                                }).ToList(),
                                        }).ToList(),
                                }).ToList(),

                            // Tổng giá dịch vụ
                            TotalServicePrice = totalServicePrice
                        }
                    };

                    resultList.Add(result);
                }

                // Lưu thay đổi vào cơ sở dữ liệu sau khi cập nhật giá package tour
                 _unitOfWork.Save();

                // Trả về kết quả
                return new APIResponseModel
                {
                    IsSuccess = true,
                    Data = resultList,
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

        public async Task<APIResponseModel> CreatePartsPackageTourFlowAsync(FPackageTourUpdate updateModel)
        {
            var existingPackageTour = await _unitOfWork.PackageTourRepository.GetByIdGuidAsync(updateModel.PackageTourId);
            if (existingPackageTour == null)
            {
                return new APIResponseModel
                {
                    Message = "PackageTour not found",
                    IsSuccess = false
                };
            }
            existingPackageTour.PackageTourName = updateModel.PackageTourName;
            existingPackageTour.Status = 1;
            existingPackageTour.UpdateDate = DateTime.Now;
            existingPackageTour.CityId = updateModel.CityId;
            existingPackageTour.PackageTourImgUrl = updateModel.PackageTourImgUrl;
            float totalPrice = existingPackageTour.PackageTourPrice ?? 0;
            foreach (var destinationModel in updateModel.Destinations)
            {
                var destination = new Destination
                {
                    DestinationId = Guid.NewGuid().ToString(),
                    DestinationName = destinationModel.DestinationName,
                    CityId = updateModel.CityId,
                    CreateDate = DateTime.Now,
                    Status = 1,
                    Locations = new List<Location>()
                };


                // Iterate through each location in the destination
                foreach (var locationModel in destinationModel.Locations)
                {
                    var location = new Location
                    {
                        LocationId = Guid.NewGuid().ToString(),
                        LocationName = locationModel.LocationName,
                        //LocationType = locationModel.LocationType,
                        DestinationId = destination.DestinationId,
                        CreateDate = DateTime.Now,
                        Status = 1,
                        Services = new List<Service>()
                    };

                    // Iterate through each service in the location
                    foreach (var serviceModel in locationModel.Services)
                    {
                        var service = new Service
                        {
                            ServiceId = Guid.NewGuid().ToString(),
                            ServiceName = serviceModel.ServiceName,
                            ServicePrice = serviceModel.ServicePrice,
                            Status = 1,
                            CreateDate = DateTime.Now,
                        };

                        // Add service price to total price
                        totalPrice += service.ServicePrice ?? 0;

                        // Add service to the location
                        location.Services.Add(service);
                        await _unitOfWork.ServiceRepository.AddAsync(service);
                    }

                    // Add location to the destination
                    destination.Locations.Add(location);
                    await _unitOfWork.LocationRepository.AddAsync(location);
                }
                existingPackageTour.PackageTourPrice = totalPrice;

                // Add TourSegment for the destination
                var tourSegment = new TourSegment
                {
                    TourSegmentId = Guid.NewGuid().ToString(),
                    DestinationId = destination.DestinationId,
                    PackageTourId = updateModel.PackageTourId.ToString(),
                    CreateDate = DateTime.Now,
                    Status = 1
                };

                existingPackageTour.TourSegments.Add(tourSegment);

                await _unitOfWork.DestinationRepository.AddAsync(destination);
                await _unitOfWork.TourSegmentRepository.AddAsync(tourSegment);
                foreach (var location in destination.Locations)
                {
                    foreach (var service in location.Services)
                    {
                        var serviceByTourSegment = new ServiceByTourSegment
                        {
                            SBTSId = Guid.NewGuid().ToString(),
                            TourSegmentId = tourSegment.TourSegmentId,
                            ServiceId = service.ServiceId,
                            Status = 1,
                            CreateDate = DateTime.Now
                        };

                        await _unitOfWork.ServiceByTourSegmentRepository.AddAsync(serviceByTourSegment);
                    }
                }

                _unitOfWork.Save();

            }

            var packageTour = await _unitOfWork.PackageTourRepository.GetByIdStringAsync(existingPackageTour.PackageTourId);
            var toursegment = await _unitOfWork.TourSegmentRepository.GetByConditionAsync(i => i.PackageTourId == packageTour.PackageTourId && i.Status != -1);


            #region Respone
            var packageToursResponeMess = await _unitOfWork.PackageTourRepository.GetAllAsyncs(query => query
                    .Where(pt => pt.PackageTourId == updateModel.PackageTourId.ToString())
                    .Include(pt => pt.TourSegments)
                        .ThenInclude(ts => ts.Destinations)
                        .ThenInclude(d => d.Locations)
                        .ThenInclude(l => l.Services)
                        .ThenInclude(sbt => sbt.ServiceByTourSegments)
                    .Include(pt => pt.TicketTypes)
                );
            var resultList = new List<object>();

            foreach (var packageTourRespone in packageToursResponeMess)
            {
                var result = new
                {
                    PackageTour = new
                    {
                        packageTourRespone.PackageTourId,
                        packageTourRespone.PackageTourName,
                        packageTourRespone.PackageTourPrice,
                        packageTourRespone.PackageTourImgUrl,
                        StatusPackageTour = packageTourRespone.Status,
                        packageTourRespone.Cities?.CityName,
                        TourSegments = packageTourRespone.TourSegments
                            .Where(ts => ts.Status == 1)
                            .Select(ts => new
                            {
                                ts.TourSegmentId,
                                ts.DestinationId,
                                ts.Destinations?.DestinationName,
                                StatusDestinations = ts.Destinations?.Status,
                                Locations = ts.Destinations?.Locations
                                    .Where(l => l.Status == 1 &&
                                                l.DestinationId == ts.DestinationId &&
                                                packageTour.PackageTourId == ts.PackageTourId &&
                                                ts.ServiceByTourSegments.Any(sbts => sbts.Services?.LocationId == l.LocationId))
                                    .Select(l => new
                                    {
                                        l.LocationId,
                                        l.LocationName,
                                        l.LocationImgUrl,
                                        //l.LocationType,
                                        l.DestinationId,
                                        Services = ts.ServiceByTourSegments
                                            .Where(sbts => sbts.Services?.LocationId == l.LocationId
                                                           && sbts.Services?.Status == 1)
                                            .Select(s => new
                                            {
                                                s.Services?.ServiceId,
                                                s.Services?.ServiceName,
                                                s.Services?.ServicePrice,
                                                s.Services?.Suppliers?.SupplierName,
                                                s.Services?.LocationId,
                                                s.Services?.ServiceTypes?.ServiceTypeName,
                                                StatusServices = s.Services?.Status,
                                            }).ToList(),
                                    }).ToList(),
                            }).ToList(),
                    }
                };

                resultList.Add(result);
            }
            #endregion
            return new APIResponseModel
            {
                Message = "PackageTour updated successfully",
                IsSuccess = true,
                Data = resultList
            };

        }
        public async Task<APIResponseModel> UpdatePackageTourFlowAsync(FPackageTourUpdateModel updateModel)
        {
            if (updateModel == null || updateModel.Destinations == null || !updateModel.Destinations.Any())
            {
                return new APIResponseModel
                {
                    Message = "Invalid input data or no destinations provided",
                    IsSuccess = false
                };
            }

            // Fetch the existing PackageTour
            var existingPackageTour = await _unitOfWork.PackageTourRepository.GetByIdGuidAsync(updateModel.PackageTourId);
            if (existingPackageTour == null)
            {
                return new APIResponseModel
                {
                    Message = "PackageTour not found",
                    IsSuccess = false
                };
            }

            float totalServicePrice = existingPackageTour.PackageTourPrice ?? 0;  // To store the total price of all services

            // Loop through the list of provided destinations
            foreach (var dest in updateModel.Destinations)
            {

                // Check if the destination exists
                var destination = await _unitOfWork.DestinationRepository.GetByIdStringAsync(dest.DestinationId);

                // Retrieve tour segment ID for the existing package tour and destination
                var tourSegment = await _unitOfWork.TourSegmentRepository.GetByConditionAsync(i =>
                    i.DestinationId == destination.DestinationId &&
                    i.PackageTourId == existingPackageTour.PackageTourId.ToString());
                if (dest.Status == -1)
                {
                    if (tourSegment != null && tourSegment.Any())
                    {
                        // Lấy tour segment cần update
                        var existingTourSegment = tourSegment.FirstOrDefault();
                        existingTourSegment.Status = -1; // Cập nhật status của tour segment thành -1
                        existingTourSegment.UpdateDate = DateTime.Now; // Có thể thêm ngày cập nhật nếu cần

                        // Update tour segment
                        await _unitOfWork.TourSegmentRepository.UpdateAsync(existingTourSegment);
                    }
                    continue;
                }
                if (tourSegment == null || !tourSegment.Any())
                {
                    var tourSegmentAdd = new TourSegment
                    {
                        TourSegmentId = Guid.NewGuid().ToString(),
                        DestinationId = destination.DestinationId,
                        PackageTourId = existingPackageTour.PackageTourId.ToString(),
                        CreateDate = DateTime.Now,
                        Status = 1
                    };
                    existingPackageTour.TourSegments.Add(tourSegmentAdd);

                    // Assuming you save the new tour segment here
                    await _unitOfWork.TourSegmentRepository.AddAsync(tourSegmentAdd);
                }

                else
                {
                    // Get the first tour segment found (assuming you expect one or the first one is valid)
                    var tourSegmentId = tourSegment.First().TourSegmentId;

                    // Loop through the locations in this destination
                    if (dest.Locations != null && dest.Locations.Any())
                    {
                        foreach (var loc in dest.Locations)
                        {
                            // Check if the location exists and belongs to the current destination
                            var location = await _unitOfWork.LocationRepository.GetByIdStringAsync(loc.LocationId);

                            // Assuming LocationRepository and checks are correct
                            var locationId = await _unitOfWork.LocationRepository.GetByConditionAsync(i =>
                                i.DestinationId == destination.DestinationId &&
                                i.LocationId == location.LocationId);

                            if (location == null || location.DestinationId != destination.DestinationId)
                            {
                                continue; // Skip this location if it doesn't exist or doesn't belong to the current destination
                            }

                            // Loop through the services in this location
                            if (loc.Services != null && loc.Services.Any())
                            {
                                foreach (var svc in loc.Services)
                                {
                                    // Check if the service exists and belongs to the current location
                                    var service = await _unitOfWork.ServiceRepository.GetByIdStringAsync(svc.ServiceId);
                                    var serviceByTourSegment = await _unitOfWork.ServiceByTourSegmentRepository.GetByConditionAsync(i =>
                                        i.ServiceId == svc.ServiceId &&
                                        i.TourSegmentId == tourSegmentId);

                                    // Nếu status của dịch vụ là -1, cập nhật status của ServiceByTourSegment
                                    if (svc.Status == -1)
                                    {
                                        if (serviceByTourSegment != null && serviceByTourSegment.Any())
                                        {
                                            var existingServiceByTourSegment = serviceByTourSegment.FirstOrDefault();
                                            existingServiceByTourSegment.Status = -1; // Cập nhật status thành -1
                                            existingServiceByTourSegment.UpdateDate = DateTime.Now; // Cập nhật ngày

                                            // Update ServiceByTourSegment
                                            await _unitOfWork.ServiceByTourSegmentRepository.UpdateAsync(existingServiceByTourSegment);
                                        }
                                        // Subtract the service price if status is -1
                                        totalServicePrice -= service.ServicePrice ?? 0;
                                    }
                                    else
                                    {
                                        // Nếu serviceByTourSegment không tồn tại, tạo mới
                                        if (serviceByTourSegment == null || !serviceByTourSegment.Any())
                                        {
                                            // Create a ServiceByTourSegment for the valid service
                                            var serviceByTourSegmentAdd = new ServiceByTourSegment
                                            {
                                                SBTSId = Guid.NewGuid().ToString(),
                                                TourSegmentId = tourSegmentId,
                                                ServiceId = svc.ServiceId,
                                                Status = 1, // Cài đặt mặc định status là 1 khi tạo mới
                                                CreateDate = DateTime.Now
                                            };
                                            await _unitOfWork.ServiceByTourSegmentRepository.AddAsync(serviceByTourSegmentAdd);
                                        }

                                        // Add the service price for other statuses
                                        totalServicePrice += service.ServicePrice ?? 0;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            // Update the PackageTour price with the total service price
            existingPackageTour.PackageTourPrice = totalServicePrice;
            var packageTour = await _unitOfWork.PackageTourRepository.GetByIdStringAsync(existingPackageTour.PackageTourId);
            var toursegment = await _unitOfWork.TourSegmentRepository.GetByConditionAsync(i => i.PackageTourId == packageTour.PackageTourId && i.Status != -1);



            var packageToursResponeMess = await _unitOfWork.PackageTourRepository.GetAllAsyncs(query => query
     .Where(pt => pt.PackageTourId == updateModel.PackageTourId.ToString())
     .Include(pt => pt.TourSegments)
         .ThenInclude(ts => ts.Destinations)
         .ThenInclude(d => d.Locations)
         .ThenInclude(l => l.Services)
         .ThenInclude(sbt => sbt.ServiceByTourSegments)
     .Include(pt => pt.TicketTypes)
 );

            var resultList = new List<object>();

            foreach (var packageTourRespone in packageToursResponeMess)
            {
                // Khởi tạo biến để tính tổng giá dịch vụ, đổi tên thành calculatedTotalPrice
                float calculatedTotalPrice = 0;

                var result = new
                {
                    PackageTour = new
                    {
                        packageTourRespone.PackageTourId,
                        packageTourRespone.PackageTourName,
                        packageTourRespone.PackageTourPrice, // Giá ban đầu
                        packageTourRespone.PackageTourImgUrl,
                        StatusPackageTour = packageTourRespone.Status,
                        packageTourRespone.Cities?.CityName,
                        TourSegments = packageTourRespone.TourSegments
                            .Where(ts => ts.Status == 1)
                            .Select(ts => new
                            {
                                ts.TourSegmentId,
                                ts.DestinationId,
                                ts.Destinations?.DestinationName,
                                StatusDestinations = ts.Destinations?.Status,
                                Locations = ts.Destinations?.Locations
                                    .Where(l => l.Status == 1 &&
                                                l.DestinationId == ts.DestinationId &&
                                                packageTourRespone.PackageTourId == ts.PackageTourId &&
                                                ts.ServiceByTourSegments.Any(sbts => sbts.Services?.LocationId == l.LocationId))
                                    .Select(l => new
                                    {
                                        l.LocationId,
                                        l.LocationName,
                                        l.LocationImgUrl,
                                        //l.LocationType,
                                        l.DestinationId,
                                        Services = ts.ServiceByTourSegments
                                            .Where(sbts => sbts.Services?.LocationId == l.LocationId && sbts.Status != -1 && sbts.Services?.Status == 1)
                                            .Select(s => new
                                            {
                                                s.Services?.ServiceId,
                                                s.Services?.ServiceName,
                                                s.Services?.ServicePrice,
                                                s.Services?.Suppliers?.SupplierName,
                                                s.Services?.LocationId,
                                                s.Services?.ServiceTypes?.ServiceTypeName,
                                                StatusServices = s.Services?.Status,

                                                // Cộng tổng giá của các dịch vụ hợp lệ
                                                CalculatedServicePrice = s.Services?.ServicePrice ?? 0
                                            }).ToList()
                                    }).ToList(),
                            }).ToList(),

                        // Tính tổng giá dịch vụ trong tất cả các tour segment
                        TotalServicePrice = packageTourRespone.TourSegments
                            .SelectMany(ts => ts.ServiceByTourSegments)
                            .Where(sbts => sbts.Status != -1 && sbts.Services?.Status == 1)
                            .Sum(sbts => sbts.Services?.ServicePrice ?? 0), // Tính tổng giá
                    }
                };

                // Cập nhật giá PackageTourPrice bằng tổng giá dịch vụ
                packageTourRespone.PackageTourPrice = calculatedTotalPrice;

                resultList.Add(result);
            }

            // Save changes to the database
            _unitOfWork.Save();

            return new APIResponseModel
            {
                Message = $"Valid parts added to PackageTour successfully",
                IsSuccess = true,
                Data = resultList

            };

        }

        public async Task<APIResponseModel> AddPartToPackageTourFlow(FPackageTourUpdateModel createModel)
        {
            if (createModel == null || createModel.Destinations == null || !createModel.Destinations.Any())
            {
                return new APIResponseModel
                {
                    Message = "Invalid input data or no destinations provided",
                    IsSuccess = false
                };
            }

            // Fetch the existing PackageTour
            var existingPackageTour = await _unitOfWork.PackageTourRepository.GetByIdGuidAsync(createModel.PackageTourId);
            if (existingPackageTour == null)
            {
                return new APIResponseModel
                {
                    Message = "PackageTour not found",
                    IsSuccess = false
                };
            }

            float totalServicePrice = existingPackageTour.PackageTourPrice ?? 0;  // To store the total price of all services

            // Loop through the list of provided destinations
            foreach (var dest in createModel.Destinations)
            {
                // Check if the destination exists
                var destination = await _unitOfWork.DestinationRepository.GetByIdStringAsync(dest.DestinationId);
                if (destination == null)
                {
                    continue; // Skip this destination if it doesn't exist
                }

                // Create a new TourSegment for the valid destination
                var tourSegment = new TourSegment
                {
                    TourSegmentId = Guid.NewGuid().ToString(),
                    DestinationId = destination.DestinationId,
                    PackageTourId = existingPackageTour.PackageTourId.ToString(),
                    CreateDate = DateTime.Now,
                    Status = 1
                };

                // Add the TourSegment to the PackageTour
                existingPackageTour.TourSegments.Add(tourSegment);

                // Loop through the locations in this destination
                if (dest.Locations != null && dest.Locations.Any())
                {
                    foreach (var loc in dest.Locations)
                    {
                        // Check if the location exists and belongs to the current destination
                        var location = await _unitOfWork.LocationRepository.GetByIdStringAsync(loc.LocationId);
                        if (location == null || location.DestinationId != destination.DestinationId)
                        {
                            continue; // Skip this location if it doesn't exist or doesn't belong to the current destination
                        }

                        // Loop through the services in this location
                        if (loc.Services != null && loc.Services.Any())
                        {
                            foreach (var svc in loc.Services)
                            {
                                // Check if the service exists and belongs to the current location
                                var service = await _unitOfWork.ServiceRepository.GetByIdStringAsync(svc.ServiceId);
                                if (service == null || service.LocationId != location.LocationId)
                                {
                                    continue; // Skip this service if it doesn't exist or doesn't belong to the current location
                                }

                                // Create a ServiceByTourSegment for the valid service
                                var serviceByTourSegment = new ServiceByTourSegment
                                {
                                    SBTSId = Guid.NewGuid().ToString(),
                                    TourSegmentId = tourSegment.TourSegmentId,
                                    ServiceId = service.ServiceId,
                                    Status = 1,
                                    CreateDate = DateTime.Now
                                };

                                // Add the ServiceByTourSegment to the unit of work
                                await _unitOfWork.ServiceByTourSegmentRepository.AddAsync(serviceByTourSegment);

                                // Add the price of the current service to the total price
                                if (svc.Status == -1)
                                {
                                    // Subtract the service price if status is -1
                                    //totalServicePrice -= service.ServicePrice ?? 0;
                                }
                                else
                                {
                                    // Add the service price for other statuses
                                    totalServicePrice += service.ServicePrice ?? 0;
                                }
                            }
                        }
                    }
                }
            }

            // Update the PackageTour price with the total service price
            existingPackageTour.PackageTourPrice = totalServicePrice;
            var packageToursResponeMess = await _unitOfWork.PackageTourRepository.GetAllAsyncs(query => query
     .Where(pt => pt.PackageTourId == createModel.PackageTourId.ToString())
     .Include(pt => pt.TourSegments)
         .ThenInclude(ts => ts.Destinations)
         .ThenInclude(d => d.Locations)
         .ThenInclude(l => l.Services)
         .ThenInclude(sbt => sbt.ServiceByTourSegments)
     .Include(pt => pt.TicketTypes)
 );

            var resultList = new List<object>();

            foreach (var packageTourRespone in packageToursResponeMess)
            {
                // Khởi tạo biến để tính tổng giá dịch vụ, đổi tên thành calculatedTotalPrice
                float calculatedTotalPrice = 0;

                var result = new
                {
                    PackageTour = new
                    {
                        packageTourRespone.PackageTourId,
                        packageTourRespone.PackageTourName,
                        packageTourRespone.PackageTourPrice, // Giá ban đầu
                        packageTourRespone.PackageTourImgUrl,
                        StatusPackageTour = packageTourRespone.Status,
                        packageTourRespone.Cities?.CityName,
                        TourSegments = packageTourRespone.TourSegments
                            .Where(ts => ts.Status == 1)
                            .Select(ts => new
                            {
                                ts.TourSegmentId,
                                ts.DestinationId,
                                ts.Destinations?.DestinationName,
                                StatusDestinations = ts.Destinations?.Status,
                                Locations = ts.Destinations?.Locations
                                    .Where(l => l.Status == 1 &&
                                                l.DestinationId == ts.DestinationId &&
                                                packageTourRespone.PackageTourId == ts.PackageTourId &&
                                                ts.ServiceByTourSegments.Any(sbts => sbts.Services?.LocationId == l.LocationId))
                                    .Select(l => new
                                    {
                                        l.LocationId,
                                        l.LocationName,
                                        l.LocationImgUrl,
                                     
                                        l.DestinationId,
                                        Services = ts.ServiceByTourSegments
                                            .Where(sbts => sbts.Services?.LocationId == l.LocationId && sbts.Status != -1 && sbts.Services?.Status == 1)
                                            .Select(s => new
                                            {
                                                s.Services?.ServiceId,
                                                s.Services?.ServiceName,
                                                s.Services?.ServicePrice,
                                                s.Services?.Suppliers?.SupplierName,
                                                s.Services?.LocationId,
                                                s.Services?.ServiceTypes?.ServiceTypeName,
                                                StatusServices = s.Services?.Status,

                                                // Cộng tổng giá của các dịch vụ hợp lệ
                                                CalculatedServicePrice = s.Services?.ServicePrice ?? 0
                                            }).ToList()
                                    }).ToList(),
                            }).ToList(),

                        // Tính tổng giá dịch vụ trong tất cả các tour segment
                        TotalServicePrice = packageTourRespone.TourSegments
                            .SelectMany(ts => ts.ServiceByTourSegments)
                            .Where(sbts => sbts.Status != -1 && sbts.Services?.Status == 1)
                            .Sum(sbts => sbts.Services?.ServicePrice ?? 0), // Tính tổng giá
                    }
                };

                // Cập nhật giá PackageTourPrice bằng tổng giá dịch vụ
                packageTourRespone.PackageTourPrice = calculatedTotalPrice;

                resultList.Add(result);
            }

            // Save changes to the database
            _unitOfWork.Save();

            return new APIResponseModel
            {
                Message = $"Valid parts added to PackageTour successfully",
                IsSuccess = true,
                Data = resultList

            };
            // Save changes to the database
            

            
        }
    }
}

