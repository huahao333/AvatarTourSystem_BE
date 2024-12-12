using BusinessObjects.ViewModels.Rate;
using Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IRateService
    {
        Task<APIResponseModel> GetAllRate();
        Task<APIResponseModel> GetRateByStatus();
        Task<APIResponseModel> GetRateById(string id);
        Task<APIResponseModel> GetRateByBookingId(string id);
        Task<APIResponseModel> GetRateByUserId(string id);
        Task<APIResponseModel> GetRateByZaloUser(string zalouser);
        Task<APIResponseModel> CreaateRateWithZaloAndBooking(RateCreateWithZaloModel rateCreateModel);
        Task<APIResponseModel> CreateRate(RateCreateModel rate);
        Task<APIResponseModel> UpdateRate(RateUpdateModel rate);
        Task<APIResponseModel> DeleteRate(string id);
        Task<APIResponseModel> GetFeedbackAndRateByZaloId(RateAndFeedbackByzZaoloId rateAndFeedbackByzZaoloId);

    }
}
