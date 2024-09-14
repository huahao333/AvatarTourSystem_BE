using BusinessObjects.ViewModels.PackageTour;
using BusinessObjects.ViewModels.TicketType;
using Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface ITicketTypeService
    {
        Task<APIResponseModel> GetTicketTypesAsync();
        Task<APIResponseModel> GetActiveTicketTypesAsync();
        Task<TicketTypeModel> GetTicketTypeByIdAsync(string TicketTypeId);
        Task<APIResponseModel> CreateTicketTypeAsync(TicketTypeCreateModel createModel);
        Task<APIResponseModel> UpdateTicketTypeAsync(TicketTypeUpdateModel updateModel);
        Task<APIResponseModel> DeleteTicketType(string TicketTypeId);
    }
}
