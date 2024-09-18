using BusinessObjects.ViewModels.ServiceByTourSegment;
using Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IServiceByTourSegmentService
    {
        Task<APIResponseModel> GetServiceByTourSegmentsAsync();
        Task<APIResponseModel> GetActiveServiceByTourSegmentsAsync();
        Task<APIResponseModel> GetServiceByTourSegmentByIdAsync(string SBTSId);
        Task<APIResponseModel> CreateServiceByTourSegmentAsync(ServiceByTourSegmentCreateModel createModel);
        Task<APIResponseModel> UpdateServiceByTourSegmentAsync(ServiceByTourSegmentUpdateModel updateModel);
        Task<APIResponseModel> DeleteServiceByTourSegment(string SBTSId);
    }
}
