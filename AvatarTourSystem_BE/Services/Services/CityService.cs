﻿using AutoMapper;
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
        public async Task<APIResponseModel> GetCitiesForUser()
        {
            try
            {
                var today = DateTime.Now.Date;
                var activeCities = await _unitOfWork.CityRepository.GetAllAsyncs(query =>
                    query.Where(city => city.Status != -1 &&
                                        city.PackageTours.Any(pt =>
                                            pt.Status == 1 &&
                                            pt.DailyTours.Any(dt =>
                                                dt.Status == 1 &&
                                                dt.StartDate.Value.Date <= today &&
                                                dt.EndDate.Value.Date >= today)))
                );

                var result = activeCities.Select(city => new
                {
                    city.CityId,
                    city.CityName,
                    city.Status,
                    PackageTourCount = city.PackageTours.Count(pt =>
                        pt.Status == 1 &&
                        pt.DailyTours.Any(dt =>
                            dt.Status == 1 &&
                            dt.StartDate.Value.Date <= today &&
                            dt.EndDate.Value.Date >= today))
                }).ToList();

                if (!result.Any())
                {
                    return new APIResponseModel
                    {
                        Message = "No active cities found.",
                        IsSuccess = false,
                        Data = null
                    };
                }

                return new APIResponseModel
                {
                    Message = $"Found {result.Count} active cities.",
                    IsSuccess = true,
                    Data = result
                };
            }
            catch (Exception ex)
            {
                return new APIResponseModel
                {
                    Message = ex.Message,
                    IsSuccess = false
                };
            }
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
        public async Task<APIResponseModel> GetCityByIdAsync(string cityId)
        {
            var cities = await _unitOfWork.CityRepository.GetByConditionAsync(x => x.CityId == cityId);
            if (cities == null || !cities.Any())
            {
                return new APIResponseModel
                {
                    Message = "City not found.",
                    IsSuccess = false,
                    Data = null
                };
            }
            return new APIResponseModel
            {
                Message = "Get city by City Id Successfully",
                IsSuccess = true,
                Data = cities,
            };
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

        public async Task<APIResponseModel> GetCallbackCityAsync(object data)
        {
            string testCityId = "4c29b262-638d-46a3-ae01-e85070b61d5b";
            var existingcity = await _unitOfWork.CityRepository.GetByIdStringAsync(testCityId);

            if (existingcity == null)
            {
                return new APIResponseModel
                {
                    Message = "city not found",
                    IsSuccess = false
                };
            }
            var createDate = existingcity.CreateDate;
            string newCityName = data?.ToString() ?? "Default CityName";
            existingcity.CreateDate = createDate;
            existingcity.CityName = newCityName;
            existingcity.UpdateDate = DateTime.Now;
            existingcity.Status = 9;

            var result = await _unitOfWork.CityRepository.UpdateAsync(existingcity);
            _unitOfWork.Save();

            return new APIResponseModel
            {
                Message = "city Updated Successfully",
                IsSuccess = true,
                Data = existingcity,
            };
        }
    }
}
