using AutoMapper;
using BusinessObjects.Models;
using BusinessObjects.ViewModels.DailyTour;
using Microsoft.AspNetCore.Mvc;
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

               
                var createdDailyTour = await _unitOfWork.DailyTourRepository.AddAsync(dailyTour);
                _unitOfWork.Save();

                
                var dailyTicketAdult = new DailyTicket
                {
                    DailyTicketId = Guid.NewGuid().ToString(), 
                    DailyTourId = createdDailyTour.DailyTourId, 
                    TicketTypeId = dailyTourFlowModel.TicketTypeIdAdult, 
                    Capacity = dailyTourFlowModel.CapacityByAdult, 
                    DailyTicketPrice = dailyTourFlowModel.PriceByAdult,
                    Status = 1, 
                    CreateDate = DateTime.Now
                };

                var dailyTicketChildren = new DailyTicket
                {
                    DailyTicketId = Guid.NewGuid().ToString(), 
                    DailyTourId = createdDailyTour.DailyTourId, 
                    TicketTypeId = dailyTourFlowModel.TicketTypeIdChildren, 
                    Capacity = dailyTourFlowModel.CapacityByChildren, 
                    DailyTicketPrice = dailyTourFlowModel.PriceByChildren, 
                    Status = 1, 
                    CreateDate = DateTime.Now
                };
                await _unitOfWork.DailyTicketRepository.AddAsync(dailyTicketAdult);
                await _unitOfWork.DailyTicketRepository.AddAsync(dailyTicketChildren);

                _unitOfWork.Save();

                return new APIResponseModel
                {
                    Message = "DailyTour and Tickets created successfully",
                    IsSuccess = true,
                    Data = new
                    {
                        DailyTour = createdDailyTour,
                        DailyTicketAdult = dailyTicketAdult,
                        DailyTicketChildren = dailyTicketChildren
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

        public async Task<APIResponseModel> GetDailyTourDetails(string dailyTourId)
        {
            try
            {
                var dailyTour = await _unitOfWork.DailyTourRepository.GetByIdStringAsync(dailyTourId);

                if (dailyTour == null)
                {
                    return new APIResponseModel
                    {
                        Message = "Not Found",
                        IsSuccess = false,
                    };
                }

                var packageTour = await _unitOfWork.PackageTourRepository.GetByIdStringAsync(dailyTour.PackageTourId);

                var tourSegments = await _unitOfWork.TourSegmentRepository.GetByConditionAsync(ts => ts.PackageTourId == packageTour.PackageTourId);

                var ticketTypes = await _unitOfWork.TicketTypeRepository.GetByConditionAsync(t => t.PackageTourId == packageTour.PackageTourId);

                var destinationIds = tourSegments.Select(ts => ts.DestinationId).Distinct();
                var destinations = await _unitOfWork.DestinationRepository.GetByConditionAsync(d => destinationIds.Contains(d.DestinationId));

                var tourSegmentIds = tourSegments.Select(ts => ts.TourSegmentId).Distinct();
                var serviceByTourSegments = await _unitOfWork.ServiceByTourSegmentRepository.GetByConditionAsync(sbts => tourSegmentIds.Contains(sbts.TourSegmentId));

                var serviceIdsInTourSegments = serviceByTourSegments.Select(sbts => sbts.ServiceId).Distinct();
                var services = await _unitOfWork.ServiceRepository.GetByConditionAsync(s => serviceIdsInTourSegments.Contains(s.ServiceId));

                var locationIds = services.Select(s => s.LocationId).Distinct();
                var locations = await _unitOfWork.LocationRepository.GetByConditionAsync(l => locationIds.Contains(l.LocationId));

                var result = new
                {
                    DailyTour = new
                    {
                        dailyTour.DailyTourId,
                        dailyTour.PackageTourId,
                        dailyTour.DailyTourName,
                        dailyTour.Description,
                        dailyTour.DailyTourPrice,
                        dailyTour.StartDate,
                        dailyTour.EndDate,
                        dailyTour.Discount,
                        TicketTypes = ticketTypes.Select(tt => new
                        {
                            tt.TicketTypeId,
                            tt.TicketTypeName,
                            tt.CreateDate
                        }).ToList()
                    },
                    PackageTour = new
                    {
                        packageTour.PackageTourId,
                        packageTour.PackageTourName,
                        TourSegments = tourSegments.Select(ts => new
                        {
                            ts.TourSegmentId,
                            ts.DestinationId,
                            Destinations = destinations
                                .Where(d => d.DestinationId == ts.DestinationId)
                                .Select(d => new
                                {
                                    d.DestinationId,
                                    d.DestinationName,
                                    Locations = services
                                        .Where(s => serviceByTourSegments
                                            .Any(sbts => sbts.TourSegmentId == ts.TourSegmentId && sbts.ServiceId == s.ServiceId)) // Lọc Service theo TourSegment
                                        .Select(s => locations
                                            .Where(l => l.LocationId == s.LocationId)
                                            .Select(l => new
                                            {
                                                l.LocationId,
                                                l.LocationName,
                                                Services = services
                                                    .Where(svc => svc.LocationId == l.LocationId)
                                                    .Select(svc => new
                                                    {
                                                        svc.ServiceId,
                                                        svc.ServiceName
                                                    }).ToList()
                                            }).FirstOrDefault())
                                }).ToList()
                        }).ToList()
                    }
                };

                return new APIResponseModel
                {
                    Message = "found",
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

        public async Task<APIResponseModel> GetAllDailyTours()
        {
            try
            {
                // Retrieve all daily tours
                var dailyTours = await _unitOfWork.DailyTourRepository.GetAllAsync();

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
                    var packageTour = await _unitOfWork.PackageTourRepository.GetByIdStringAsync(dailyTour.PackageTourId);

                    var tourSegments = await _unitOfWork.TourSegmentRepository.GetByConditionAsync(ts => ts.PackageTourId == packageTour.PackageTourId);

                    var ticketTypes = await _unitOfWork.TicketTypeRepository.GetByConditionAsync(t => t.PackageTourId == packageTour.PackageTourId);

                    var destinationIds = tourSegments.Select(ts => ts.DestinationId).Distinct();
                    var destinations = await _unitOfWork.DestinationRepository.GetByConditionAsync(d => destinationIds.Contains(d.DestinationId));

                    var tourSegmentIds = tourSegments.Select(ts => ts.TourSegmentId).Distinct();
                    var serviceByTourSegments = await _unitOfWork.ServiceByTourSegmentRepository.GetByConditionAsync(sbts => tourSegmentIds.Contains(sbts.TourSegmentId));

                    var serviceIdsInTourSegments = serviceByTourSegments.Select(sbts => sbts.ServiceId).Distinct();
                    var services = await _unitOfWork.ServiceRepository.GetByConditionAsync(s => serviceIdsInTourSegments.Contains(s.ServiceId));

                    var locationIds = services.Select(s => s.LocationId).Distinct();
                    var locations = await _unitOfWork.LocationRepository.GetByConditionAsync(l => locationIds.Contains(l.LocationId));

                    var result = new
                    {
                        DailyTour = new
                        {
                            dailyTour.DailyTourId,
                            dailyTour.PackageTourId,
                            dailyTour.DailyTourName,
                            dailyTour.Description,
                            dailyTour.DailyTourPrice,
                            dailyTour.StartDate,
                            dailyTour.EndDate,
                            dailyTour.Discount,
                            TicketTypes = ticketTypes.Select(tt => new
                            {
                                tt.TicketTypeId,
                                tt.TicketTypeName,
                                tt.CreateDate
                            }).ToList()
                        },
                        PackageTour = new
                        {
                            packageTour.PackageTourId,
                            packageTour.PackageTourName,
                            TourSegments = tourSegments.Select(ts => new
                            {
                                ts.TourSegmentId,
                                ts.DestinationId,
                                Destinations = destinations
                                    .Where(d => d.DestinationId == ts.DestinationId)
                                    .Select(d => new
                                    {
                                        d.DestinationId,
                                        d.DestinationName,
                                        Locations = services
                                            .Where(s => serviceByTourSegments
                                                .Any(sbts => sbts.TourSegmentId == ts.TourSegmentId && sbts.ServiceId == s.ServiceId)) // Filter Services by TourSegment
                                            .Select(s => locations
                                                .Where(l => l.LocationId == s.LocationId)
                                                .Select(l => new
                                                {
                                                    l.LocationId,
                                                    l.LocationName,
                                                    Services = services
                                                        .Where(svc => svc.LocationId == l.LocationId)
                                                        .Select(svc => new
                                                        {
                                                            svc.ServiceId,
                                                            svc.ServiceName
                                                        }).ToList()
                                                }).FirstOrDefault())
                                    }).ToList()
                            }).ToList()
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
