using AutoMapper;
using BusinessObjects.Models;
using BusinessObjects.ViewModels.DailyTour;
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
                    Status = (int)dailyTourFlowModel.Status, 
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
                    Status = (int)dailyTourFlowModel.Status, 
                    CreateDate = DateTime.Now
                };

                var dailyTicketChildren = new DailyTicket
                {
                    DailyTicketId = Guid.NewGuid().ToString(), 
                    DailyTourId = createdDailyTour.DailyTourId, 
                    TicketTypeId = dailyTourFlowModel.TicketTypeIdChildren, 
                    Capacity = dailyTourFlowModel.CapacityByChildren, 
                    DailyTicketPrice = dailyTourFlowModel.PriceByChildren, 
                    Status = (int)dailyTourFlowModel.Status, 
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
    }
}   
