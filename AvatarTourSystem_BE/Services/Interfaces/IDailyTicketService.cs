using BusinessObjects.ViewModels.DailyTicket;
using BusinessObjects.ViewModels.TicketType;
using Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IDailyTicketService
    {
        Task<APIResponseModel> GetDailyTicketsAsync();
        Task<APIResponseModel> GetActiveDailyTicketsAsync();
        Task<APIResponseModel> GetDailyTicketByIdAsync(string DailyTicketId);
        Task<APIResponseModel> CreateDailyTicketAsync(DailyTicketCreateModel createModel);
        Task<APIResponseModel> UpdateDailyTicketAsync(DailyTicketUpdateModel updateModel);
        Task<APIResponseModel> DeleteDailyTicket(string DailyTicketId);
    }
}
