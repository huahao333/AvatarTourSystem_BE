using AutoMapper;
using BusinessObjects.Models;
using BusinessObjects.ViewModels.DailyTour;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;
using Services.Common;
using Services.Interfaces;
using Services.RealTime;
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
        private readonly GoogleMapsService _googleMapsService;
        public DailyTourFlowService(IUnitOfWork unitOfWork, IMapper mapper, GoogleMapsService googleMapsService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _googleMapsService = googleMapsService;
            //  _hubContext = hubContext;
        }

        //public async Task<APIResponseModel> CreateDailyTourFlow(DailyTourFlowModel dailyTourFlowModel)
        //{
        //    try
        //    {
        //        var packageTourIdExisting = await _unitOfWork.PackageTourRepository.GetByIdStringAsync(dailyTourFlowModel.PackageTourId);
        //        if (packageTourIdExisting == null || string.IsNullOrEmpty(dailyTourFlowModel.PackageTourId))
        //        {
        //            return new APIResponseModel
        //            {
        //                Message = "PackageTourId must exist.",
        //                IsSuccess = false,
        //            };
        //        }

        //        if (dailyTourFlowModel.DailyTourPrice <= 0)
        //        {
        //            return new APIResponseModel
        //            {
        //                Message = "DailyTourPrice must be greater than 0.",
        //                IsSuccess = false,
        //            };
        //        }


        //        if (dailyTourFlowModel.StartDate < DateTime.Now)
        //        {
        //            return new APIResponseModel
        //            {
        //                Message = "StartDate must be greater than or equal to the current date.",
        //                IsSuccess = false,
        //            };
        //        }

        //        if (dailyTourFlowModel.EndDate < DateTime.Now || dailyTourFlowModel.EndDate < dailyTourFlowModel.StartDate)
        //        {
        //            return new APIResponseModel
        //            {
        //                Message = "EndDate must be greater than or equal to the current date and greater than StartDate.",
        //                IsSuccess = false,
        //            };
        //        }

        //        if (dailyTourFlowModel.Discount < 0)
        //        {
        //            return new APIResponseModel
        //            {
        //                Message = "Discount must be greater than or equal to 0.",
        //                IsSuccess = false,
        //            };
        //        }

        //        if (string.IsNullOrEmpty(dailyTourFlowModel.TicketTypeIdAdult?.Trim()))
        //        {
        //            return new APIResponseModel
        //            {
        //                Message = "TicketTypeId must exist.",
        //                IsSuccess = false,
        //            };
        //        }

        //        if (dailyTourFlowModel.CapacityByAdult < 0 || dailyTourFlowModel.CapacityByChildren<0)
        //        {
        //            return new APIResponseModel
        //            {
        //                Message = "CapacityByAdult or CapacityByChildren must be greater than or equal to 0.",
        //                IsSuccess = false,
        //            };
        //        }

        //        if (dailyTourFlowModel.PriceByAdult < 0 || dailyTourFlowModel.PriceByChildren < 0)
        //        {
        //            return new APIResponseModel
        //            {
        //                Message = "PriceByAdult or PriceByChildren must be greater than or equal to 0.",
        //                IsSuccess = false,
        //            };
        //        }



        //        var ticketTypeAdultExisting = await _unitOfWork.TicketTypeRepository.GetByIdStringAsync(dailyTourFlowModel.TicketTypeIdAdult);

        //        if (ticketTypeAdultExisting == null)
        //        {
        //            return new APIResponseModel
        //            {
        //                Message = "TicketTypeIdAdult must exist.",
        //                IsSuccess = false,
        //            };
        //        }

        //        TicketType ticketTypeChildrenExisting = null;
        //        if (!string.IsNullOrEmpty(dailyTourFlowModel.TicketTypeIdChildren?.Trim()))
        //        {
        //            ticketTypeChildrenExisting = await _unitOfWork.TicketTypeRepository.GetByIdStringAsync(dailyTourFlowModel.TicketTypeIdChildren);
        //            if (ticketTypeChildrenExisting == null)
        //            {
        //                return new APIResponseModel
        //                {
        //                    Message = "TicketTypeIdChildren must exist.",
        //                    IsSuccess = false,
        //                };
        //            }
        //        }

        //        var newDailyTourId = Guid.NewGuid();
        //        var dailyTour = new DailyTour
        //        {
        //            DailyTourId = newDailyTourId.ToString(), 
        //            PackageTourId = dailyTourFlowModel.PackageTourId,
        //            DailyTourName = dailyTourFlowModel.DailyTourName,
        //            Description = dailyTourFlowModel.Description,
        //            DailyTourPrice = dailyTourFlowModel.DailyTourPrice,
        //            ImgUrl = dailyTourFlowModel.ImgUrl,
        //            StartDate = dailyTourFlowModel.StartDate,
        //            EndDate = dailyTourFlowModel.EndDate,
        //            Discount = dailyTourFlowModel.Discount,
        //            Status = 1, 
        //            CreateDate = DateTime.Now
        //        };


        //        //var createdDailyTour = await _unitOfWork.DailyTourRepository.AddAsync(dailyTour);
        //     //   _unitOfWork.Save();


        //        var dailyTicketAdult = new DailyTicketType
        //        {
        //            DailyTicketId = Guid.NewGuid().ToString(), 
        //            DailyTourId = dailyTour.DailyTourId, 
        //            TicketTypeId = dailyTourFlowModel.TicketTypeIdAdult, 
        //            Capacity = dailyTourFlowModel.CapacityByAdult, 
        //            DailyTicketPrice = dailyTourFlowModel.PriceByAdult,
        //            Status = 1, 
        //            CreateDate = DateTime.Now
        //        };

        //        DailyTicketType dailyTicketChildren = null;
        //        if (!string.IsNullOrEmpty(dailyTourFlowModel.TicketTypeIdChildren?.Trim()))
        //        {
        //            dailyTicketChildren = new DailyTicketType
        //            {
        //                DailyTicketId = Guid.NewGuid().ToString(),
        //                DailyTourId = dailyTour.DailyTourId,
        //                TicketTypeId = dailyTourFlowModel.TicketTypeIdChildren,
        //                Capacity = dailyTourFlowModel.CapacityByChildren,
        //                DailyTicketPrice = dailyTourFlowModel.PriceByChildren,
        //                Status = 1,
        //                CreateDate = DateTime.Now
        //            };
        //        }
        //        await _unitOfWork.DailyTourRepository.AddAsync(dailyTour);
        //        await _unitOfWork.DailyTicketRepository.AddAsync(dailyTicketAdult);

        //        if (dailyTicketChildren != null)
        //        {
        //            await _unitOfWork.DailyTicketRepository.AddAsync(dailyTicketChildren);
        //        }
        //        _unitOfWork.Save();

        //        return new APIResponseModel
        //        {
        //            Message = "DailyTour and Tickets created successfully",
        //            IsSuccess = true,
        //            //Data = new
        //            //{
        //            //    DailyTour = dailyTour,
        //            //    DailyTicketAdult = dailyTicketAdult,
        //            //    DailyTicketChildren = dailyTicketChildren
        //            //}
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

        public async Task<APIResponseModel> CreateDailyTourFlow(DailyToursFlowModel dailyTourFlowModel)
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

                if (string.IsNullOrEmpty(dailyTourFlowModel.Description))
                {
                    return new APIResponseModel
                    {
                        Message = "Description cannot be left blank.",
                        IsSuccess = false,
                    };
                }

                if (string.IsNullOrEmpty(dailyTourFlowModel.DailyTourName))
                {
                    return new APIResponseModel
                    {
                        Message = "DailyTourName cannot be left blank.",
                        IsSuccess = false,
                    };
                }

                if (dailyTourFlowModel.DailyTourName.Length < 4 || dailyTourFlowModel.DailyTourName.Length > 20)
                {
                    return new APIResponseModel
                    {
                        Message = "DailyTourName must be between 5 and 20 characters.",
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

                if (dailyTourFlowModel.StartDate.Value < DateTime.UtcNow.Date)
                {
                    return new APIResponseModel
                    {
                        Message = "StartDate must be greater than to the current date.",
                        IsSuccess = false,
                    };
                }

                if (dailyTourFlowModel.EndDate <= DateTime.Now.Date || dailyTourFlowModel.EndDate <= dailyTourFlowModel.StartDate)
                {
                    return new APIResponseModel
                    {
                        Message = "EndDate must be greater than to the current date and greater than StartDate.",
                        IsSuccess = false,
                    };
                }


                if (dailyTourFlowModel.DailyTicketTypes == null || !dailyTourFlowModel.DailyTicketTypes.Any())
                {
                    return new APIResponseModel
                    {
                        Message = "At least one DailyTicketType must be provided.",
                        IsSuccess = false,
                    };
                }

                if (dailyTourFlowModel.ExpirationDate <= dailyTourFlowModel.StartDate || dailyTourFlowModel.ExpirationDate <= dailyTourFlowModel.EndDate)
                {
                    return new APIResponseModel
                    {
                        Message = "ExpirationDate must be greater than both StartDate and EndDate.",
                        IsSuccess = false,
                    };
                }


                if (dailyTourFlowModel.Discount < 0 && dailyTourFlowModel.Discount>100)
                {
                    return new APIResponseModel
                    {
                        Message = "Discount must be greater than or equal to 0.",
                        IsSuccess = false,
                    };
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
                    ExpirationDate = dailyTourFlowModel.ExpirationDate,
                    StartDate = dailyTourFlowModel.StartDate,
                    EndDate = dailyTourFlowModel.EndDate,
                    Discount = dailyTourFlowModel.Discount,
                    Status = 1,
                    CreateDate = DateTime.Now
                };

                await _unitOfWork.DailyTourRepository.AddAsync(dailyTour);

                foreach (var ticketType in dailyTourFlowModel.DailyTicketTypes)
                {
                    if (string.IsNullOrEmpty(ticketType.TicketTypeId))
                    {
                        return new APIResponseModel
                        {
                            Message = "TicketTypeId in DailyTicketTypes must exist.",
                            IsSuccess = false,
                        };
                    }

                    var ticketTypeExisting = await _unitOfWork.TicketTypeRepository.GetByIdStringAsync(ticketType.TicketTypeId);
                    if (ticketTypeExisting == null)
                    {
                        return new APIResponseModel
                        {
                            Message = $"TicketTypeId {ticketType.TicketTypeId} does not exist.",
                            IsSuccess = false,
                        };
                    }

                    if (ticketType.Capacity < 0 || ticketType.Price < 0)
                    {
                        return new APIResponseModel
                        {
                            Message = "Capacity and Price in DailyTicketTypes must be greater than or equal to 0.",
                            IsSuccess = false,
                        };
                    }

                    var dailyTicketType = new DailyTicketType
                    {
                        DailyTicketId = Guid.NewGuid().ToString(),
                        DailyTourId = dailyTour.DailyTourId,
                        TicketTypeId = ticketType.TicketTypeId,
                        Capacity = ticketType.Capacity ?? 0,
                        DailyTicketPrice = ticketType.Price ?? 0,
                        Status = 1,
                        CreateDate = DateTime.Now
                    };

                    await _unitOfWork.DailyTicketRepository.AddAsync(dailyTicketType);
                }

                _unitOfWork.Save();
          //      await _hubContext.Clients.All.SendAsync("ReceiveNewDailyTour", dailyTour.DailyTourId, dailyTour.DailyTourName);
                return new APIResponseModel
                {
                    Message = "DailyTour and Tickets created successfully",
                    IsSuccess = true
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

        public async Task<APIResponseModel> UpdateDailyTourFlow(UpdateDailyTourFlowModel updateModel)
        {
            try
            {
                // Check if the DailyTour exists
                var existingDailyTour = await _unitOfWork.DailyTourRepository.GetByIdStringAsync(updateModel.DailyTourId);
                if (existingDailyTour == null)
                {
                    return new APIResponseModel
                    {
                        Message = "DailyTour not found.",
                        IsSuccess = false,
                    };
                }

                // Check PackageTourId
                if (!string.IsNullOrEmpty(updateModel.PackageTourId))
                {
                    var packageTourExists = await _unitOfWork.PackageTourRepository.GetByIdStringAsync(updateModel.PackageTourId);
                    if (packageTourExists == null)
                    {
                        return new APIResponseModel
                        {
                            Message = "PackageTourId does not exist.",
                            IsSuccess = false,
                        };
                    }
                }

                // Validate other fields
                if (updateModel.DailyTourPrice <= 0)
                {
                    return new APIResponseModel
                    {
                        Message = "DailyTourPrice must be greater than 0.",
                        IsSuccess = false,
                    };
                }

                //if (updateModel.StartDate < DateTime.Now)
                //{
                //    return new APIResponseModel
                //    {
                //        Message = "StartDate must be greater than or equal to the current date.",
                //        IsSuccess = false,
                //    };
                //}

                //if (updateModel.EndDate < DateTime.Now || updateModel.EndDate < updateModel.StartDate)
                //{
                //    return new APIResponseModel
                //    {
                //        Message = "EndDate must be greater than or equal to the current date and greater than StartDate.",
                //        IsSuccess = false,
                //    };
                //}

                //if (updateModel.ExpirationDate <= updateModel.StartDate || updateModel.ExpirationDate <= updateModel.EndDate)
                //{
                //    return new APIResponseModel
                //    {
                //        Message = "ExpirationDate must be greater than both StartDate and EndDate.",
                //        IsSuccess = false,
                //    };
                //}
                // Validate StartDate if provided
                if (updateModel.StartDate.HasValue)
                {
                    if (updateModel.StartDate.Value < DateTime.Now.Date && updateModel.StartDate.Value != existingDailyTour.StartDate)
                    {
                        return new APIResponseModel
                        {
                            Message = "StartDate must be greater than to the current date.",
                            IsSuccess = false,
                        };
                    }
                }

                // Validate EndDate if provided
                if (updateModel.EndDate.HasValue)
                {
                    if (updateModel.EndDate.Value <= DateTime.Now.Date && updateModel.EndDate.Value != existingDailyTour.EndDate)
                    {
                        return new APIResponseModel
                        {
                            Message = "EndDate must be greater than to the current date.",
                            IsSuccess = false,
                        };
                    }

                    if (updateModel.StartDate.HasValue && updateModel.EndDate.Value <= updateModel.StartDate.Value)
                    {
                        return new APIResponseModel
                        {
                            Message = "EndDate must be greater than to StartDate.",
                            IsSuccess = false,
                        };
                    }
                    else if (!updateModel.StartDate.HasValue && updateModel.EndDate.Value <= existingDailyTour.StartDate)
                    {
                        return new APIResponseModel
                        {
                            Message = "EndDate must be greater than to the existing StartDate.",
                            IsSuccess = false,
                        };
                    }
                }

                // Validate ExpirationDate if provided
                if (updateModel.ExpirationDate.HasValue)
                {
                    if (updateModel.ExpirationDate.Value <= (updateModel.StartDate ?? existingDailyTour.StartDate))
                    {
                        return new APIResponseModel
                        {
                            Message = "ExpirationDate must be greater than StartDate.",
                            IsSuccess = false,
                        };
                    }

                    if (updateModel.ExpirationDate.Value <= (updateModel.EndDate ?? existingDailyTour.EndDate))
                    {
                        return new APIResponseModel
                        {
                            Message = "ExpirationDate must be greater than EndDate.",
                            IsSuccess = false,
                        };
                    }
                }

                if (updateModel.Discount < 0 && updateModel.Discount >100)
                {
                    return new APIResponseModel
                    {
                        Message = "Discount must be greater than or equal to 0.",
                        IsSuccess = false,
                    };
                }

                // Update DailyTour fields
                existingDailyTour.PackageTourId = updateModel.PackageTourId ?? existingDailyTour.PackageTourId;
                existingDailyTour.DailyTourName = updateModel.DailyTourName ?? existingDailyTour.DailyTourName;
                existingDailyTour.Description = updateModel.Description ?? existingDailyTour.Description;
                existingDailyTour.DailyTourPrice = updateModel.DailyTourPrice ?? existingDailyTour.DailyTourPrice;
                existingDailyTour.ImgUrl = updateModel.ImgUrl ?? existingDailyTour.ImgUrl;
                existingDailyTour.ExpirationDate = updateModel.ExpirationDate ?? existingDailyTour.ExpirationDate;
                existingDailyTour.StartDate = updateModel.StartDate ?? existingDailyTour.StartDate;
                existingDailyTour.EndDate = updateModel.EndDate ?? existingDailyTour.EndDate;
                existingDailyTour.Discount = updateModel.Discount ?? existingDailyTour.Discount;
                existingDailyTour.UpdateDate = DateTime.Now;

                // Update DailyTicketTypes if provided
                if (updateModel.DailyTicketTypes != null && updateModel.DailyTicketTypes.Any())
                {
                    foreach (var ticketType in updateModel.DailyTicketTypes)
                    {
                        if (string.IsNullOrEmpty(ticketType.TicketTypeId))
                        {
                            return new APIResponseModel
                            {
                                Message = "TicketTypeId in DailyTicketTypes must exist.",
                                IsSuccess = false,
                            };
                        }

                        var ticketTypeExists = await _unitOfWork.DailyTicketRepository.GetByIdStringAsync(ticketType.TicketTypeId);
                        if (ticketTypeExists == null)
                        {
                            return new APIResponseModel
                            {
                                Message = $"DailyTicketId {ticketType.TicketTypeId} does not exist.",
                                IsSuccess = false,
                            };
                        }

                        if (ticketType.Capacity < 0 || ticketType.Price < 0)
                        {
                            return new APIResponseModel
                            {
                                Message = "Capacity and Price in DailyTicketTypes must be greater than or equal to 0.",
                                IsSuccess = false,
                            };
                        }

                        var existingTicket = (await _unitOfWork.DailyTicketRepository.GetByConditionAsync(x =>
                                             x.DailyTourId == updateModel.DailyTourId && x.DailyTicketId == ticketType.TicketTypeId))
                                             .FirstOrDefault();

                        if (existingTicket != null)
                        {
                            // Update existing ticket
                            existingTicket.Capacity = ticketType.Capacity ?? existingTicket.Capacity;
                            existingTicket.DailyTicketPrice = ticketType.Price ?? existingTicket.DailyTicketPrice;
                            existingTicket.UpdateDate = DateTime.Now;
                        }
                        else
                        {
                            // Add new ticket
                            //var newDailyTicket = new DailyTicketType
                            //{
                            //    DailyTicketId = Guid.NewGuid().ToString(),
                            //    DailyTourId = updateModel.DailyTourId,
                            //    TicketTypeId = ticketType.TicketTypeId,
                            //    Capacity = ticketType.Capacity ?? 0,
                            //    DailyTicketPrice = ticketType.Price ?? 0,
                            //    Status = 1,
                            //    CreateDate = DateTime.Now
                            //};

                            //await _unitOfWork.DailyTicketRepository.AddAsync(newDailyTicket);
                        }
                    }
                }

                // Save changes
                _unitOfWork.Save();

                return new APIResponseModel
                {
                    Message = "DailyTour updated successfully.",
                    IsSuccess = true
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
                    .Include(dt => dt.PackageTours )
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
                var pointOfI = await _unitOfWork.PointOfInterestRepository.GetAllAsyncs(query => query);

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
                      //  dailyTour.PackageTours?.PackageTourPrice,
                        dailyTour.PackageTours?.PackageTourImgUrl,
                        dailyTour.PackageTours?.CityId,
                        StatusPackageTour = dailyTour.PackageTours?.Status,
                        dailyTour.PackageTours?.Cities?.CityName,
                        TourSegments = dailyTour.PackageTours?.TourSegments
                            .Where(ts => ts.Status == 1 && ts.Destinations?.Status==1)
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
                                                                                      sbts.Status==1))
                                    .Select(lo => new
                                    {
                                        lo.LocationId,
                                        lo.LocationName,
                                        lo.LocationImgUrl,
                                        lo.LocationHotline,
                                        lo.LocationOpeningHours,
                                        lo.LocationClosingHours,
                                        lo.LocationGoogleMap,
                                        //lo.LocationType,
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


        public async Task<APIResponseModel> GetAllDailyToursForUser()
        {
            try
            {
                var toDay = DateTime.Now.Date;
                var dailyTours = await _unitOfWork.DailyTourRepository.GetAllAsyncs(query => query
                   .Where(dt => dt.Status == 1 && dt.StartDate.Value.Date <=toDay  && dt.EndDate.Value.Date>=toDay )
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
                    var pointOfI = await _unitOfWork.PointOfInterestRepository.GetAllAsyncs(query => query);
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
                            dailyTour.ExpirationDate,
                            dailyTour.StartDate,
                            dailyTour.EndDate,
                            dailyTour.Discount,
                            IsSoldOut = dailyTour.DailyTickets.All(tt => tt.Capacity <= 0),
                            HasDiscount = dailyTour.Discount > 0,
                            IsHotPOI = dailyTour.PackageTours?.TourSegments
                                                   .Any(ts => ts.Destinations.Locations
                                                   .Any(lo => lo.PointOfInterests
                                                   .Any(poi => poi.Status == 1))),
                            StatusDailyTour = dailyTour.Status,
                            TicketTypes = dailyTour.DailyTickets
                                .Where(c => c.DailyTourId == dailyTour.DailyTourId
                                           && c.TicketTypes?.PackageTourId == dailyTour.PackageTourId
                                           && c.Capacity > 0 || c.TicketTypes.TicketTypeName == "Vé người lớn")
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
                            // dailyTour.PackageTours?.PackageTourPrice,
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
                                        .Select( lo => new
                                        {
                                            lo.LocationId,
                                            lo.LocationName,
                                            lo.LocationImgUrl,
                                            lo.LocationHotline,
                                            lo.LocationGoogleMap,
                                            lo.LocationOpeningHours,
                                            lo.LocationClosingHours,
                                            //lo.LocationType,
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

        public async Task<APIResponseModel> GetAllDailysTours()
        {
            try
            {
                var dailyTours = await _unitOfWork.DailyTourRepository.GetAllAsyncs(query => query.OrderBy(s=> s.Status==1)
               //    .Where(dt=>dt.Status==1)
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
                    var bookingCount = await _unitOfWork.BookingRepository.CountAsync(b => b.DailyTourId == dailyTour.DailyTourId);
                    var pointOfI = await _unitOfWork.PointOfInterestRepository.GetAllAsyncs(query => query);
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
                            dailyTour.ExpirationDate,
                            dailyTour.StartDate,
                            dailyTour.EndDate,
                            dailyTour.Discount,
                            StatusDailyTour = dailyTour.Status,
                            BookingCount = bookingCount,
                            TicketTypes = dailyTour.DailyTickets
                                .Where(c => c.DailyTourId == dailyTour.DailyTourId
                                           && c.TicketTypes?.PackageTourId==dailyTour.PackageTourId
                                           && c.Capacity>0)
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
                           // dailyTour.PackageTours?.PackageTourPrice,
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
                                        .Where(c=>  c.Status==1 &&
                                                    c.DestinationId == ts.DestinationId &&
                                                    dailyTour.PackageTours?.PackageTourId ==ts.PackageTourId &&
                                                    ts.ServiceByTourSegments.Any(sbts=> sbts.Services?.LocationId==c.LocationId &&
                                                                                        sbts.Status == 1))
                                        .Select(lo => new
                                        {
                                            lo.LocationId,
                                            lo.LocationName,
                                            lo.LocationImgUrl,
                                            lo.LocationHotline,
                                            lo.LocationGoogleMap,
                                            lo.LocationOpeningHours,
                                            lo.LocationClosingHours,
                                            //lo.LocationType,
                                            lo.DestinationId,
                                            PointOfInterests = pointOfI.Where(poi => poi.LocationId == lo.LocationId).Select(type => new
                                            {
                                                type.PointId,
                                                type.PointName,
                                            }),
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
                var toDay = DateTime.Now.Date;
                // Query all DailyTours with related entities in one query
                var dailyTours = await _unitOfWork.DailyTourRepository.GetAllAsyncs(query => query
                   .Where(dt => dt.Status == 1 && dt.Discount>0 && dt.StartDate.Value.Date <= toDay && dt.EndDate.Value.Date >= toDay)
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
                    var pointOfI = await _unitOfWork.PointOfInterestRepository.GetAllAsyncs(query => query);

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
                            dailyTour.ExpirationDate,
                            dailyTour.StartDate,
                            dailyTour.EndDate,
                            dailyTour.Discount,
                            IsSoldOut = dailyTour.DailyTickets.All(tt => tt.Capacity <= 0),
                            HasDiscount = dailyTour.Discount > 0,
                            IsHotPOI = dailyTour.PackageTours?.TourSegments
                                                   .Any(ts => ts.Destinations.Locations
                                                   .Any(lo => lo.PointOfInterests
                                                   .Any(poi => poi.Status == 1))),
                            StatusDailyTour = dailyTour.Status,
                            TicketTypes = dailyTour.DailyTickets
                                .Where(c => c.DailyTourId == dailyTour.DailyTourId
                                           && c.TicketTypes?.PackageTourId == dailyTour.PackageTourId
                                           && c.Capacity > 0 || c.TicketTypes.TicketTypeName == "Vé người lớn")
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
                         //   dailyTour.PackageTours?.PackageTourPrice,
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
                                            lo.LocationGoogleMap,
                                            lo.LocationOpeningHours,
                                            lo.LocationClosingHours,
                                            //lo.LocationType,
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
                var toDay = DateTime.Now.Date;
                // Query all DailyTours with related entities in one query
                var dailyTours = await _unitOfWork.DailyTourRepository.GetAllAsyncs(query => query
                   .Where(dt =>dt.Status ==1 && dt.StartDate.Value.Date<=toDay && dt.EndDate.Value.Date >= toDay && dt.PackageTours.TourSegments.Any(c=>c.Destinations.Locations.Any(loca=>loca.PointOfInterests.Any(poi=>poi.LocationId==loca.LocationId && poi.Status==1))))
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
                    var pointOfI = await _unitOfWork.PointOfInterestRepository.GetAllAsyncs(query => query);
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
                            dailyTour.ExpirationDate,
                            dailyTour.StartDate,
                            dailyTour.EndDate,
                            dailyTour.Discount,
                            IsSoldOut = dailyTour.DailyTickets.All(tt => tt.Capacity <= 0),
                            HasDiscount = dailyTour.Discount > 0,
                            IsHotPOI = dailyTour.PackageTours?.TourSegments
                                                   .Any(ts => ts.Destinations.Locations
                                                   .Any(lo => lo.PointOfInterests
                                                   .Any(poi => poi.Status == 1))),
                            StatusDailyTour = dailyTour.Status,
                            TicketTypes = dailyTour.DailyTickets
                                .Where(c => c.DailyTourId == dailyTour.DailyTourId
                                           && c.TicketTypes?.PackageTourId == dailyTour.PackageTourId
                                           && c.Capacity > 0 || c.TicketTypes.TicketTypeName=="Vé người lớn")
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
                         //   dailyTour.PackageTours?.PackageTourPrice,
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
                                            lo.LocationGoogleMap,
                                            lo.LocationOpeningHours,
                                            lo.LocationClosingHours,
                                            //lo.LocationType,
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

        public async Task<APIResponseModel> UpdateStatusDailyTour(UpdateStatusDailyTourViewModel updateStatusDailyTourViewModel)
        {
            try
            {
                var dailyTour = await _unitOfWork.DailyTourRepository.GetFirstOrDefaultAsync(query=> query.Where(d=>d.DailyTourId==updateStatusDailyTourViewModel.DailyTourId));
                if (dailyTour == null)
                {
                    return new APIResponseModel
                    {
                        Message = "DailyTour not Found",
                        IsSuccess = false
                    };
                }
                var packageTour = await _unitOfWork.PackageTourRepository.GetFirstOrDefaultAsync(query => query.Where(p => p.PackageTourId == dailyTour.PackageTourId));
                if (packageTour.Status == -1 || packageTour.Status ==0)
                {
                    return new APIResponseModel
                    {
                        Message = "DailyTour cannot be updated because the package this DailyTour uses has been removed or disabled.",
                        IsSuccess = false
                    };
                }

                dailyTour.Status = updateStatusDailyTourViewModel.Status;
                dailyTour.UpdateDate = DateTime.Now;
                await _unitOfWork.DailyTourRepository.UpdateAsync(dailyTour);

                var dailyTickets = await _unitOfWork.DailyTicketRepository.GetAllAsyncs(query => query.Where(dt=>dt.DailyTourId==dailyTour.DailyTourId));
                foreach (var dailyTicket in dailyTickets)
                {
                    dailyTicket.Status = updateStatusDailyTourViewModel.Status;
                    dailyTicket.UpdateDate = DateTime.Now;
                    await _unitOfWork.DailyTicketRepository.UpdateAsync(dailyTicket);
                }
                _unitOfWork.Save();

                return new APIResponseModel
                {
                    Message = "Status updated successfully for DailyTour.",
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

        public async Task<APIResponseModel> UpdateStatusPackageTour(UpdateStatusPackageTourViewModel updateStatusPackageTourViewModel)
        {
            try
            {
                var packageTour = await _unitOfWork.PackageTourRepository.GetFirstOrDefaultAsync(query => query.Where(p=>p.PackageTourId==updateStatusPackageTourViewModel.PackageTourId));
                if (packageTour == null)
                {
                    return new APIResponseModel
                    {
                        Message = "Package not Found",
                        IsSuccess = false
                    };
                }
                packageTour.Status = updateStatusPackageTourViewModel.Status;
                packageTour.UpdateDate = DateTime.Now;
                await _unitOfWork.PackageTourRepository.UpdateAsync(packageTour);

                var ticketTypes = await _unitOfWork.TicketTypeRepository.GetAllAsyncs(query => query.Where(tt=>tt.PackageTourId == packageTour.PackageTourId));
                foreach ( var ticketType in ticketTypes)
                {
                    ticketType.Status = updateStatusPackageTourViewModel.Status;
                    ticketType.UpdateDate = DateTime.Now;
                    await _unitOfWork.TicketTypeRepository.UpdateAsync(ticketType);
                }

                var dailyTours = await _unitOfWork.DailyTourRepository.GetAllAsyncs(query => query.Where(dt => dt.PackageTourId == packageTour.PackageTourId));
                foreach (var dailyTour in dailyTours)
                {
                    dailyTour.Status = updateStatusPackageTourViewModel.Status;
                    dailyTour.UpdateDate = DateTime.Now;
                    await _unitOfWork.DailyTourRepository.UpdateAsync(dailyTour);

                    var dailyTickets = await _unitOfWork.DailyTicketRepository.GetAllAsyncs(query => query.Where(dt => dt.DailyTourId == dailyTour.DailyTourId));
                    foreach (var dailyTicket in dailyTickets)
                    {
                        dailyTicket.Status = updateStatusPackageTourViewModel.Status;
                        dailyTicket.UpdateDate = DateTime.Now;
                        await _unitOfWork.DailyTicketRepository.UpdateAsync(dailyTicket);
                    }

                }

                _unitOfWork.Save();

                return new APIResponseModel
                {
                    Message = "Status updated successfully for PackageTour.",
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
    }
}   
