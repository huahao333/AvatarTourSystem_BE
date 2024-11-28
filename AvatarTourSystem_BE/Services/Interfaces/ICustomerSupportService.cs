using BusinessObjects.ViewModels.Account;
using BusinessObjects.ViewModels.CustomerSupport;
using Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface ICustomerSupportService
    {
        Task<APIResponseModel> GetAllCustomerSupport();
        Task<APIResponseModel> GetCustomerSupportByStatus();
        Task<APIResponseModel> GetCustomerSupportById(string cusId);
        Task<APIResponseModel> GetCustomerSupportByUserId(string userId);
        Task<APIResponseModel> CreateCustomerSupport(CustomerSupportCreateModel createModel);
        Task<APIResponseModel> UpdateCustomerSupport(CustomerSupportUpdateModel updateModel);
        Task<APIResponseModel> DeleteCustomerSupport(string cusId);
        Task<APIResponseModel> GetAllRequest();
        Task<APIResponseModel> UpdateStatusCustomerSupport(CustomerSupportStatusViewModel customerSupportStatusViewModel);
    }
}
