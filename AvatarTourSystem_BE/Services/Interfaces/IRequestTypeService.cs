using BusinessObjects.ViewModels.ResquestType;
using Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IRequestTypeService
    {
        Task<APIResponseModel> GetAllRequestType();
        Task<APIResponseModel> GetRequestTypeById(string id);
        Task<APIResponseModel> GetRequestTypeByStatus();
        Task<APIResponseModel> CreateRequestType(RequestTypeCreateModel requestTypeCreateModel);
        Task<APIResponseModel> UpdateRequestType(RequestTypeUpdateModel requestTypeUpdateModel);
        Task<APIResponseModel> DeleteRequestType(string id);
    }
}
