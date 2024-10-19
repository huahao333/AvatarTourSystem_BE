using BusinessObjects.ViewModels.PackageTourFlow.PackageTourUpdate;
using BusinessObjects.ViewModels.PackageTourFlow;
using Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.ViewModels.Booking;

namespace Services.Interfaces
{
    public interface IBookingFlowService
    {
        Task<APIResponseModel> GetBookingFlowAsync();
        Task<APIResponseModel> GetBookingFlowByIdAsync(string id);
        Task<APIResponseModel> CreateBookingFlowAsync(BookingCreateModel createModel);
        Task<APIResponseModel> UpdateBookingFlowAsync(BookingUpdateModel updateModel);
        Task<APIResponseModel> UpdateBookingByZaloIdFlowAsync(BookingModel updateModel);
    }
}
