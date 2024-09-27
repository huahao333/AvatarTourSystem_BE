using AutoMapper;
using BusinessObjects.Enums;
using BusinessObjects.Models;
using BusinessObjects.ViewModels.ResquestType;
using BusinessObjects.ViewModels.TransactionHistory;
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
    public class RequestTypeService : IRequestTypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public RequestTypeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<APIResponseModel> CreateRequestType(RequestTypeCreateModel requestTypeCreateModel)
        {
            var requestType = _mapper.Map<RequestType>(requestTypeCreateModel);
            requestType.RequestTypeId = Guid.NewGuid().ToString();
            requestType.CreateDate = DateTime.Now;
            await _unitOfWork.RequestTypeRepository.AddAsync(requestType);
            _unitOfWork.Save();
            return new APIResponseModel
            {
                Message = "Create Request Type Successfully",
                IsSuccess = true,
                Data = requestType,
            };

        }

        public async Task<APIResponseModel> DeleteRequestType(string id)
        {
            var requestType = await _unitOfWork.RequestTypeRepository.GetByIdStringAsync(id);
            if (requestType == null)
            {
                return new APIResponseModel
                {
                    Message = "Request Type not found.",
                    IsSuccess = false,
                    Data = null
                };
            }
            if (requestType.Status == (int?)EStatus.IsDeleted)
            {
                return new APIResponseModel
                {
                    Message = "Request Type has already been deleted.",
                    IsSuccess = false,
                    Data = null
                };
            }
            
            requestType.Status = (int?)EStatus.IsDeleted;
            requestType.UpdateDate = DateTime.Now;
            var result = await _unitOfWork.RequestTypeRepository.UpdateAsync(requestType);
            _unitOfWork.Save();
            return new APIResponseModel
            {
                Message = "Delete Request Type Successfully",
                IsSuccess = true,
                Data = result,
            };
        }

        public async Task<APIResponseModel> GetAllRequestType()
        {
            var requestTypes = await _unitOfWork.RequestTypeRepository.GetAllAsync();
            return new APIResponseModel
            {
                Message = "Get All Request Type Successfully",
                IsSuccess = true,
                Data = requestTypes,
            };

        }

        public async Task<APIResponseModel> GetRequestTypeById(string id)
        {
           var requestType = await _unitOfWork.RequestTypeRepository.GetByIdStringAsync(id);
            if (requestType == null)
            {
                return new APIResponseModel
                {
                    Message = "Request Type not found.",
                    IsSuccess = false,
                    Data = null
                };
            }
            var requestTypeViewModel = _mapper.Map<RequestTypeModel>(requestType);
            return new APIResponseModel
            {
                Message = "Get Request Type by Id Successfully",
                IsSuccess = true,
                Data = requestTypeViewModel,
            };
        }

        public async Task<APIResponseModel> GetRequestTypeByStatus()
        {
            var requestTypes = await _unitOfWork.RequestTypeRepository.GetByConditionAsync(x => x.Status != -1);
            return new APIResponseModel
            {
                Message = "Get Request Type by Status Successfully",
                IsSuccess = true,
                Data = requestTypes,
            };
        }

        public async Task<APIResponseModel> UpdateRequestType(RequestTypeUpdateModel requestTypeUpdateModel)
        {
            var requestType = await _unitOfWork.RequestTypeRepository.GetByIdGuidAsync(requestTypeUpdateModel. RequestTypeId);
            if (requestType == null)
            {
                return new APIResponseModel
                {
                    Message = "Request Type not found",
                    IsSuccess = false
                };
            }
            var createDate = requestType.CreateDate;
            requestType = _mapper.Map(requestTypeUpdateModel, requestType);
            requestType.CreateDate = createDate;
            requestType.UpdateDate = DateTime.Now;
            var result = await _unitOfWork.RequestTypeRepository.UpdateAsync(requestType);
            _unitOfWork.Save();
            return new APIResponseModel
            {
                Message = "Request Type Updated Successfully",
                IsSuccess = true,
                Data = requestType,
            };
        }
    }
}
