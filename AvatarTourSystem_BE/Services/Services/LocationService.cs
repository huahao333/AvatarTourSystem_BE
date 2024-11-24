using AutoMapper;
using BusinessObjects.Enums;
using BusinessObjects.Models;
using BusinessObjects.ViewModels.Location;
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
    public class LocationService : ILocationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly GoogleMapsService _googleMapsService;
        public LocationService(IUnitOfWork unitOfWork, IMapper mapper, GoogleMapsService googleMapsService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _googleMapsService = googleMapsService;
        }

        public async Task<APIResponseModel> GetLocationsAsync()
        {
            var list = await _unitOfWork.LocationRepository.GetAllAsync();
            var count = list.Count();
            return new APIResponseModel
            {
                Message = $" Found {count} Location ",
                IsSuccess = true,
                Data = list,
            };
        }
        public async Task<APIResponseModel> GetActiveLocationsAsync()
        {
            var list = await _unitOfWork.LocationRepository.GetByConditionAsync(s => s.Status != -1);
            var count = list.Count();
            return new APIResponseModel
            {
                Message = $" Found {count} Location ",
                IsSuccess = true,
                Data = list,
            };
        }
        public async Task<APIResponseModel> GetLocationByIdAsync(string locationId)
        {
            var location = await _unitOfWork.LocationRepository.GetByIdStringAsync(locationId);
            if (location == null)
            {
                return new APIResponseModel
                {
                    Message = "Location not found",
                    IsSuccess = false
                };
            }
            return new APIResponseModel
            {
                Message = "Location found",
                IsSuccess = true,
                Data = location,
            };
        }

        public async Task<APIResponseModel> CreateLocationAsync(LocationCreateModel createModel)
        {
            //var location = _mapper.Map<Location>(createModel);
            //location.LocationId = Guid.NewGuid().ToString();
            //location.CreateDate = DateTime.Now;
            //var result = await _unitOfWork.LocationRepository.AddAsync(location);
            //_unitOfWork.Save();
            //return new APIResponseModel
            //{
            //    Message = " Location Created Successfully",
            //    IsSuccess = true,
            //    Data = location,
            //};
            if (!string.IsNullOrEmpty(createModel.LocationAddress))
            {
                var embedCode = await _googleMapsService.GetEmbedCodesAsync(createModel.LocationAddress);

                var locationGGMap = embedCode;
                var location = new Location
                {
                    LocationId = Guid.NewGuid().ToString(),
                    LocationName = createModel.LocationName,
                    LocationGoogleMap = locationGGMap,
                    LocationImgUrl = createModel.LocationImgUrl,
                    LocationHotline = createModel.LocationHotline,
                    DestinationId = createModel.DestinationId,
                    LocationOpeningHours = createModel.LocationOpeningHours,
                    LocationClosingHours = createModel.LocationClosingHours,
                    Status = (int)EStatus.Active,
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now
                };
                await _unitOfWork.LocationRepository.AddAsync(location);
                _unitOfWork.Save();
                return new APIResponseModel
                {
                    Message = " Location Created Successfully",
                    IsSuccess = true,
                    Data = location,
                };
            }
           return new APIResponseModel
           {
               Message = "Location Google Map is required",
               IsSuccess = false
           };


        }
        public async Task<APIResponseModel> UpdateLocationAsync(LocationUpdateModel updateModel)
        {
            var existinglocation = await _unitOfWork.LocationRepository.GetByIdGuidAsync(updateModel.LocationId);

            if (existinglocation == null)
            {
                return new APIResponseModel
                {
                    Message = "Location not found",
                    IsSuccess = false
                };
            }
            var createDate = existinglocation.CreateDate;

            var location = _mapper.Map(updateModel, existinglocation);
            location.CreateDate = createDate;
            location.UpdateDate = DateTime.Now;

            var result = await _unitOfWork.LocationRepository.UpdateAsync(location);
            _unitOfWork.Save();

            return new APIResponseModel
            {
                Message = "Location Updated Successfully",
                IsSuccess = true,
                Data = location,
            };
        }
        public async Task<APIResponseModel> DeleteLocation(string locationId)
        {
            var location = await _unitOfWork.LocationRepository.GetByIdStringAsync(locationId);
            if (location == null)
            {
                return new APIResponseModel
                {
                    Message = "Location not found",
                    IsSuccess = false
                };
            }
            if (location.Status == -1)
            {
                return new APIResponseModel
                {
                    Message = "Location has been removed",
                    IsSuccess = false
                };
            }
            var createDate = location.CreateDate;
            location.Status = (int?)EStatus.IsDeleted;
            location.CreateDate = createDate;
            location.UpdateDate = DateTime.Now;
            var result = await _unitOfWork.LocationRepository.UpdateAsync(location);
            _unitOfWork.Save();
            return new APIResponseModel
            {
                Message = " Location Deleted Successfully",
                IsSuccess = true,
                Data = location,
            };
        }
    }
}
