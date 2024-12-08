using BusinessObjects.ViewModels.Booking;
using BusinessObjects.ViewModels.ServiceUsedByTicket;
using Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IBookingService
    {
        Task<APIResponseModel> GetBookingsAsync();
        Task<APIResponseModel> GetActiveBookingsAsync();
        Task<APIResponseModel> GetBookingByIdAsync(string bookingId);
        Task<APIResponseModel> CreateBookingAsync(BookingCreateModel createModel);
        Task<APIResponseModel> UpdateBookingAsync(BookingUpdateModel updateModel);
        Task<APIResponseModel> DeleteBooking(string bookingId);
        Task<APIResponseModel> GetAllBookingsAsync();
        Task<APIResponseModel> GetAllBookingsByDailyTourIdAsync(BookingByDailyTourIdViewModel bookingByDailyTourIdViewModel);
    }
}
