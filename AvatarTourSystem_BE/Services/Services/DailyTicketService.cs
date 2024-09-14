using AutoMapper;
using BusinessObjects.Enums;
using BusinessObjects.Models;
using BusinessObjects.ViewModels.DailyTicket;
using BusinessObjects.ViewModels.TicketType;
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
    public class DailyTicketService : IDailyTicketService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public DailyTicketService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<APIResponseModel> GetDailyTicketsAsync()
        {
            var list = await _unitOfWork.DailyTicketRepository.GetAllAsync();
            var count = list.Count();
            return new APIResponseModel
            {
                Message = $" Found {count} DailyTicket ",
                IsSuccess = true,
                Data = list,
            };
        }
        public async Task<APIResponseModel> GetActiveDailyTicketsAsync()
        {
            var list = await _unitOfWork.DailyTicketRepository.GetByConditionAsync(s => s.Status != -1);
            var count = list.Count();
            return new APIResponseModel
            {
                Message = $" Found {count} DailyTicket ",
                IsSuccess = true,
                Data = list,
            };
        }
        public async Task<DailyTicketModel> GetDailyTicketByIdAsync(string DailyTicketId)
        {
            var dailyTicket = await _unitOfWork.DailyTicketRepository.GetByIdStringAsync(DailyTicketId);
            return _mapper.Map<DailyTicketModel>(dailyTicket);
        }

        public async Task<APIResponseModel> CreateDailyTicketAsync(DailyTicketCreateModel createModel)
        {
            var dailyTicket = _mapper.Map<DailyTicket>(createModel);
            dailyTicket.DailyTicketId = Guid.NewGuid().ToString();
            dailyTicket.CreateDate = DateTime.Now;
            var result = await _unitOfWork.DailyTicketRepository.AddAsync(dailyTicket);
            _unitOfWork.Save();
            return new APIResponseModel
            {
                Message = " DailyTicket Created Successfully",
                IsSuccess = true,
                Data = dailyTicket,
            };
        }
        public async Task<APIResponseModel> UpdateDailyTicketAsync(DailyTicketUpdateModel updateModel)
        {
            var existingDailyTicket = await _unitOfWork.DailyTicketRepository.GetByIdGuidAsync(updateModel.DailyTicketId);

            if (existingDailyTicket == null)
            {
                return new APIResponseModel
                {
                    Message = "DailyTicket not found",
                    IsSuccess = false
                };
            }
            var createDate = existingDailyTicket.CreateDate;

            var dailyTicket = _mapper.Map(updateModel, existingDailyTicket);
            dailyTicket.CreateDate = createDate;
            dailyTicket.UpdateDate = DateTime.Now;

            var result = await _unitOfWork.DailyTicketRepository.UpdateAsync(dailyTicket);
            _unitOfWork.Save();

            return new APIResponseModel
            {
                Message = "DailyTicket Updated Successfully",
                IsSuccess = true,
                Data = dailyTicket,
            };
        }
        public async Task<APIResponseModel> DeleteDailyTicket(string DailyTicketId)
        {
            var dailyTicket = await _unitOfWork.DailyTicketRepository.GetByIdStringAsync(DailyTicketId);
            dailyTicket.Status = (int?)EStatus.IsDeleted;
            var result = await _unitOfWork.DailyTicketRepository.UpdateAsync(dailyTicket);
            _unitOfWork.Save();
            return new APIResponseModel
            {
                Message = " DailyTicket Deleted Successfully",
                IsSuccess = true,
                Data = dailyTicket,
            };
        }
    }
}
