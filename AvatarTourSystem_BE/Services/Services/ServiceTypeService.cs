using AutoMapper;
using BusinessObjects.Enums;
using BusinessObjects.Models;
using BusinessObjects.ViewModels.ServiceType;
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
    public class ServiceTypeService : IServiceTypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ServiceTypeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<APIResponseModel> GetServiceTypesAsync()
        {
            var list = await _unitOfWork.ServiceTypeRepository.GetAllAsync();
            var count = list.Count();
            return new APIResponseModel
            {
                Message = $" Found {count} Service Types ",
                IsSuccess = true,
                Data = list,
            };
        }
        public async Task<APIResponseModel> GetActiveServiceTypesAsync()
        {
            var list = await _unitOfWork.ServiceTypeRepository.GetByConditionAsync(s => s.Status != -1);
            var count = list.Count();
            return new APIResponseModel
            {
                Message = $" Found {count} Service Types ",
                IsSuccess = true,
                Data = list,
            };
        }
        public async Task<APIResponseModel> GetServiceTypeByIdAsync(string SUBTId)
        {
            var serviceTypes = await _unitOfWork.ServiceTypeRepository.GetByIdStringAsync(SUBTId);
            if (serviceTypes == null)
            {
                return new APIResponseModel
                {
                    Message = "Service Type not found",
                    IsSuccess = false
                };
            }
            return new APIResponseModel
            {
                Message = " Service Type found",
                IsSuccess = true,
                Data = serviceTypes,
            };
            //  return _mapper.Map<ServiceTypesModel>(serviceTypes);
        }

        public async Task<APIResponseModel> CreateServiceTypeAsync(ServiceTypeCreateModel createModel)
        {
            var serviceTypes = _mapper.Map<ServiceType>(createModel);
            serviceTypes.ServiceTypeId= Guid.NewGuid().ToString();
            serviceTypes.CreateDate = DateTime.Now;
            var result = await _unitOfWork.ServiceTypeRepository.AddAsync(serviceTypes);
            _unitOfWork.Save();
            return new APIResponseModel
            {
                Message = " Service Type Created Successfully",
                IsSuccess = true,
                Data = serviceTypes,
            };
        }
        public async Task<APIResponseModel> UpdateServiceTypeAsync(ServiceTypeUpdateModel updateModel)
        {
            var existingServiceTypes = await _unitOfWork.ServiceTypeRepository.GetByIdGuidAsync(updateModel.ServiceTypeId);

            if (existingServiceTypes == null)
            {
                return new APIResponseModel
                {
                    Message = "Service Type not found",
                    IsSuccess = false
                };
            }
            var createDate = existingServiceTypes.CreateDate;

            var serviceTypes = _mapper.Map(updateModel, existingServiceTypes);
            serviceTypes.CreateDate = createDate;
            serviceTypes.UpdateDate = DateTime.Now;

            var result = await _unitOfWork.ServiceTypeRepository.UpdateAsync(serviceTypes);
            _unitOfWork.Save();

            return new APIResponseModel
            {
                Message = "Service Type Updated Successfully",
                IsSuccess = true,
                Data = serviceTypes,
            };
        }
        public async Task<APIResponseModel> DeleteServiceType(string SUBTId)
        {
            var serviceTypes = await _unitOfWork.ServiceTypeRepository.GetByIdStringAsync(SUBTId);
            if (serviceTypes == null)
            {
                return new APIResponseModel
                {
                    Message = "Service Type not found",
                    IsSuccess = false
                };
            }
            if (serviceTypes.Status == -1)
            {
                return new APIResponseModel
                {
                    Message = "Service Type has been removed",
                    IsSuccess = false
                };
            }
            var createDate = serviceTypes.CreateDate;
            serviceTypes.Status = (int?)EStatus.IsDeleted;
            serviceTypes.CreateDate = createDate;
            serviceTypes.UpdateDate = DateTime.Now;
            var result = await _unitOfWork.ServiceTypeRepository.UpdateAsync(serviceTypes);
            _unitOfWork.Save();
            return new APIResponseModel
            {
                Message = " Service Type Deleted Successfully",
                IsSuccess = true,
                Data = serviceTypes,
            };
        }
    }
}
