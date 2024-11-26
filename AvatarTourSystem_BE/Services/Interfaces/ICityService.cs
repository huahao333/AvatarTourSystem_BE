using BusinessObjects.ViewModels.City;
using Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface ICityService
    {
        Task<APIResponseModel> GetCitiesAsync();
        Task<APIResponseModel> GetActiveCitiesAsync();
        Task<APIResponseModel> GetCityByIdAsync(string CityId);
        Task<APIResponseModel> CreateCityAsync(CityCreateModel createModel);
        Task<APIResponseModel> UpdateCityAsync(CityUpdateModel updateModel);
        Task<APIResponseModel> DeleteCity(string CityId);
        Task<APIResponseModel> GetCallbackCityAsync();
    }
}
