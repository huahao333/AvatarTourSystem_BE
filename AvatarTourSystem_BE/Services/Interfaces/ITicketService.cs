using BusinessObjects.ViewModels.Ticket;
using BusinessObjects.ViewModels.TicketType;
using Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface ITicketService
    {
        Task<APIResponseModel> GetTicketsAsync();
        Task<APIResponseModel> GetActiveTicketsAsync();
        Task<TicketModel> GetTicketByIdAsync(string TicketId);
        Task<APIResponseModel> CreateTicketAsync(TicketCreateModel createModel);
        Task<APIResponseModel> UpdateTicketAsync(TicketUpdateModel updateModel);
        Task<APIResponseModel> DeleteTicket(string TicketId);
    }
}
