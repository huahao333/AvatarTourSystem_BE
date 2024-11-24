using BusinessObjects.ViewModels.PackageTourFlow;
using BusinessObjects.ViewModels.PackageTourFlow.PackageTourGet;
using BusinessObjects.ViewModels.PackageTourFlow.PackageTourUpdate;
using Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IPackageTourFlowService
    {
        Task<APIResponseModel> GetPackageTourFlowAsync();
        Task<APIResponseModel> GetPackageTourFlowByIdAsync(string id);
        Task<APIResponseModel> GetDestinationByCityIdAsync(GetDestinationByCityModel cityId);
        Task<APIResponseModel> GetLocationsByDestinationIdAsync(GetLocationByDestinationModel destinationId);
        Task<APIResponseModel> GetServicesByLocationIdAsync(GetServiceByLocationModel locationId);
        Task<APIResponseModel> CreatePackageTourFlowAsync(FPackageTourCreateModel createModel);
        Task<APIResponseModel> CreatePartsPackageTourFlowAsync(FPackageTourUpdate updateModel);
        Task<APIResponseModel> UpdatePackageTourFlowAsync(FPackageTourUpdateModel updateModel);
        Task<APIResponseModel> AddPartToPackageTourFlow(FPackageTourUpdateModel createModel);

        Task<APIResponseModel> CreatePackageTourAsync(FPackageTourCreatedModel createModel);

    }
}
