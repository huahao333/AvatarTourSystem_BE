using AutoMapper;
using BusinessObjects.Enums;
using BusinessObjects.Models;
using BusinessObjects.ViewModels.POI;
using BusinessObjects.ViewModels.POIType;
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
    public class POITypeService : IPOITypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public POITypeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<APIResponseModel> CreatePOIType(POITypeCreateModel poiTypeCreateModel)
        {
            var poiType = _mapper.Map<POIType>(poiTypeCreateModel);
            poiType.POITypeId = Guid.NewGuid().ToString();
            poiType.CreateDate = DateTime.Now;
            await _unitOfWork.POITypeRepository.AddAsync(poiType);
            _unitOfWork.Save();
            return new APIResponseModel
            {
                Message = "Create Point of Interest Type Successfully",
                IsSuccess = true,
                Data = poiType,
            };
        }

        public async Task<APIResponseModel> DeletePOIType(string id)
        {

            var poiType = await _unitOfWork.POITypeRepository.GetByIdStringAsync(id);

            if (poiType == null)
            {
                return new APIResponseModel
                {
                    Message = "Point of Interest Type not found.",
                    IsSuccess = false,
                    Data = null
                };
            }

            if (poiType.Status == (int?)EStatus.IsDeleted)
            {
                return new APIResponseModel
                {
                    Message = "Point of Interest Type has already been deleted.",
                    IsSuccess = false,
                    Data = null
                };
            }
            var createDate = poiType.CreateDate;
            poiType.CreateDate = createDate;
            poiType.Status = (int?)EStatus.IsDeleted;
            poiType.UpdateDate = DateTime.Now;

            var result = await _unitOfWork.POITypeRepository.UpdateAsync(poiType);
            _unitOfWork.Save();

            return new APIResponseModel
            {
                Message = "Delete Point of Interest Type Successfully",
                IsSuccess = true,
                Data = result,
            };
        }


        public async Task<APIResponseModel> GetAllPOITypes()
        {
            var poiTypeList = await _unitOfWork.POITypeRepository.GetAllAsync();
            var count = poiTypeList.Count();
            return new APIResponseModel
            {
                Message = $" Found {count} Point of Interest Types ",
                IsSuccess = true,
                Data = poiTypeList,
            };
        }

        public async Task<APIResponseModel> GetPOITypeById(string id)
        {
            var poiType = await _unitOfWork.POITypeRepository.GetByIdStringAsync(id);
            return new APIResponseModel
            {
                Message = "Get Point of Interest Type Successfully",
                IsSuccess = true,
                Data = poiType,
            };

        }

        public async Task<APIResponseModel> GetPOITypesByStatus()
        {
            var poiTypeList = await _unitOfWork.POITypeRepository.GetByConditionAsync(s => s.Status != -1);
            var count = poiTypeList.Count();
            return new APIResponseModel
            {
                Message = $" Found {count} Point of Interest Types ",
                IsSuccess = true,
                Data = poiTypeList,
            };
        }

        public async Task<APIResponseModel> UpdatePOIType(POITypeUpdateModel poiTypeUpdateModel)
        {
            var existingPOIType = await _unitOfWork.POITypeRepository.GetByIdGuidAsync(poiTypeUpdateModel.POITypeId);

            if (existingPOIType == null)
            {
                return new APIResponseModel
                {
                    Message = "Point of Interest Type not found",
                    IsSuccess = false
                };
            }
            var createDate = existingPOIType.CreateDate;
            var poiType = _mapper.Map(poiTypeUpdateModel, existingPOIType);
            poiType.CreateDate = createDate;
            poiType.UpdateDate = DateTime.Now;
            var result = await _unitOfWork.POITypeRepository.UpdateAsync(poiType);
            _unitOfWork.Save();
            return new APIResponseModel
            {
                Message = "Update Point of Interest Type Successfully",
                IsSuccess = true,
                Data = result,
            };
        }
    }

}
