using BusinessObjects.ViewModels.Supplier;
using BusinessObjects.ViewModels.TourSegment;
using Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface ITourSegmentService
    {
        Task<APIResponseModel> GetTourSegmentsAsync();
        Task<APIResponseModel> GetActiveTourSegmentsAsync();
        Task<APIResponseModel> GetTourSegmentByIdAsync(string TourSegmentId);
        Task<APIResponseModel> CreateTourSegmentAsync(TourSegmentCreateModel createModel);
        Task<APIResponseModel> UpdateTourSegmentAsync(TourSegmentUpdateModel updateModel);
        Task<APIResponseModel> DeleteTourSegment(string TourSegmentId);
    }
}
