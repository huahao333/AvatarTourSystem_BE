using BusinessObjects.ViewModels.Booking;
using Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IDashboardService
    {
        public Task<APIResponseModel> GetActiveZaloUser();
        public Task<APIGenericResponseModel<int>> GetActiveZaloUserCount();
        public Task<APIGenericResponseModel<decimal>> GetMonthlyBookings(int month, int year);
        public Task<APIGenericResponseModel<int>> GetMonthlyTours(int month, int year);
        public Task<APIGenericResponseModel<decimal>> GetMonthlyRevenue(int month, int year);
        public Task<APIGenericResponseModel<int>> GetMonthlyTicketsByType(string typeId, int month, int year);
    }
}
