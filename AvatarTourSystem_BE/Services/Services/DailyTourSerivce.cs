using AutoMapper;
using BusinessObjects.Models;
using BusinessObjects.ViewModels.DailyTour;
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
    public class DailyTourSerivce : IDailyTourService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public DailyTourSerivce(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<APIResponseModel> CreateDailyTour(DailyTourCreateModel dailyTourCreateModel)
        {
            var newDailyTour = _mapper.Map<DailyTour>(dailyTourCreateModel);
            newDailyTour.DailyTourId = Guid.NewGuid().ToString();
            newDailyTour.CreateDate = DateTime.Now;
            await _unitOfWork.DailyTourRepository.AddAsync(newDailyTour);
            _unitOfWork.Save();
            return new APIResponseModel
            {
                Message = "Create Daily Tour Successfully",
                IsSuccess = true,
                Data = newDailyTour,
            };
        }

        public async Task<APIResponseModel> DeleteDailyTour(string dailyTourId)
        {
            var dailyTour = await _unitOfWork.DailyTourRepository.GetByIdStringAsync(dailyTourId);
            if (dailyTour == null)
            {
                return new APIResponseModel
                {
                    Message = "Daily Tour not found.",
                    IsSuccess = false,
                    Data = null
                };
            }
            return new APIResponseModel
            {
                Message = "Delete Daily Tour Successfully",
                IsSuccess = true,
                Data = dailyTour,
            };
        }

        public async Task<APIResponseModel> GetAllDailyTour()
        {
            var dailyTours = await _unitOfWork.DailyTourRepository.GetAllAsync();
            return new APIResponseModel
            {
                Message = "Get All Daily Tour Successfully",
                IsSuccess = true,
                Data = dailyTours,
            };
        }

        public async Task<APIResponseModel> GetDailyTourById(string dailyTourId)
        {
            var dailyTour = await _unitOfWork.DailyTourRepository.GetByIdStringAsync(dailyTourId);
            if (dailyTour == null)
            {
                return new APIResponseModel
                {
                    Message = "Daily Tour not found.",
                    IsSuccess = false,
                    Data = null
                };
            }
            return new APIResponseModel
            {
                Message = "Get Daily Tour Successfully",
                IsSuccess = true,
                Data = dailyTour,
            };
        }

        public async Task<APIResponseModel> GetDailyTourByPackageTour(string packId)
        {
            var dailyTour = await _unitOfWork.DailyTourRepository.GetByConditionAsync(x => x.PackageTourId == packId);
            if (dailyTour == null)
            {
                return new APIResponseModel
                {
                    Message = "Daily Tour not found.",
                    IsSuccess = false,
                    Data = null
                };
            }
            return new APIResponseModel
            {
                Message = "Get Daily Tour by Package Tour Successfully",
                IsSuccess = true,
                Data = dailyTour,
            };
        }

        public async Task<APIResponseModel> GetDailyTourByStatus()
        {
            var dailyTour = await _unitOfWork.DailyTourRepository.GetByConditionAsync(s => s.Status != -1);
            if (dailyTour == null)
            {
                return new APIResponseModel
                {
                    Message = "Daily Tour not found.",
                    IsSuccess = false,
                    Data = null
                };
            }
            return new APIResponseModel
            {
                Message = "Get Daily Tour by Status Successfully",
                IsSuccess = true,
                Data = dailyTour,
            };
        }

        public async Task<APIResponseModel> UpdateDailyTour(DailyTourUpdateModel dailyTourUpdateModel)
        {
            var dailyTour = await _unitOfWork.DailyTourRepository.GetByIdGuidAsync(dailyTourUpdateModel.DailyTourId);
            if (dailyTour == null)
            {
                return new APIResponseModel
                {
                    Message = "Daily Tour not found.",
                    IsSuccess = false,
                    Data = null
                };
            }

            // Retain the existing creation date
            var createDate = dailyTour.CreateDate;

            // Map only the properties from the update model to the existing entity
            _mapper.Map(dailyTourUpdateModel, dailyTour);

            // Set the CreateDate and UpdateDate manually
            dailyTour.CreateDate = createDate;
            dailyTour.UpdateDate = DateTime.Now;

            // Save the changes
            await _unitOfWork.DailyTourRepository.UpdateAsync(dailyTour);
            _unitOfWork.Save();

            return new APIResponseModel
            {
                Message = "Update Daily Tour Successfully",
                IsSuccess = true,
                Data = dailyTour,
            };
        }

    }
}
