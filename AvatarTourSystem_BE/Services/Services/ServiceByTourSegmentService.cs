using AutoMapper;
using BusinessObjects.Enums;
using BusinessObjects.Models;
using BusinessObjects.ViewModels.ServiceByTourSegment;
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
    public class ServiceByTourSegmentService : IServiceByTourSegmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ServiceByTourSegmentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<APIResponseModel> GetServiceByTourSegmentsAsync()
        {
            var list = await _unitOfWork.ServiceByTourSegmentRepository.GetAllAsync();
            var count = list.Count();
            return new APIResponseModel
            {
                Message = $" Found {count} ServiceByTourSegment ",
                IsSuccess = true,
                Data = list,
            };
        }
        public async Task<APIResponseModel> GetActiveServiceByTourSegmentsAsync()
        {
            var list = await _unitOfWork.ServiceByTourSegmentRepository.GetByConditionAsync(s => s.Status != -1);
            var count = list.Count();
            return new APIResponseModel
            {
                Message = $" Found {count} ServiceByTourSegment ",
                IsSuccess = true,
                Data = list,
            };
        }
        public async Task<APIResponseModel> GetServiceByTourSegmentByIdAsync(string SBTSId)
        {
            var serviceByTourSegment = await _unitOfWork.ServiceByTourSegmentRepository.GetByIdStringAsync(SBTSId);
            if (serviceByTourSegment == null)
            {
                return new APIResponseModel
                {
                    Message = "ServiceByTourSegment not found",
                    IsSuccess = false
                };
            }
            return new APIResponseModel
            {
                Message = " ServiceByTourSegment found",
                IsSuccess = true,
                Data = serviceByTourSegment,
            };
            //  return _mapper.Map<ServiceByTourSegmentModel>(serviceByTourSegment);
        }

        public async Task<APIResponseModel> CreateServiceByTourSegmentAsync(ServiceByTourSegmentCreateModel createModel)
        {
            var serviceByTourSegment = _mapper.Map<ServiceByTourSegment>(createModel);
            serviceByTourSegment.SBTSId = Guid.NewGuid().ToString();
            serviceByTourSegment.CreateDate = DateTime.Now;
            var result = await _unitOfWork.ServiceByTourSegmentRepository.AddAsync(serviceByTourSegment);
            _unitOfWork.Save();
            return new APIResponseModel
            {
                Message = " ServiceByTourSegment Created Successfully",
                IsSuccess = true,
                Data = serviceByTourSegment,
            };
        }
        public async Task<APIResponseModel> UpdateServiceByTourSegmentAsync(ServiceByTourSegmentUpdateModel updateModel)
        {
            var existingServiceByTourSegment = await _unitOfWork.ServiceByTourSegmentRepository.GetByIdGuidAsync(updateModel.SBTSId);

            if (existingServiceByTourSegment == null)
            {
                return new APIResponseModel
                {
                    Message = "ServiceByTourSegment not found",
                    IsSuccess = false
                };
            }
            var createDate = existingServiceByTourSegment.CreateDate;

            var serviceByTourSegment = _mapper.Map(updateModel, existingServiceByTourSegment);
            serviceByTourSegment.CreateDate = createDate;
            serviceByTourSegment.UpdateDate = DateTime.Now;

            var result = await _unitOfWork.ServiceByTourSegmentRepository.UpdateAsync(serviceByTourSegment);
            _unitOfWork.Save();

            return new APIResponseModel
            {
                Message = "ServiceByTourSegment Updated Successfully",
                IsSuccess = true,
                Data = serviceByTourSegment,
            };
        }
        public async Task<APIResponseModel> DeleteServiceByTourSegment(string SBTSId)
        {
            var serviceByTourSegment = await _unitOfWork.ServiceByTourSegmentRepository.GetByIdStringAsync(SBTSId);
            if (serviceByTourSegment == null)
            {
                return new APIResponseModel
                {
                    Message = "ServiceByTourSegment not found",
                    IsSuccess = false
                };
            }
            if (serviceByTourSegment.Status == -1)
            {
                return new APIResponseModel
                {
                    Message = "ServiceByTourSegment has been removed",
                    IsSuccess = false
                };
            }
            var createDate = serviceByTourSegment.CreateDate;
            serviceByTourSegment.Status = (int?)EStatus.IsDeleted;
            serviceByTourSegment.CreateDate = createDate;
            serviceByTourSegment.UpdateDate = DateTime.Now;
            var result = await _unitOfWork.ServiceByTourSegmentRepository.UpdateAsync(serviceByTourSegment);
            _unitOfWork.Save();
            return new APIResponseModel
            {
                Message = " ServiceByTourSegment Deleted Successfully",
                IsSuccess = true,
                Data = serviceByTourSegment,
            };
        }
    }
}
