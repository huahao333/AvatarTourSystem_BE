using AutoMapper;
using BusinessObjects.Models;
using BusinessObjects.ViewModels.DailyTour;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
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
    public class DailyTourFlowService : IDailyTourFlowService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public DailyTourFlowService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<APIResponseModel> CreateDailyTourFlow(DailyTourFlowModel dailyTourFlowModel)
        {
            try
            {
                var packageTourIdExisting = await _unitOfWork.PackageTourRepository.GetByIdStringAsync(dailyTourFlowModel.PackageTourId);
                if (packageTourIdExisting == null || string.IsNullOrEmpty(dailyTourFlowModel.PackageTourId))
                {
                    return new APIResponseModel
                    {
                        Message = "PackageTourId must exist.",
                        IsSuccess = false,
                    };
                }

                if (dailyTourFlowModel.DailyTourPrice <= 0)
                {
                    return new APIResponseModel
                    {
                        Message = "DailyTourPrice must be greater than 0.",
                        IsSuccess = false,
                    };
                }


                if (dailyTourFlowModel.StartDate < DateTime.Now)
                {
                    return new APIResponseModel
                    {
                        Message = "StartDate must be greater than or equal to the current date.",
                        IsSuccess = false,
                    };
                }

                if (dailyTourFlowModel.EndDate < DateTime.Now || dailyTourFlowModel.EndDate < dailyTourFlowModel.StartDate)
                {
                    return new APIResponseModel
                    {
                        Message = "EndDate must be greater than or equal to the current date and greater than StartDate.",
                        IsSuccess = false,
                    };
                }

                if (dailyTourFlowModel.Discount < 0)
                {
                    return new APIResponseModel
                    {
                        Message = "Discount must be greater than or equal to 0.",
                        IsSuccess = false,
                    };
                }

                if (string.IsNullOrEmpty(dailyTourFlowModel.TicketTypeIdAdult?.Trim()))
                {
                    return new APIResponseModel
                    {
                        Message = "TicketTypeId must exist.",
                        IsSuccess = false,
                    };
                }

                if (dailyTourFlowModel.CapacityByAdult < 0 || dailyTourFlowModel.CapacityByChildren<0)
                {
                    return new APIResponseModel
                    {
                        Message = "CapacityByAdult or CapacityByChildren must be greater than or equal to 0.",
                        IsSuccess = false,
                    };
                }

                if (dailyTourFlowModel.PriceByAdult < 0 || dailyTourFlowModel.PriceByChildren < 0)
                {
                    return new APIResponseModel
                    {
                        Message = "PriceByAdult or PriceByChildren must be greater than or equal to 0.",
                        IsSuccess = false,
                    };
                }



                var ticketTypeAdultExisting = await _unitOfWork.TicketTypeRepository.GetByIdStringAsync(dailyTourFlowModel.TicketTypeIdAdult);

                if (ticketTypeAdultExisting == null)
                {
                    return new APIResponseModel
                    {
                        Message = "TicketTypeIdAdult must exist.",
                        IsSuccess = false,
                    };
                }

                TicketType ticketTypeChildrenExisting = null;
                if (!string.IsNullOrEmpty(dailyTourFlowModel.TicketTypeIdChildren?.Trim()))
                {
                    ticketTypeChildrenExisting = await _unitOfWork.TicketTypeRepository.GetByIdStringAsync(dailyTourFlowModel.TicketTypeIdChildren);
                    if (ticketTypeChildrenExisting == null)
                    {
                        return new APIResponseModel
                        {
                            Message = "TicketTypeIdChildren must exist.",
                            IsSuccess = false,
                        };
                    }
                }

                var newDailyTourId = Guid.NewGuid();
                var dailyTour = new DailyTour
                {
                    DailyTourId = newDailyTourId.ToString(), 
                    PackageTourId = dailyTourFlowModel.PackageTourId,
                    DailyTourName = dailyTourFlowModel.DailyTourName,
                    Description = dailyTourFlowModel.Description,
                    DailyTourPrice = dailyTourFlowModel.DailyTourPrice,
                    ImgUrl = dailyTourFlowModel.ImgUrl,
                    StartDate = dailyTourFlowModel.StartDate,
                    EndDate = dailyTourFlowModel.EndDate,
                    Discount = dailyTourFlowModel.Discount,
                    Status = 1, 
                    CreateDate = DateTime.Now
                };

               
                //var createdDailyTour = await _unitOfWork.DailyTourRepository.AddAsync(dailyTour);
             //   _unitOfWork.Save();

                
                var dailyTicketAdult = new DailyTicket
                {
                    DailyTicketId = Guid.NewGuid().ToString(), 
                    DailyTourId = dailyTour.DailyTourId, 
                    TicketTypeId = dailyTourFlowModel.TicketTypeIdAdult, 
                    Capacity = dailyTourFlowModel.CapacityByAdult, 
                    DailyTicketPrice = dailyTourFlowModel.PriceByAdult,
                    Status = 1, 
                    CreateDate = DateTime.Now
                };

                DailyTicket dailyTicketChildren = null;
                if (!string.IsNullOrEmpty(dailyTourFlowModel.TicketTypeIdChildren?.Trim()))
                {
                    dailyTicketChildren = new DailyTicket
                    {
                        DailyTicketId = Guid.NewGuid().ToString(),
                        DailyTourId = dailyTour.DailyTourId,
                        TicketTypeId = dailyTourFlowModel.TicketTypeIdChildren,
                        Capacity = dailyTourFlowModel.CapacityByChildren,
                        DailyTicketPrice = dailyTourFlowModel.PriceByChildren,
                        Status = 1,
                        CreateDate = DateTime.Now
                    };
                }
                await _unitOfWork.DailyTourRepository.AddAsync(dailyTour);
                await _unitOfWork.DailyTicketRepository.AddAsync(dailyTicketAdult);

                if (dailyTicketChildren != null)
                {
                    await _unitOfWork.DailyTicketRepository.AddAsync(dailyTicketChildren);
                }
                _unitOfWork.Save();

                return new APIResponseModel
                {
                    Message = "DailyTour and Tickets created successfully",
                    IsSuccess = true,
                    //Data = new
                    //{
                    //    DailyTour = dailyTour,
                    //    DailyTicketAdult = dailyTicketAdult,
                    //    DailyTicketChildren = dailyTicketChildren
                    //}
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

        //public async Task<APIResponseModel> GetDailyTourDetails(string dailyTourId)
        //{
        //    try
        //    {
        //        var dailyTour = await _unitOfWork.DailyTourRepository.GetByIdStringAsync(dailyTourId);

        //        if (dailyTour == null)
        //        {
        //            return new APIResponseModel
        //            {
        //                Message = "Not Found",
        //                IsSuccess = false,
        //            };
        //        }

        //        var packageTour = await _unitOfWork.PackageTourRepository.GetByIdStringAsync(dailyTour.PackageTourId);

        //        var tourSegments = await _unitOfWork.TourSegmentRepository.GetByConditionAsync(ts => ts.PackageTourId == packageTour.PackageTourId);

        //        var ticketTypes = await _unitOfWork.TicketTypeRepository.GetByConditionAsync(t => t.PackageTourId == packageTour.PackageTourId);

        //        var destinationIds = tourSegments.Select(ts => ts.DestinationId).Distinct();
        //        var destinations = await _unitOfWork.DestinationRepository.GetByConditionAsync(d => destinationIds.Contains(d.DestinationId));

        //        var tourSegmentIds = tourSegments.Select(ts => ts.TourSegmentId).Distinct();
        //        var serviceByTourSegments = await _unitOfWork.ServiceByTourSegmentRepository.GetByConditionAsync(sbts => tourSegmentIds.Contains(sbts.TourSegmentId));

        //        var serviceIdsInTourSegments = serviceByTourSegments.Select(sbts => sbts.ServiceId).Distinct();
        //        var services = await _unitOfWork.ServiceRepository.GetByConditionAsync(s => serviceIdsInTourSegments.Contains(s.ServiceId));

        //        var locationIds = services.Select(s => s.LocationId).Distinct();
        //        var locations = await _unitOfWork.LocationRepository.GetByConditionAsync(l => locationIds.Contains(l.LocationId));

        //        var result = new
        //        {
        //            DailyTour = new
        //            {
        //                dailyTour.DailyTourId,
        //                dailyTour.PackageTourId,
        //                dailyTour.DailyTourName,
        //                dailyTour.Description,
        //                dailyTour.DailyTourPrice,
        //                dailyTour.StartDate,
        //                dailyTour.EndDate,
        //                dailyTour.Discount,
        //                TicketTypes = ticketTypes.Select(tt => new
        //                {
        //                    tt.TicketTypeId,
        //                    tt.TicketTypeName,
        //                    tt.CreateDate
        //                }).ToList()
        //            },
        //            PackageTour = new
        //            {
        //                packageTour.PackageTourId,
        //                packageTour.PackageTourName,
        //                TourSegments = tourSegments.Select(ts => new
        //                {
        //                    ts.TourSegmentId,
        //                    ts.DestinationId,
        //                    Destinations = destinations
        //                        .Where(d => d.DestinationId == ts.DestinationId)
        //                        .Select(d => new
        //                        {
        //                            d.DestinationId,
        //                            d.DestinationName,
        //                            Locations = services
        //                                .Where(s => serviceByTourSegments
        //                                    .Any(sbts => sbts.TourSegmentId == ts.TourSegmentId && sbts.ServiceId == s.ServiceId)) // Lọc Service theo TourSegment
        //                                .Select(s => locations
        //                                    .Where(l => l.LocationId == s.LocationId)
        //                                    .Select(l => new
        //                                    {
        //                                        l.LocationId,
        //                                        l.LocationName,
        //                                        Services = services
        //                                            .Where(svc => svc.LocationId == l.LocationId)
        //                                            .Select(svc => new
        //                                            {
        //                                                svc.ServiceId,
        //                                                svc.ServiceName
        //                                            }).ToList()
        //                                    }).FirstOrDefault())
        //                        }).ToList()
        //                }).ToList()
        //            }
        //        };

        //        return new APIResponseModel
        //        {
        //            Message = "found",
        //            IsSuccess = true,
        //            Data = result
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

        //public async Task<APIResponseModel> GetAllDailyTours()
        //{
        //    try
        //    {
        //        // Retrieve all daily tours
        //        var dailyTours = await _unitOfWork.DailyTourRepository.GetAllAsync();

        //        if (dailyTours == null || !dailyTours.Any())
        //        {
        //            return new APIResponseModel
        //            {
        //                Message = "No Daily Tours Found",
        //                IsSuccess = false,
        //            };
        //        }

        //        var resultList = new List<object>();

        //        foreach (var dailyTour in dailyTours)
        //        {
        //            var packageTour = await _unitOfWork.PackageTourRepository.GetByIdStringAsync(dailyTour.PackageTourId);

        //            var tourSegments = await _unitOfWork.TourSegmentRepository.GetByConditionAsync(ts => ts.PackageTourId == packageTour.PackageTourId);

        //            var ticketTypes = await _unitOfWork.TicketTypeRepository.GetByConditionAsync(t => t.PackageTourId == packageTour.PackageTourId);

        //            var destinationIds = tourSegments.Select(ts => ts.DestinationId).Distinct();
        //            var destinations = await _unitOfWork.DestinationRepository.GetByConditionAsync(d => destinationIds.Contains(d.DestinationId));

        //            var tourSegmentIds = tourSegments.Select(ts => ts.TourSegmentId).Distinct();
        //            var serviceByTourSegments = await _unitOfWork.ServiceByTourSegmentRepository.GetByConditionAsync(sbts => tourSegmentIds.Contains(sbts.TourSegmentId));

        //            var serviceIdsInTourSegments = serviceByTourSegments.Select(sbts => sbts.ServiceId).Distinct();
        //            var services = await _unitOfWork.ServiceRepository.GetByConditionAsync(s => serviceIdsInTourSegments.Contains(s.ServiceId));

        //            var locationIds = services.Select(s => s.LocationId).Distinct();
        //            var locations = await _unitOfWork.LocationRepository.GetByConditionAsync(l => locationIds.Contains(l.LocationId));

        //            var result = new
        //            {
        //                DailyTour = new
        //                {
        //                    dailyTour.DailyTourId,
        //                    dailyTour.PackageTourId,
        //                    dailyTour.DailyTourName,
        //                    dailyTour.Description,
        //                    dailyTour.DailyTourPrice,
        //                    dailyTour.StartDate,
        //                    dailyTour.EndDate,
        //                    dailyTour.Discount,
        //                    TicketTypes = ticketTypes.Select(tt => new
        //                    {
        //                        tt.TicketTypeId,
        //                        tt.TicketTypeName,
        //                        tt.CreateDate
        //                    }).ToList()
        //                },
        //                PackageTour = new
        //                {
        //                    packageTour.PackageTourId,
        //                    packageTour.PackageTourName,
        //                    TourSegments = tourSegments.Select(ts => new
        //                    {
        //                        ts.TourSegmentId,
        //                        ts.DestinationId,
        //                        Destinations = destinations
        //                            .Where(d => d.DestinationId == ts.DestinationId)
        //                            .Select(d => new
        //                            {
        //                                d.DestinationId,
        //                                d.DestinationName,
        //                                Locations = services
        //                                    .Where(s => serviceByTourSegments
        //                                        .Any(sbts => sbts.TourSegmentId == ts.TourSegmentId && sbts.ServiceId == s.ServiceId)) // Filter Services by TourSegment
        //                                    .Select(s => locations
        //                                        .Where(l => l.LocationId == s.LocationId)
        //                                        .Select(l => new
        //                                        {
        //                                            l.LocationId,
        //                                            l.LocationName,
        //                                            Services = services
        //                                                .Where(svc => svc.LocationId == l.LocationId)
        //                                                .Select(svc => new
        //                                                {
        //                                                    svc.ServiceId,
        //                                                    svc.ServiceName
        //                                                }).ToList()
        //                                        }).FirstOrDefault())
        //                            }).ToList()
        //                    }).ToList()
        //                }
        //            };

        //            resultList.Add(result);
        //        }

        //        return new APIResponseModel
        //        {
        //            Message = "Found",
        //            IsSuccess = true,
        //            Data = resultList
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

        public async Task<APIResponseModel> GetDailyTourDetails(string dailyTourId)
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
                    return new APIResponseModel
                    {
                        Message = "DailyTour not found",
                        IsSuccess = false,
                    };
                }
                //var pointOfI = await _unitOfWork.PointOfInterestRepository.GetAllAsyncs(query => query
                //                          .Where(c => c.POITypeId == c.POITypes.POITypeId)
                //                          .Include(type => type.POITypes));

                var result = new
                {
                    DailyTour = new
                    {
                        dailyTour.DailyTourId,
                        dailyTour.PackageTourId,
                        dailyTour.DailyTourName,
                        dailyTour.Description,
                        dailyTour.DailyTourPrice,
                        dailyTour.ImgUrl,
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
                                tt.DailyTicketPrice,
                                tt.CreateDate
                            }).ToList()
                    },

                    PackageTour = new
                    {
                        dailyTour.PackageTours?.PackageTourId,
                        dailyTour.PackageTours?.PackageTourName,
                        dailyTour.PackageTours?.PackageTourPrice,
                        dailyTour.PackageTours?.PackageTourImgUrl,
                        StatusPackageTour = dailyTour.PackageTours?.Status,
                        dailyTour.PackageTours?.Cities?.CityName,
                        TourSegments = dailyTour.PackageTours?.TourSegments
                            .Where(ts => ts.Status == 1)
                            .Select(ts => new
                            {
                                ts.TourSegmentId,
                                ts.DestinationId,
                                ts.Destinations?.DestinationName,
                                StatusDestinations = ts.Destinations?.Status,
                                Locations = ts.Destinations?.Locations
                                    .Where(c => c.Status == 1 &&
                                                c.DestinationId == ts.DestinationId &&
                                                dailyTour.PackageTours?.PackageTourId == ts.PackageTourId &&
                                                ts.ServiceByTourSegments.Any(sbts => sbts.Services?.LocationId == c.LocationId))
                                    .Select(lo => new
                                    {
                                        lo.LocationId,
                                        lo.LocationName,
                                        lo.LocationImgUrl,
                                        //lo.LocationType,
                                        lo.DestinationId,
                                        //PointOfInterests = pointOfI.Where(poi => poi.LocationId == lo.LocationId).Select(type => new
                                        //{
                                        //    type.PointId,
                                        //    type.PointName,
                                        //    type.POITypes?.POITypeId,
                                        //    type.POITypes?.POITypeName,
                                        //}),
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

                return new APIResponseModel
                {
                    Message = "DailyTour found",
                    IsSuccess = true,
                    Data = result
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




        public async Task<APIResponseModel> GetAllDailysTours()
        {
            try
            {
                var dailyTours = await _unitOfWork.DailyTourRepository.GetAllAsyncs(query => query
                   .Where(dt=>dt.Status==1)
                    .Include(dt => dt.PackageTours)
                        .ThenInclude(pt => pt.TourSegments)
                           .ThenInclude(des=>des.Destinations)
                                .ThenInclude(lo=>lo.Locations)
                                 .ThenInclude(l => l.Services)
                                  .ThenInclude(sbt=>sbt.ServiceByTourSegments)
                    .Include(dt => dt.DailyTickets)
                        .ThenInclude(dt => dt.TicketTypes)
                );

                if (dailyTours == null || !dailyTours.Any())
                {
                    return new APIResponseModel
                    {
                        Message = "No Daily Tours Found",
                        IsSuccess = false,
                    };
                }

                var resultList = new List<object>();

                foreach (var dailyTour in dailyTours)
                {
                    //var pointOfI = await _unitOfWork.PointOfInterestRepository.GetAllAsyncs(query => query
                    //                      .Where(c=> c.POITypeId == c.POITypes.POITypeId)
                    //                      .Include(type=>type.POITypes));
                    var result = new
                    {
                        DailyTour = new
                        {
                            dailyTour.DailyTourId,
                            dailyTour.PackageTourId,
                            dailyTour.DailyTourName,
                            dailyTour.Description,
                            dailyTour.DailyTourPrice,
                            dailyTour.ImgUrl,
                            dailyTour.StartDate,
                            dailyTour.EndDate,
                            dailyTour.Discount,
                            StatusDailyTour = dailyTour.Status,
                            TicketTypes = dailyTour.DailyTickets
                                .Where(c => c.DailyTourId == dailyTour.DailyTourId
                                           && c.TicketTypes?.PackageTourId==dailyTour.PackageTourId
                                           && c.Capacity>0)
                                .Select(tt => new
                                {
                                    tt.DailyTicketId,
                                    tt.Capacity,
                                    tt.TicketTypes?.TicketTypeName,
                                    tt.DailyTicketPrice,
                                    tt.CreateDate
                                }).ToList()
                        },

                        PackageTour = new
                        {
                            dailyTour.PackageTours?.PackageTourId,
                            dailyTour.PackageTours?.PackageTourName,
                            dailyTour.PackageTours?.PackageTourPrice,
                            dailyTour.PackageTours?.PackageTourImgUrl,
                            StatusPackageTour = dailyTour.PackageTours?.Status,
                            dailyTour.PackageTours?.Cities?.CityName,
                            TourSegments = dailyTour.PackageTours?.TourSegments
                                .Where(ts => ts.Status == 1)
                                .Select(ts => new
                                {
                                    ts.TourSegmentId,
                                    ts.DestinationId,
                                    ts.Destinations?.DestinationName,
                                    StatusDestinations= ts.Destinations?.Status,
                                    Locations = ts.Destinations?.Locations
                                        .Where(c=>  c.Status==1 &&
                                                    c.DestinationId == ts.DestinationId &&
                                                    dailyTour.PackageTours?.PackageTourId ==ts.PackageTourId &&
                                                    ts.ServiceByTourSegments.Any(sbts=> sbts.Services?.LocationId==c.LocationId))
                                        .Select(lo => new
                                        {
                                            lo.LocationId,
                                            lo.LocationName,
                                            lo.LocationImgUrl,
                                            //lo.LocationType,
                                            lo.DestinationId,
                                            //PointOfInterests = pointOfI.Where(poi=> poi.LocationId==lo.LocationId).Select(type=> new
                                            //{
                                            //    type.PointId,
                                            //    type.PointName,
                                            //    type.POITypes?.POITypeId,
                                            //    type.POITypes?.POITypeName,
                                            //}),
                                            StatusLocation = lo.Status,
                                            Services = ts.ServiceByTourSegments
                                                .Where(sbts =>  sbts.Services?.LocationId == lo.LocationId
                                                               && sbts.Services?.Status==1)
                                                .Select(se => new {
                                                    se.Services?.ServiceId,
                                                    se.Services?.ServiceName,
                                                    se.Services?.ServicePrice,
                                                    se.Services?.Suppliers?.SupplierName,
                                                    se.Services?.LocationId,
                                                    se.Services?.ServiceTypes?.ServiceTypeName,
                                                    StatusServices= se.Services?.Status,
                                                }).ToList(),
                                        }).ToList(),
                                }).ToList(),
                        }
                    };

                    resultList.Add(result);
                }

                return new APIResponseModel
                {
                    Message = "Found",
                    IsSuccess = true,
                    Data = resultList
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

        public async Task<APIResponseModel> GetDailyToursHaveDiscount()
        {
            try
            {
                // Query all DailyTours with related entities in one query
                var dailyTours = await _unitOfWork.DailyTourRepository.GetAllAsyncs(query => query
                   .Where(dt => dt.Status == 1 && dt.Discount>0)
                    .Include(dt => dt.PackageTours)
                        .ThenInclude(pt => pt.TourSegments)
                           .ThenInclude(des => des.Destinations)
                                .ThenInclude(lo => lo.Locations)
                                 .ThenInclude(l => l.Services)
                                  .ThenInclude(sbt => sbt.ServiceByTourSegments)
                    .Include(dt => dt.DailyTickets)
                        .ThenInclude(dt => dt.TicketTypes)
                );

                if (dailyTours == null || !dailyTours.Any())
                {
                    return new APIResponseModel
                    {
                        Message = "No Daily Tours Found",
                        IsSuccess = false,
                    };
                }

                var resultList = new List<object>();

                foreach (var dailyTour in dailyTours)
                {
                    //var pointOfI = await _unitOfWork.PointOfInterestRepository.GetAllAsyncs(query => query
                    //                      .Where(c => c.POITypeId == c.POITypes.POITypeId)
                    //                      .Include(type => type.POITypes));

                    var result = new
                    {
                        DailyTour = new
                        {
                            dailyTour.DailyTourId,
                            dailyTour.PackageTourId,
                            dailyTour.DailyTourName,
                            dailyTour.Description,
                            dailyTour.DailyTourPrice,
                            dailyTour.ImgUrl,
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
                                    tt.DailyTicketPrice,
                                    tt.CreateDate
                                }).ToList()
                        },

                        PackageTour = new
                        {
                            dailyTour.PackageTours?.PackageTourId,
                            dailyTour.PackageTours?.PackageTourName,
                            dailyTour.PackageTours?.PackageTourPrice,
                            dailyTour.PackageTours?.PackageTourImgUrl,
                            StatusPackageTour = dailyTour.PackageTours?.Status,
                            dailyTour.PackageTours?.Cities?.CityName,
                            TourSegments = dailyTour.PackageTours?.TourSegments
                                .Where(ts => ts.Status == 1)
                                .Select(ts => new
                                {
                                    ts.TourSegmentId,
                                    ts.DestinationId,
                                    ts.Destinations?.DestinationName,
                                    StatusDestinations = ts.Destinations?.Status,
                                    Locations = ts.Destinations?.Locations
                                        .Where(c => c.Status == 1 &&
                                                    c.DestinationId == ts.DestinationId &&
                                                    dailyTour.PackageTours?.PackageTourId == ts.PackageTourId &&
                                                    ts.ServiceByTourSegments.Any(sbts => sbts.Services?.LocationId == c.LocationId))
                                        .Select(lo => new
                                        {
                                            lo.LocationId,
                                            lo.LocationName,
                                            lo.LocationImgUrl,
                                            //lo.LocationType,
                                            lo.DestinationId,
                                            //PointOfInterests = pointOfI.Where(poi => poi.LocationId == lo.LocationId).Select(type => new
                                            //{
                                            //    type.PointId,
                                            //    type.PointName,
                                            //    type.POITypes?.POITypeId,
                                            //    type.POITypes?.POITypeName,
                                            //}),
                                            StatusLocation = lo.Status,
                                            Services = ts.ServiceByTourSegments
                                                .Where(sbts => sbts.Services?.LocationId == lo.LocationId
                                                               && sbts.Services?.Status == 1)
                                                .Select(se => new {
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

                    resultList.Add(result);
                }

                return new APIResponseModel
                {
                    Message = "Found",
                    IsSuccess = true,
                    Data = resultList
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
        public async Task<APIResponseModel> GetDailyToursHavePOI()
        {
            try
            {
                // Query all DailyTours with related entities in one query
                var dailyTours = await _unitOfWork.DailyTourRepository.GetAllAsyncs(query => query
                   .Where(dt => dt.PackageTours.TourSegments.Any(c=>c.Destinations.Locations.Any(loca=>loca.PointOfInterests.Any(poi=>poi.LocationId==loca.LocationId))))
                    .Include(dt => dt.PackageTours)
                        .ThenInclude(pt => pt.TourSegments)
                           .ThenInclude(des => des.Destinations)
                                .ThenInclude(lo => lo.Locations)
                                 .ThenInclude(l => l.Services)
                                  .ThenInclude(sbt => sbt.ServiceByTourSegments)
                    .Include(dt => dt.DailyTickets)
                        .ThenInclude(dt => dt.TicketTypes)
                );

                if (dailyTours == null || !dailyTours.Any())
                {
                    return new APIResponseModel
                    {
                        Message = "No Daily Tours Found",
                        IsSuccess = false,
                    };
                }

                var resultList = new List<object>();

                foreach (var dailyTour in dailyTours)
                {
                    //var pointOfI = await _unitOfWork.PointOfInterestRepository.GetAllAsyncs(query => query
                    //                       .Where(c => c.POITypeId == c.POITypes.POITypeId)
                    //                       .Include(type => type.POITypes));
                    var result = new
                    {
                        DailyTour = new
                        {
                            dailyTour.DailyTourId,
                            dailyTour.PackageTourId,
                            dailyTour.DailyTourName,
                            dailyTour.Description,
                            dailyTour.DailyTourPrice,
                            dailyTour.ImgUrl,
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
                                    tt.DailyTicketPrice,
                                    tt.CreateDate
                                }).ToList()
                        },

                        PackageTour = new
                        {
                            dailyTour.PackageTours?.PackageTourId,
                            dailyTour.PackageTours?.PackageTourName,
                            dailyTour.PackageTours?.PackageTourPrice,
                            dailyTour.PackageTours?.PackageTourImgUrl,
                            StatusPackageTour = dailyTour.PackageTours?.Status,
                            dailyTour.PackageTours?.Cities?.CityName,
                            TourSegments = dailyTour.PackageTours?.TourSegments
                                .Where(ts => ts.Status == 1)
                                .Select(ts => new
                                {
                                    ts.TourSegmentId,
                                    ts.DestinationId,
                                    ts.Destinations?.DestinationName,
                                    StatusDestinations = ts.Destinations?.Status,
                                    Locations = ts.Destinations?.Locations
                                        .Where(c => c.Status == 1 &&
                                                    c.DestinationId == ts.DestinationId &&
                                                    dailyTour.PackageTours?.PackageTourId == ts.PackageTourId &&
                                                    ts.ServiceByTourSegments.Any(sbts => sbts.Services?.LocationId == c.LocationId))
                                        .Select(lo => new
                                        {
                                            lo.LocationId,
                                            lo.LocationName,
                                            lo.LocationImgUrl,
                                            //lo.LocationType,
                                            lo.DestinationId,
                                            //PointOfInterests = pointOfI.Where(poi => poi.LocationId == lo.LocationId).Select(type => new
                                            //{
                                            //    type.PointId,
                                            //    type.PointName,
                                            //    type.POITypes?.POITypeId,
                                            //    type.POITypes?.POITypeName,
                                            //}),
                                            StatusLocation = lo.Status,
                                            Services = ts.ServiceByTourSegments
                                                .Where(sbts => sbts.Services?.LocationId == lo.LocationId
                                                               && sbts.Services?.Status == 1)
                                                .Select(se => new {
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

                    resultList.Add(result);
                }

                return new APIResponseModel
                {
                    Message = "Found",
                    IsSuccess = true,
                    Data = resultList
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


    }
}   
