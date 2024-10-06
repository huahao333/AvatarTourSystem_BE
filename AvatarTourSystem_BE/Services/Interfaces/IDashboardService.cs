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
        public Task<APIGenericResponseModel<int>> GetCountActiveZaloUser();
        public Task<APIResponseModel> GetActiveZaloUser();
        public Task<APIGenericResponseModel<decimal>> GetTotalRevenue();
        public Task<APIGenericResponseModel<decimal>> GetMonthlyBookings(int month, int year);
        public Task<APIGenericResponseModel<int>> GetMonthlyTour(int month, int year);
        public Task<decimal> GetMonthlytAdultTickets(int month, int year);
        public Task<decimal> GetMonthlytChildTickets(int month, int year);
    }
}
