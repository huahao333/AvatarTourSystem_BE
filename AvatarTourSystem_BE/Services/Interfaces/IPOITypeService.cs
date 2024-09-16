using BusinessObjects.ViewModels.POI;
using BusinessObjects.ViewModels.POIType;
using Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IPOITypeService
    {
        Task<APIResponseModel> GetAllPOITypes();
        Task<APIResponseModel> GetPOITypesByStatus();
        Task<APIResponseModel> GetPOITypeById(string id);
        Task<APIResponseModel> CreatePOIType(POITypeCreateModel poiTypeCreateModel);
        Task<APIResponseModel> UpdatePOIType(POITypeUpdateModel poiTypeUpdateModel);
        Task<APIResponseModel> DeletePOIType(string id);
    }
}
