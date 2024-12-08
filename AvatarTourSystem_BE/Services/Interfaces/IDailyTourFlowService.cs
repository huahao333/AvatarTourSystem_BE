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
     //   Task<APIResponseModel> CreateDailyTourFlow(DailyTourFlowModel dailyTourFlowModel);
        Task<APIResponseModel> CreateDailyTourFlow(DailyToursFlowModel dailyTourFlowModel);
        Task<APIResponseModel> UpdateDailyTourFlow(UpdateDailyTourFlowModel updateModel);
        Task<APIResponseModel> GetDailyTourDetails(string dailyTourId);
        Task<APIResponseModel> GetAllDailyToursForUser();
        Task<APIResponseModel> GetAllDailysTours();
        Task<APIResponseModel> GetDailyToursHaveDiscount();
        Task<APIResponseModel> GetDailyToursHavePOI();
        Task<APIResponseModel> UpdateStatusDailyTour(UpdateStatusDailyTourViewModel updateStatusDailyTourViewModel);
        Task<APIResponseModel> UpdateStatusPackageTour(UpdateStatusPackageTourViewModel updateStatusPackageTourViewModel);
    }
}
