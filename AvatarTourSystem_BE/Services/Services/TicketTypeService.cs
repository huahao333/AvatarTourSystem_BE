using AutoMapper;
using BusinessObjects.Enums;
using BusinessObjects.Models;
using BusinessObjects.ViewModels.PackageTour;
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
    public class TicketTypeService: ITicketTypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public TicketTypeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<APIResponseModel> GetTicketTypesAsync()
        {
            var list = await _unitOfWork.TicketTypeRepository.GetAllAsync();
            var count = list.Count();
            return new APIResponseModel
            {
                Message = $" Found {count} TicketType ",
                IsSuccess = true,
                Data = list,
            };
        }
        public async Task<APIResponseModel> GetActiveTicketTypesAsync()
        {
            var list = await _unitOfWork.TicketTypeRepository.GetByConditionAsync(s => s.Status != -1);
            var count = list.Count();
            return new APIResponseModel
            {
                Message = $" Found {count} TicketType ",
                IsSuccess = true,
                Data = list,
            };
        }
        public async Task<APIResponseModel> GetTicketTypeByIdAsync(string TicketTypeId)
        {
            var ticketType = await _unitOfWork.TicketTypeRepository.GetByIdStringAsync(TicketTypeId);
            if (ticketType == null)
            {
                return new APIResponseModel
                {
                    Message = "TicketType not found",
                    IsSuccess = false
                };
            }
            return new APIResponseModel
            {
                Message = " TicketType found",
                IsSuccess = true,
                Data = ticketType,
            };
            //return _mapper.Map<TicketTypeModel>(ticketType);
        }

        public async Task<APIResponseModel> CreateTicketTypeAsync(TicketTypeCreateModel createModel)
        {
            var ticketType = _mapper.Map<TicketType>(createModel);
            ticketType.TicketTypeId = Guid.NewGuid().ToString();
            ticketType.CreateDate = DateTime.Now;
            ticketType.MinBuyTicket = createModel.MinBuyTicket;
            var result = await _unitOfWork.TicketTypeRepository.AddAsync(ticketType);
            _unitOfWork.Save();
            return new APIResponseModel
            {
                Message = " TicketType Created Successfully",
                IsSuccess = true,
                Data = ticketType,
            };
        }
        public async Task<APIResponseModel> UpdateTicketTypeAsync(TicketTypeUpdateModel updateModel)
        {
            var existingTicketType = await _unitOfWork.TicketTypeRepository.GetByIdGuidAsync(updateModel.TicketTypeId);

            if (existingTicketType == null)
            {
                return new APIResponseModel
                {
                    Message = "TicketType not found",
                    IsSuccess = false
                };
            }
            var createDate = existingTicketType.CreateDate;

            var ticketType = _mapper.Map(updateModel, existingTicketType);
            ticketType.CreateDate = createDate;
            ticketType.UpdateDate = DateTime.Now;

            var result = await _unitOfWork.TicketTypeRepository.UpdateAsync(ticketType);
            _unitOfWork.Save();

            return new APIResponseModel
            {
                Message = "TicketType Updated Successfully",
                IsSuccess = true,
                Data = ticketType,
            };
        }
        public async Task<APIResponseModel> DeleteTicketType(string TicketTypeId)
        {
            var ticketType = await _unitOfWork.TicketTypeRepository.GetByIdStringAsync(TicketTypeId);
            if (ticketType == null)
            {
                return new APIResponseModel
                {
                    Message = "TicketType not found",
                    IsSuccess = false
                };
            }
            if (ticketType.Status == -1)
            {
                return new APIResponseModel
                {
                    Message = "TicketType has been removed",
                    IsSuccess = false
                };
            }
            var createDate = ticketType.CreateDate;
            ticketType.Status = (int?)EStatus.IsDeleted;
            ticketType.CreateDate = createDate;
            ticketType.UpdateDate = DateTime.Now;
            var result = await _unitOfWork.TicketTypeRepository.UpdateAsync(ticketType);
            _unitOfWork.Save();
            return new APIResponseModel
            {
                Message = " TicketType Deleted Successfully",
                IsSuccess = true,
                Data = ticketType,
            };
        }
    }
}
