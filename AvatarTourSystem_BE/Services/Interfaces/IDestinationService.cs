using BusinessObjects.ViewModels.Destination;
using Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IDestinationService
    {
        Task<APIResponseModel> GetDestinationsAsync();
        Task<APIResponseModel> GetActiveDestinationsAsync();
        Task<DestinationModel> GetDestinationByIdAsync(string DestinationId);
        Task<APIResponseModel> CreateDestinationAsync(DestinationCreateModel createModel);
        Task<APIResponseModel> UpdateDestinationAsync(DestinationUpdateModel updateModel);
        Task<APIResponseModel> DeleteDestination(string DestinationId);
    }
}
