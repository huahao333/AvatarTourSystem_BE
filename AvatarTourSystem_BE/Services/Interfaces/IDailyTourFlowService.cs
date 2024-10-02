using BusinessObjects.ViewModels.DailyTour;
using Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IDailyTourFlowService
    {
        Task<APIResponseModel> CreateDailyTourFlow(DailyTourFlowModel dailyTourFlowModel);
        Task<APIResponseModel> GetDailyTourDetails(string dailyTourId);
       // Task<APIResponseModel> GetAllDailyTours();
        Task<APIResponseModel> GetAllDailysTours();
        Task<APIResponseModel> GetDailyToursHaveDiscount();
        Task<APIResponseModel> GetDailyToursHavePOI();
    }
}
