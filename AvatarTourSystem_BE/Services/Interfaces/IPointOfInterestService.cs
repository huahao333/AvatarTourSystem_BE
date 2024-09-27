using BusinessObjects.ViewModels.POI;
using Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IPointOfInterestService
    {
        Task<APIResponseModel> GetAllPointOfInterests();
        Task<APIResponseModel> GetPointOfInterestsByStatus();
        Task<APIResponseModel> GetPointOfInterestById(string id);
        Task<APIResponseModel> CreatePointOfInterest(POICreateModel poiCreateModel);
        Task<APIResponseModel> UpdatePointOfInterest(POIUpdateModel poiUpdateModel);
        Task<APIResponseModel> DeletePointOfInterest(string id);
    }
}
