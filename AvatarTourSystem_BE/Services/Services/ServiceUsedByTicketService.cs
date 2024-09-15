using AutoMapper;
using BusinessObjects.Enums;
using BusinessObjects.Models;
using BusinessObjects.ViewModels.DailyTicket;
using BusinessObjects.ViewModels.ServiceUsedByTicket;
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
    public class ServiceUsedByTicketService : IServiceUsedByTicketService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ServiceUsedByTicketService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<APIResponseModel> GetServiceUsedByTicketsAsync()
        {
            var list = await _unitOfWork.ServiceUsedByTicketRepository.GetAllAsync();
            var count = list.Count();
            return new APIResponseModel
            {
                Message = $" Found {count} ServiceUsedByTicket ",
                IsSuccess = true,
                Data = list,
            };
        }
        public async Task<APIResponseModel> GetActiveServiceUsedByTicketsAsync()
        {
            var list = await _unitOfWork.ServiceUsedByTicketRepository.GetByConditionAsync(s => s.Status != -1);
            var count = list.Count();
            return new APIResponseModel
            {
                Message = $" Found {count} ServiceUsedByTicket ",
                IsSuccess = true,
                Data = list,
            };
        }
        public async Task<ServiceUsedByTicketModel> GetServiceUsedByTicketByIdAsync(string SUBTId)
        {
            var serviceUsedByTicket = await _unitOfWork.ServiceUsedByTicketRepository.GetByIdStringAsync(SUBTId);
            return _mapper.Map<ServiceUsedByTicketModel>(serviceUsedByTicket);
        }

        public async Task<APIResponseModel> CreateServiceUsedByTicketAsync(ServiceUsedByTicketCreateModel createModel)
        {
            var serviceUsedByTicket = _mapper.Map<ServiceUsedByTicket>(createModel);
            serviceUsedByTicket.SUBTId = Guid.NewGuid().ToString();
            serviceUsedByTicket.CreateDate = DateTime.Now;
            var result = await _unitOfWork.ServiceUsedByTicketRepository.AddAsync(serviceUsedByTicket);
            _unitOfWork.Save();
            return new APIResponseModel
            {
                Message = " ServiceUsedByTicket Created Successfully",
                IsSuccess = true,
                Data = serviceUsedByTicket,
            };
        }
        public async Task<APIResponseModel> UpdateServiceUsedByTicketAsync(ServiceUsedByTicketUpdateModel updateModel)
        {
            var existingServiceUsedByTicket = await _unitOfWork.ServiceUsedByTicketRepository.GetByIdGuidAsync(updateModel.SUBTId);

            if (existingServiceUsedByTicket == null)
            {
                return new APIResponseModel
                {
                    Message = "ServiceUsedByTicket not found",
                    IsSuccess = false
                };
            }
            var createDate = existingServiceUsedByTicket.CreateDate;

            var serviceUsedByTicket = _mapper.Map(updateModel, existingServiceUsedByTicket);
            serviceUsedByTicket.CreateDate = createDate;
            serviceUsedByTicket.UpdateDate = DateTime.Now;

            var result = await _unitOfWork.ServiceUsedByTicketRepository.UpdateAsync(serviceUsedByTicket);
            _unitOfWork.Save();

            return new APIResponseModel
            {
                Message = "ServiceUsedByTicket Updated Successfully",
                IsSuccess = true,
                Data = serviceUsedByTicket,
            };
        }
        public async Task<APIResponseModel> DeleteServiceUsedByTicket(string SUBTId)
        {
            var serviceUsedByTicket = await _unitOfWork.ServiceUsedByTicketRepository.GetByIdStringAsync(SUBTId);
            if (serviceUsedByTicket == null)
            {
                return new APIResponseModel
                {
                    Message = "ServiceUsedByTicket not found",
                    IsSuccess = false
                };
            } 
            if(serviceUsedByTicket.Status == -1)
            {
                return new APIResponseModel
                {
                    Message = "ServiceUsedByTicket has been removed",
                    IsSuccess = false
                };
            }
            var createDate = serviceUsedByTicket.CreateDate;
            serviceUsedByTicket.Status = (int?)EStatus.IsDeleted;
            serviceUsedByTicket.CreateDate = createDate;
            serviceUsedByTicket.UpdateDate = DateTime.Now;
            var result = await _unitOfWork.ServiceUsedByTicketRepository.UpdateAsync(serviceUsedByTicket);
            _unitOfWork.Save();
            return new APIResponseModel
            {
                Message = " ServiceUsedByTicket Deleted Successfully",
                IsSuccess = true,
                Data = serviceUsedByTicket,
            };
        }
    }
}
