using BusinessObjects.ViewModels.PackageTourFlow;
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
        Task<APIResponseModel> CreatePackageTourFlowAsync(FPackageTourCreateModel createModel);
    }
}
