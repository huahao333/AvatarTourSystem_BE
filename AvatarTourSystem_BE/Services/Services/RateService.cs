using AutoMapper;
using BusinessObjects.Enums;
using BusinessObjects.Models;
using BusinessObjects.ViewModels.Rate;
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
    public class RateService : IRateService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public RateService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<APIResponseModel> CreateRate(RateCreateModel rate)
        {
            var newRate = _mapper.Map<Rate>(rate);
            newRate.RateId = Guid.NewGuid().ToString();
            newRate.CreateDate = DateTime.Now;
            await _unitOfWork.RateRepository.AddAsync(newRate);
            _unitOfWork.Save();
            return new APIResponseModel
            {
                Message = "Create Rate Successfully",
                IsSuccess = true,
                Data = newRate,
            };

        }

        public async Task<APIResponseModel> DeleteRate(string id)
        {
            var rate = await _unitOfWork.RateRepository.GetByIdStringAsync(id);
            if (rate == null)
            {
                return new APIResponseModel
                {
                    Message = "Rate not found.",
                    IsSuccess = false,
                    Data = null
                };
            }
            if (rate.Status == (int?)EStatus.IsDeleted)
            {
                return new APIResponseModel
                {
                    Message = "Rate has already been deleted.",
                    IsSuccess = false,
                    Data = null
                };
            }
            var createdDate = rate.CreateDate;
            rate.Status = (int)EStatus.IsDeleted;
            rate.CreateDate = createdDate;
            rate.UpdateDate = DateTime.Now;
            var result = await  _unitOfWork.RateRepository.UpdateAsync(rate);
            _unitOfWork.Save();
            return new APIResponseModel
            {
                Message = "Delete Rate Successfully",
                IsSuccess = true,
                Data = result,
            };
        }

        public async Task<APIResponseModel> GetAllRate()
        {
           var rates = await _unitOfWork.RateRepository.GetAllAsync();
            return new APIResponseModel
            {
                Message = "Get All Rate Successfully",
                IsSuccess = true,
                Data = rates,
            };
        }

        public async Task<APIResponseModel> GetRateByBookingId(string id)
        {
            var rate = await _unitOfWork.RateRepository.GetByConditionAsync(x => x.BookingId == id);
            return new APIResponseModel
            {
                Message = "Get Rate by Booking Id Successfully",
                IsSuccess = true,
                Data = rate,
            };
        }

        public async Task<APIResponseModel> GetRateById(string id)
        {
           var rate =  await _unitOfWork.RateRepository.GetByIdStringAsync(id);
            return new APIResponseModel
            {
                Message = "Get Rate by Id Successfully",
                IsSuccess = true,
                Data = rate,
            };
        }

        public async Task<APIResponseModel> GetRateByStatus()
        {
            var rate = await _unitOfWork.RateRepository.GetByConditionAsync(s => s.Status != -1);
            if (rate == null)
            {
                return new APIResponseModel
                {
                    Message = "Rate not found.",
                    IsSuccess = false,
                    Data = null
                };
            }
            return new APIResponseModel
            {
                Message = "Get Rate by Status Successfully",
                IsSuccess = true,
                Data = rate,
            };
        }

        public async Task<APIResponseModel> GetRateByUserId(string id)
        {
            var rate = await _unitOfWork.RateRepository.GetByConditionAsync(x => x.UserId == id);
            return new APIResponseModel
            {
                Message = "Get Rate by User Id Successfully",
                IsSuccess = true,
                Data = rate,
            };
        }

        public async Task<APIResponseModel> UpdateRate(RateUpdateModel rate)
        {
            var existingRate = await _unitOfWork.RateRepository.GetByIdGuidAsync(rate.RateId);
            if (existingRate == null)
            {
                return new APIResponseModel
                {
                    Message = "Rate not found.",
                    IsSuccess = false,
                    Data = null
                };
            }
            var createdDate = existingRate.CreateDate;
            var rateUpdate = _mapper.Map(rate, existingRate);
            existingRate.CreateDate = createdDate;
            existingRate.UpdateDate = DateTime.Now;
            var result = await _unitOfWork.RateRepository.UpdateAsync(existingRate);
            _unitOfWork.Save();
            return new APIResponseModel
            {
                Message = "Update Rate Successfully",
                IsSuccess = true,
                Data = result,
            };

        }
    }
}
