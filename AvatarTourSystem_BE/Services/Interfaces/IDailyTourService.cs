using BusinessObjects.ViewModels.DailyTour;
using Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IDailyTourService
    {
        Task<APIResponseModel> GetAllDailyTour();
        Task<APIResponseModel> GetDailyTourById(string dailyTourId);
        Task<APIResponseModel> GetDailyTourByStatus();
        Task<APIResponseModel> GetDailyTourByPackageTour(string packId);
        Task<APIResponseModel> CreateDailyTour(DailyTourCreateModel dailyTourUpdateModel);
        Task<APIResponseModel> UpdateDailyTour(DailyTourUpdateModel dailyTourUpdateModel);
        Task<APIResponseModel> DeleteDailyTour(string dailyTourId);

    }
}
