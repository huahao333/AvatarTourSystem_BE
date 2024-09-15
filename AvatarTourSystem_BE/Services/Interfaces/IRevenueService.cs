using BusinessObjects.ViewModels.Booking;
using BusinessObjects.ViewModels.Revenue;
using Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IRevenueService
    {
        Task<APIResponseModel> GetRevenuesAsync();
        Task<APIResponseModel> GetActiveRevenuesAsync();
        Task<APIResponseModel> GetRevenueByIdAsync(string revenueId);
        Task<APIResponseModel> CreateRevenueAsync(RevenueCreateModel createModel);
        Task<APIResponseModel> UpdateRevenueAsync(RevenueUpdateModel updateModel);
        Task<APIResponseModel> DeleteRevenue(string revenueId);
    }
}
