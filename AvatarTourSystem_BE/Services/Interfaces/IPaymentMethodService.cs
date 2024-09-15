using BusinessObjects.Models;
using BusinessObjects.ViewModels.PaymentMethod;
using Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IPaymentMethodService 
    {
        Task<APIResponseModel> GetAllPaymentMethods();
        Task<APIResponseModel> GetPaymentMethodsByStatus();
        Task<APIResponseModel> GetPaymentMethodById(string id);
        Task<APIResponseModel> CreatePaymentMethod(PaymentMethodCreateModel paymentMethodCreateModel);
        Task<APIResponseModel> UpdatePaymentMethod( PaymentMethodUpdateModel paymentMethodUpdateModel);
        Task<APIResponseModel> DeletePaymentMethod(string id);
    }
}
