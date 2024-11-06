using BusinessObjects.ViewModels.PackageTourFlow.PackageTourUpdate;
using BusinessObjects.ViewModels.PackageTourFlow;
using Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.ViewModels.Booking;
using BusinessObjects.ViewModels.Account;

namespace Services.Interfaces
{
    public interface IBookingFlowService
    {
        Task<APIResponseModel> GetBookingFlowAsync();
        Task<APIResponseModel> GetBookingFlowByZaloIdAsync(AccountZaloIdModel accountZaloIdModel);
        Task<APIResponseModel> CreateBookingFlowAsync(BookingFlowCreateModel createModel);
        Task<APIResponseModel> UpdateBookingFlowAsync(BookingUpdateModel updateModel);
        Task<APIResponseModel> UpdateBookingByZaloIdFlowAsync(BookingModel updateModel);
        Task<APIResponseModel> ShareTicketByPhoneNumber(BookingPhoneNumberShareTicket updateModel);
        Task<APIResponseModel> UpdateBookingStatusAsync(BookingFlowModel updateModel);
        Task<APIResponseModel> DecryptBookingFlowAsync(DecryptBooking encryptedQrData);
        Task<APIResponseModel> UpdateTicketByQR(TicketUsageViewModel ticketUsageViewModel);
    }
}
