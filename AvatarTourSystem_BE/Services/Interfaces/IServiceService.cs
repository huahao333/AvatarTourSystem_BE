using BusinessObjects.ViewModels.Service;
using Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IServiceService
    {
        Task<APIResponseModel> GetServicesAsync();
        Task<APIResponseModel> GetActiveServicesAsync();
        Task<APIResponseModel> GetServiceByIdAsync(string id);
        Task<APIResponseModel> CreateServiceAsync(ServiceCreateModel createModel);
        Task<APIResponseModel> UpdateServiceAsync(ServiceUpdateModel updateModel);
        Task<APIResponseModel> DeleteService(string id);
        Task<APIResponseModel> GetServicesByLocation(ServiceByLocationModel serviceByLocationModel);
    }
}
