using BusinessObjects.ViewModels.Booking;
using BusinessObjects.ViewModels.BookingByRevenue;
using Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IBookingByRevenueService
    {
        Task<APIResponseModel> GetBookingByRevenuesAsync();
        Task<APIResponseModel> GetActiveBookingByRevenuesAsync();
        Task<APIResponseModel> GetBookingByRevenueByIdAsync(string bookingByRevenueId);
        Task<APIResponseModel> CreateBookingByRevenueAsync(BookingByRevenueCreateModel createModel);
        Task<APIResponseModel> UpdateBookingByRevenueAsync(BookingByRevenueUpdateModel updateModel);
        Task<APIResponseModel> DeleteBookingByRevenue(string bookingByRevenueId);
    }
}
