using BusinessObjects.Models;
using BusinessObjects.ViewModels.DailyTicket;
using BusinessObjects.ViewModels.ServiceUsedByTicket;
using Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IServiceUsedByTicketService
    {
        Task<APIResponseModel> GetServiceUsedByTicketsAsync();
        Task<APIResponseModel> GetActiveServiceUsedByTicketsAsync();
        Task<APIResponseModel> GetServiceUsedByTicketByIdAsync(string SUBTId);
        Task<APIResponseModel> CreateServiceUsedByTicketAsync(ServiceUsedByTicketCreateModel createModel);
        Task<APIResponseModel> UpdateServiceUsedByTicketAsync(ServiceUsedByTicketUpdateModel updateModel);
        Task<APIResponseModel> DeleteServiceUsedByTicket(string SUBTId);
    }
}
