﻿using BusinessObjects.ViewModels.Feedback;
using Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IFeedbackService 
    {
        Task<APIResponseModel> GetAllFeedbacks();
        Task<APIResponseModel> GetFeedbackById(string id);
        Task<APIResponseModel> GetFeedbackByUserId(string userId);
        Task<APIResponseModel> GetFeedbackByBookingId(string bookingId);
        Task<APIResponseModel> GetFeedbackByZaloUser(string zalouser);
        Task<APIResponseModel> GetFeedbackByZaloUserAndBookingID(FeedbackGetModel feedbackGetModel);
        Task<APIResponseModel> GetFeedbackByStatus();
        Task<APIResponseModel> CreateFeedback(FeedbackCreateModel feedbackCreateModel);
        Task<APIResponseModel> CreateFeedbackByZaloUser(FeedbackCreateWithZaloModel feedbackCreateModel);
        Task<APIResponseModel> UpdateFeedback(FeedbackUpdateModel feedbackUpdateModel);
        Task<APIResponseModel> DeleteFeedback(string id);       
    }
}
