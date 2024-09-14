using AutoMapper;
using BusinessObjects.Enums;
using BusinessObjects.Models;
using BusinessObjects.ViewModels.City;
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
    public class CityService : ICityService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CityService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<APIResponseModel> GetActiveCitiesAsync()
        {
            var list = await _unitOfWork.CityRepository.GetByConditionAsync(s => s.Status != -1);
            var count = list.Count();
            return new APIResponseModel
            {
                Message = $" Found {count} Cities ",
                IsSuccess = true,
                Data = list,
            };
        }
        public async Task<APIResponseModel> GetCitiesAsync()
        {
            var list = await _unitOfWork.CityRepository.GetAllAsync();
            var count = list.Count();
            return new APIResponseModel
            {
                Message = $" Found {count} Cities ",
                IsSuccess = true,
                Data = list,
            };
        }
        public async Task<CityModel> GetCityByIdAsync(string CityId)
        {
            var city = await _unitOfWork.CityRepository.GetByIdStringAsync(CityId);
            return _mapper.Map<CityModel>(city);
        }
        public async Task<APIResponseModel> CreateCityAsync(CityCreateModel createModel)
        {
            var city = _mapper.Map<City>(createModel);
            city.CityId = Guid.NewGuid().ToString();
            city.CreateDate = DateTime.Now;
            await _unitOfWork.CityRepository.AddAsync(city);
            _unitOfWork.Save();
            return new APIResponseModel
            {
                Message = " City Created Successfully",
                IsSuccess = true,
                Data = city,
            };
        }
        public async Task<APIResponseModel> UpdateCityAsync(CityUpdateModel updateModel)
        {
            var existingcity = await _unitOfWork.CityRepository.GetByIdGuidAsync(updateModel.CityId);

            if (existingcity == null)
            {
                return new APIResponseModel
                {
                    Message = "city not found",
                    IsSuccess = false
                };
            }
            var createDate = existingcity.CreateDate;

            var city = _mapper.Map(updateModel, existingcity);
            city.CreateDate = createDate;
            city.UpdateDate = DateTime.Now;

            var result = await _unitOfWork.CityRepository.UpdateAsync(city);
            _unitOfWork.Save();

            return new APIResponseModel
            {
                Message = "city Updated Successfully",
                IsSuccess = true,
                Data = city,
            };
        }
        public async Task<APIResponseModel> DeleteCity(string cityId)
        {
            var city = await _unitOfWork.CityRepository.GetByIdStringAsync(cityId);
            city.Status = (int?)EStatus.IsDeleted;
            var result = await _unitOfWork.CityRepository.UpdateAsync(city);
            _unitOfWork.Save();
            return new APIResponseModel
            {
                Message = " city Deleted Successfully",
                IsSuccess = true,
                Data = city,
            };
        }
    }
}
