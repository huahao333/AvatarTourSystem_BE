using AutoMapper;
using BusinessObjects.Enums;
using BusinessObjects.Models;
using BusinessObjects.ViewModels.Service;
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
    public class ServiceService : IServiceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ServiceService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<APIResponseModel> GetServicesAsync()
        {
            var list = await _unitOfWork.ServiceRepository.GetAllAsync();
            var count = list.Count();
            return new APIResponseModel
            {
                Message = $" Found {count} Service ",
                IsSuccess = true,
                Data = list,
            };
        }
        public async Task<APIResponseModel> GetActiveServicesAsync()
        {
            var list = await _unitOfWork.ServiceRepository.GetByConditionAsync(s => s.Status != -1);
            var count = list.Count();
            return new APIResponseModel
            {
                Message = $" Found {count} Service ",
                IsSuccess = true,
                Data = list,
            };
        }
        public async Task<APIResponseModel> GetServiceByIdAsync(string SUBTId)
        {
            var service = await _unitOfWork.ServiceRepository.GetByIdStringAsync(SUBTId);
            if (service == null)
            {
                return new APIResponseModel
                {
                    Message = "Service not found",
                    IsSuccess = false
                };
            }
            return new APIResponseModel
            {
                Message = " Service found",
                IsSuccess = true,
                Data = service,
            };
            //  return _mapper.Map<ServiceModel>(service);
        }

        public async Task<APIResponseModel> CreateServiceAsync(ServiceCreateModel createModel)
        {
            var service = _mapper.Map<Service>(createModel);
            service.ServiceId = Guid.NewGuid().ToString();
            service.CreateDate = DateTime.Now;
            var result = await _unitOfWork.ServiceRepository.AddAsync(service);
            _unitOfWork.Save();
            return new APIResponseModel
            {
                Message = " Service Created Successfully",
                IsSuccess = true,
                Data = service,
            };
        }
        public async Task<APIResponseModel> UpdateServiceAsync(ServiceUpdateModel updateModel)
        {
            var existingService = await _unitOfWork.ServiceRepository.GetByIdGuidAsync(updateModel.ServiceId);

            if (existingService == null)
            {
                return new APIResponseModel
                {
                    Message = "Service not found",
                    IsSuccess = false
                };
            }
            var createDate = existingService.CreateDate;

            var service = _mapper.Map(updateModel, existingService);
            service.CreateDate = createDate;
            service.UpdateDate = DateTime.Now;

            var result = await _unitOfWork.ServiceRepository.UpdateAsync(service);
            _unitOfWork.Save();

            return new APIResponseModel
            {
                Message = "Service Updated Successfully",
                IsSuccess = true,
                Data = service,
            };
        }
        public async Task<APIResponseModel> DeleteService(string SUBTId)
        {
            var service = await _unitOfWork.ServiceRepository.GetByIdStringAsync(SUBTId);
            if (service == null)
            {
                return new APIResponseModel
                {
                    Message = "Service not found",
                    IsSuccess = false
                };
            }
            if (service.Status == -1)
            {
                return new APIResponseModel
                {
                    Message = "Service has been removed",
                    IsSuccess = false
                };
            }
            var createDate = service.CreateDate;
            service.Status = (int?)EStatus.IsDeleted;
            service.CreateDate = createDate;
            service.UpdateDate = DateTime.Now;
            var result = await _unitOfWork.ServiceRepository.UpdateAsync(service);
            _unitOfWork.Save();
            return new APIResponseModel
            {
                Message = " Service Deleted Successfully",
                IsSuccess = true,
                Data = service,
            };
        }
    }
}
