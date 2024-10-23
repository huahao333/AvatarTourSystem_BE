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

        public async Task<APIResponseModel> CreateFeedbackByZaloUser(FeedbackCreateWithZaloModel feedbackCreateModel)
        {
            var feedback = _mapper.Map<Feedback>(feedbackCreateModel);
            feedback.FeedbackId = Guid.NewGuid().ToString();
            feedback.CreateDate = DateTime.Now;
            var user = await _unitOfWork.AccountRepository.GetByConditionAsync(x => x.ZaloUser == feedbackCreateModel.ZaloUser);
            if (user == null || !user.Any())
            {
                return new APIResponseModel
                {
                    Message = "Zalo User not found.",
                    IsSuccess = false,
                    Data = null
                };
            }
            feedback.UserId = user.FirstOrDefault().Id;
            await _unitOfWork.FeedbackRepository.AddAsync(feedback);
            _unitOfWork.Save();
            return new APIResponseModel
            {
                Message = "Create Feedback Successfully",
                IsSuccess = true,
                Data = new
                {
                    feedback.FeedbackId,
                    feedback.FeedbackMsg,
                    feedback.BookingId,
                    feedback.CreateDate,
                    feedback.Status
                }
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

        public async Task<APIResponseModel> GetFeedbackByZaloUser(string zalouser)
        {
            var feedbacks = await _unitOfWork.FeedbackRepository.GetByConditionAsync(x => x.Accounts.ZaloUser == zalouser);
            if (feedbacks == null || !feedbacks.Any())
            {
                return new APIResponseModel
                {
                    Message = "Feedback not found.",
                    IsSuccess = false,
                    Data = null
                };
            }
            var feedbackWithPackageTour = new List<object>();

            foreach (var feedback in feedbacks)
            {
                string? packageTourId = null;  // Khởi tạo giá trị null cho PackageTourId

                // Tìm Booking liên quan đến Feedback
                var booking = await _unitOfWork.BookingRepository.GetByIdStringAsync(feedback.BookingId);

                if (booking != null)
                {
                    // Tìm DailyTour liên quan đến Booking
                    var dailyTour = await _unitOfWork.DailyTourRepository.GetByIdStringAsync(booking.DailyTourId);

                    if (dailyTour != null)
                    {
                        // Lấy PackageTourId từ DailyTour
                        packageTourId = dailyTour.PackageTourId;
                    }
                }

                // Thêm thông tin feedback cùng với PackageTourId vào kết quả
                feedbackWithPackageTour.Add(new
                {
                    FeedbackId = feedback.FeedbackId,
                    UserId = feedback.UserId,
                    BookingId = feedback.BookingId,
                    FeedbackMsg = feedback.FeedbackMsg,
                    CreateDate = feedback.CreateDate,
                    UpdateDate = feedback.UpdateDate,
                    Status = feedback.Status,
                    PackageTourId = packageTourId // Chỉ trả về PackageTourId
                });
            }

            return new APIResponseModel
            {
                Message = "Get Feedbacks by Zalo User Successfully",
                IsSuccess = true,
                Data = feedbackWithPackageTour,
            };
        }

        public async Task<APIResponseModel> GetFeedbackByZaloUserAndBookingID(FeedbackGetModel feedbackGetModel)
        {
            var feedbacks = await _unitOfWork.FeedbackRepository.GetByConditionAsync(x => x.Accounts.ZaloUser == feedbackGetModel.ZaloUser && x.BookingId == feedbackGetModel.BookingId);
            var feedbackByZaloAndBooking = feedbacks.FirstOrDefault();
            if (feedbackByZaloAndBooking == null)
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
                Message = "Get Feedbacks by Zalo User and Booking Id Successfully",
                IsSuccess = true,
                Data = feedbackByZaloAndBooking,
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
