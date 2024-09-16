using BusinessObjects.ViewModels.Booking;
using BusinessObjects.ViewModels.Location;
using Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace   Services.Interfaces
{
    public interface ILocationService
    {
        Task<APIResponseModel> GetLocationsAsync();
        Task<APIResponseModel> GetActiveLocationsAsync();
        Task<APIResponseModel> GetLocationByIdAsync(string locationId);
        Task<APIResponseModel> CreateLocationAsync(LocationCreateModel locationModel);
        Task<APIResponseModel> UpdateLocationAsync(LocationUpdateModel locationModel);
        Task<APIResponseModel> DeleteLocation(string locationId);
    }
}
