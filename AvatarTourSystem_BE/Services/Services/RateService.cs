using AutoMapper;
using BusinessObjects.Enums;
using BusinessObjects.Models;
using BusinessObjects.ViewModels.Feedback;
using BusinessObjects.ViewModels.Rate;
using Microsoft.EntityFrameworkCore;
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
    public class RateService : IRateService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public RateService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<APIResponseModel> CreaateRateWithZaloAndBooking(RateCreateWithZaloModel rateCreateModel)
        {
            var rate = _mapper.Map<Rate>(rateCreateModel);
            rate.RateId = Guid.NewGuid().ToString();
            rate.CreateDate = DateTime.Now;
            rate.RateStar = rateCreateModel.RateStar;
            rate.Status = (int)EStatus.Active;

            var user = await _unitOfWork.AccountRepository.GetByConditionAsync(x => x.ZaloUser == rateCreateModel.ZaloUser);
            if (user == null || !user.Any())
            {
                return new APIResponseModel
                {
                    Message = "Zalo User not found.",
                    IsSuccess = false,
                    Data = null
                };
            }
            rate.UserId = user.FirstOrDefault().Id;
            await _unitOfWork.RateRepository.AddAsync(rate);
            _unitOfWork.Save();
            return new APIResponseModel
            {
                Message = "Create Rate Successfully",
                IsSuccess = true,
                Data = new
                {
                    rate.RateId,
                    rate.RateStar,
                    rate.BookingId,
                    rate.CreateDate,
                    rate.Status
                }
            };
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

        public Task<APIResponseModel> GetRateByZaloUser(string zalouser)
        {
            throw new NotImplementedException();
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

        public async Task<APIResponseModel> GetFeedbackAndRateByZaloId(RateAndFeedbackByzZaoloId rateAndFeedbackByzZaoloId)
        {
            try
            {
                var user = await _unitOfWork.AccountRepository.GetFirstOrDefaultAsync(query => query.Where(a=>a.ZaloUser==rateAndFeedbackByzZaoloId.ZaloId));
                if (user == null)
                {
                    return new APIResponseModel
                    {
                        Message = "ZaloId not found",
                        IsSuccess = false
                    };
                }
               

                var bookingRate = await _unitOfWork.BookingRepository.GetAllAsyncs(query => query.Where(b=>b.UserId==user.Id)
                                                                                                           .Include(r=>r.Rates)
                                                                                                           .Include(f=>f.Feedbacks)
                                                                                                           .Include(d=>d.DailyTours));

                if (bookingRate == null || !bookingRate.Any())
                {
                    return new APIResponseModel
                    {
                        Message = "Feedback not found",
                        IsSuccess = false
                    };
                }

                var feedbackAndRateDetails = bookingRate.Select(b => new
                {
                    DailyTourName = b.DailyTours?.DailyTourName,
                    FeedbackMessages = b.Feedbacks?.Select(f => f.FeedbackMsg).ToList(),
                    Rates = b.Rates?.Select(r => r.RateStar).ToList()
                }).ToList();

                return new APIResponseModel
                {
                    Message = "Data retrieved successfully",
                    IsSuccess = true,
                    Data = feedbackAndRateDetails
                };

            }
            catch (Exception ex)
            {
                return new APIResponseModel
                {
                    Message = "Error get data",
                    IsSuccess = false
                };
            }
        }
    }
}
