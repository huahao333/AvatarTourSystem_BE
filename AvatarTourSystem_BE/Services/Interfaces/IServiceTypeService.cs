using BusinessObjects.ViewModels.ServiceType;
using Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IServiceTypeService
    {
        Task<APIResponseModel> GetServiceTypesAsync();
        Task<APIResponseModel> GetActiveServiceTypesAsync();
        Task<APIResponseModel> GetServiceTypeByIdAsync(string id);
        Task<APIResponseModel> CreateServiceTypeAsync(ServiceTypeCreateModel createModel);
        Task<APIResponseModel> UpdateServiceTypeAsync(ServiceTypeUpdateModel updateModel);
        Task<APIResponseModel> DeleteServiceType(string id);
    }
}
