using BusinessObjects.ViewModels.PackageTour;
using BusinessObjects.ViewModels.Supplier;
using Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IPackageTourService
    {
        Task<APIResponseModel> GetPackageToursAsync();
        Task<APIResponseModel> GetActivePackageToursAsync();
        Task<PackageTourModel> GetPackageTourByIdAsync(string PackageTourId);
        Task<APIResponseModel> CreatePackageTourAsync(PackageTourCreateModel createModel);
        Task<APIResponseModel> UpdatePackageTourAsync(PackageTourUpdateModel updateModel);
        Task<APIResponseModel> DeletePackageTour(string PackageTourId);
    }
}
