using AutoMapper;
using BusinessObjects.Enums;
using BusinessObjects.Models;
using BusinessObjects.ViewModels.POI;
using Repositories.Interfaces;
using Services.Common;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZXing;

namespace Services.Services
{
    public class PointOfInterestService : IPointOfInterestService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public PointOfInterestService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<APIResponseModel> CreatePointOfInterest(POICreateModel poiCreateModel)
        {
            var pointOfInterest = _mapper.Map<PointOfInterest>(poiCreateModel);
            pointOfInterest.PointId = Guid.NewGuid().ToString();
            pointOfInterest.CreateDate = DateTime.Now;
            await _unitOfWork.PointOfInterestRepository.AddAsync(pointOfInterest);
            _unitOfWork.Save();
            return new APIResponseModel
            {
                Message = "Create Point Of Interest Successfully",
                IsSuccess = true,
                Data = pointOfInterest,
            };
        }

        public async Task<APIResponseModel> DeletePointOfInterest(string id)
        {

            var pointOfInterest = await _unitOfWork.PointOfInterestRepository.GetByIdStringAsync(id);

            if (pointOfInterest == null)
            {
                return new APIResponseModel
                {
                    Message = "Point Of Interest not found.",
                    IsSuccess = false,
                    Data = null
                };
            }

            if (pointOfInterest.Status == (int?)EStatus.IsDeleted)
            {
                return new APIResponseModel
                {
                    Message = "Point Of Interest has already been deleted.",
                    IsSuccess = false,
                    Data = null
                };
            }
            var createDate = pointOfInterest.CreateDate;
            pointOfInterest.CreateDate = createDate;
            pointOfInterest.Status = (int?)EStatus.IsDeleted;
            pointOfInterest.UpdateDate = DateTime.Now;

            var result = await _unitOfWork.PointOfInterestRepository.UpdateAsync(pointOfInterest);
            _unitOfWork.Save();

            return new APIResponseModel
            {
                Message = "Delete Point Of Interest Successfully",
                IsSuccess = true,
                Data = result,
            };
        }


        public async Task<APIResponseModel> GetAllPointOfInterests()
        {
            var pointOfInterestList = await _unitOfWork.PointOfInterestRepository.GetAllAsync();
            var count = pointOfInterestList.Count();
            return new APIResponseModel
            {
                Message = $" Found {count} Point Of Interests ",
                IsSuccess = true,
                Data = pointOfInterestList,
            };
        }

        public async Task<APIResponseModel> GetPointOfInterestById(string id)
        {
            var pointOfInterest = await _unitOfWork.PointOfInterestRepository.GetByIdStringAsync(id);
            return new APIResponseModel
            {
                Message = "Get PointOfInterest Successfully",
                IsSuccess = true,
                Data = pointOfInterest,
            };

        }

        public async Task<APIResponseModel> GetPointOfInterestsByStatus()
        {
            var pointOfInterestList = await _unitOfWork.PointOfInterestRepository.GetByConditionAsync(s => s.Status != -1);
            var count = pointOfInterestList.Count();
            return new APIResponseModel
            {
                Message = $" Found {count} Point Of Interests ",
                IsSuccess = true,
                Data = pointOfInterestList,
            };
        }

        public async Task<APIResponseModel> UpdatePointOfInterest(POIUpdateModel poiUpdateModel)
        {
            var existingPointOfInterest = await _unitOfWork.PointOfInterestRepository.GetByIdGuidAsync(poiUpdateModel.PointId);

            if (existingPointOfInterest == null)
            {
                return new APIResponseModel
                {
                    Message = "Point Of Interest not found",
                    IsSuccess = false
                };
            }
            var createDate = existingPointOfInterest.CreateDate;
            var pointOfInterest = _mapper.Map(poiUpdateModel, existingPointOfInterest);
            pointOfInterest.CreateDate = createDate;
            pointOfInterest.UpdateDate = DateTime.Now;
            var result = await _unitOfWork.PointOfInterestRepository.UpdateAsync(pointOfInterest);
            _unitOfWork.Save();
            return new APIResponseModel
            {
                Message = "Update Point Of Interest Successfully",
                IsSuccess = true,
                Data = result,
            };
        }

        public async Task<APIResponseModel> CreatePointOfInterestByLocation(POICreateByLocationViewModel pOICreateByLocation)
        {
            try
            {
                var location = await _unitOfWork.PointOfInterestRepository.GetFirstOrDefaultAsync(query=>query.Where(a=>a.LocationId == pOICreateByLocation.LocationId));
                if (location == null)
                {
                    var poi = new PointOfInterest
                    {
                        PointId = Guid.NewGuid().ToString(),
                        PointName = "",
                        LocationId = pOICreateByLocation.LocationId,
                        Status =1,
                        CreateDate = DateTime.Now,
                    };
                    await _unitOfWork.PointOfInterestRepository.AddAsync(poi);
                    _unitOfWork.Save();
                    return new APIResponseModel
                    {
                        Message = "Update POI success",
                        IsSuccess = true
                    };
                }
                int statusPOI = 0;
                if(pOICreateByLocation.StatusPOI == true)
                {
                    statusPOI = 1;
                }else if(pOICreateByLocation.StatusPOI == false)
                {
                    statusPOI = -1;
                }

                location.Status = statusPOI;
                await _unitOfWork.PointOfInterestRepository.UpdateAsync(location);
                _unitOfWork.Save();
                return new APIResponseModel
                {
                    Message = "Update POI successfully",
                    IsSuccess = true
                };

            }
            catch (Exception ex)
            {
                return new APIResponseModel
                {
                    Message = "Error create POI",
                    IsSuccess = false
                };
            }
        }
    }
}
