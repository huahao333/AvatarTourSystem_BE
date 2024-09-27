using AutoMapper;
using BusinessObjects.Enums;
using BusinessObjects.Models;
using BusinessObjects.ViewModels.Feedback;
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
    public class FeedbackService : IFeedbackService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public FeedbackService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<APIResponseModel> CreateFeedback(FeedbackCreateModel feedbackCreateModel)
        {
            var feedback =  _mapper.Map<Feedback>(feedbackCreateModel);
            feedback.FeedbackId = Guid.NewGuid().ToString();
            feedback.CreateDate = DateTime.Now;
            await _unitOfWork.FeedbackRepository.AddAsync(feedback);
            _unitOfWork.Save();
            return new APIResponseModel
            {
                Message = "Create Feedback Successfully",
                IsSuccess = true,
                Data = feedback,
            };
        }

        public async Task<APIResponseModel> DeleteFeedback(string id)
        {
            var feedback = await _unitOfWork.FeedbackRepository.GetByIdStringAsync(id);
            if (feedback == null)
            {
                return new APIResponseModel
                {
                    Message = "Feedback not found.",
                    IsSuccess = false,
                    Data = null
                };
            }
            if (feedback.Status == (int?)EStatus.IsDeleted)
            {
                return new APIResponseModel
                {
                    Message = "Feedback has already been deleted.",
                    IsSuccess = false,
                    Data = null
                };
            }
            var createDate = feedback.CreateDate;
            feedback.Status = (int?)EStatus.IsDeleted;
            feedback.UpdateDate = DateTime.Now;
            feedback.CreateDate = createDate;

            var result = await _unitOfWork.FeedbackRepository.UpdateAsync(feedback);
            _unitOfWork.Save();
            return new APIResponseModel
            {
                Message = "Delete Feedback Successfully",
                IsSuccess = true,
                Data = result,
            };
        }

        public async Task<APIResponseModel> GetAllFeedbacks()
        {
            var feedbacks = await _unitOfWork.FeedbackRepository.GetAllAsync();
            return new APIResponseModel
            {
                Message = "Get All Feedbacks Successfully",
                IsSuccess = true,
                Data = feedbacks,
            };
        }

        public async Task<APIResponseModel> GetFeedbackByBookingId(string bookingId)
        {
           var feedbacks = await _unitOfWork.FeedbackRepository.GetByConditionAsync(x => x.BookingId == bookingId);
            if (feedbacks == null || !feedbacks.Any())
            {
                return new APIResponseModel
                {
                    Message = "Feedback not found.",
                    IsSuccess = false,
                    Data = null
                };
            }
            return new APIResponseModel
            {
                Message = "Get Feedbacks by Booking Id Successfully",
                IsSuccess = true,
                Data = feedbacks,
            };
        }

        public async Task<APIResponseModel> GetFeedbackById(string id)
        {
            var feedback = await _unitOfWork.FeedbackRepository.GetByIdStringAsync(id);
            if (feedback == null)
            {
                return new APIResponseModel
                {
                    Message = "Feedback not found.",
                    IsSuccess = false,
                    Data = null
                };
            }
            return new APIResponseModel
            {
                Message = "Get Feedback by Id Successfully",
                IsSuccess = true,
                Data = feedback,
            };
        }

        public async Task<APIResponseModel> GetFeedbackByStatus()
        {
            var feedbacks = await _unitOfWork.FeedbackRepository.GetByConditionAsync(s => s.Status != -1);
            if (feedbacks == null || !feedbacks.Any())
            {
                return new APIResponseModel
                {
                    Message = "Feedback not found.",
                    IsSuccess = false,
                    Data = null
                };
            }
            return new APIResponseModel
            {
                Message = "Get Feedbacks by Status Successfully",
                IsSuccess = true,
                Data = feedbacks,
            };
        }

        public async Task<APIResponseModel> GetFeedbackByUserId(string userId)
        {
            var feedbacks = await _unitOfWork.FeedbackRepository.GetByConditionAsync(x => x.UserId == userId);
            if (feedbacks == null || !feedbacks.Any())
            {
                return new APIResponseModel
                {
                    Message = "Feedback not found.",
                    IsSuccess = false,
                    Data = null
                };
            }
            return new APIResponseModel
            {
                Message = "Get Feedbacks by User Id Successfully",
                IsSuccess = true,
                Data = feedbacks,
            };
        }

        public async Task<APIResponseModel> UpdateFeedback(FeedbackUpdateModel feedbackUpdateModel)
        {
            var existingFeedback = await _unitOfWork.FeedbackRepository.GetByIdGuidAsync(feedbackUpdateModel.FeedbackId);
            if (existingFeedback == null)
            {
                return new APIResponseModel
                {
                    Message = "Feedback not found.",
                    IsSuccess = false,
                   
                };
            }
            var feedbackUpdate = _mapper.Map(feedbackUpdateModel, existingFeedback);
            feedbackUpdate.CreateDate = existingFeedback.CreateDate;
            feedbackUpdate.UpdateDate = DateTime.Now;
            var result = await _unitOfWork.FeedbackRepository.UpdateAsync(feedbackUpdate);
            _unitOfWork.Save();
            return new APIResponseModel
            {
                Message = "Update Feedback Successfully",
                IsSuccess = true,
                Data = result,
            };

        }
    }
}
