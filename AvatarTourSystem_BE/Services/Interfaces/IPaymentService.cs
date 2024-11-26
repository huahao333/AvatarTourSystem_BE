using BusinessObjects.ViewModels.PaymentMethod;
using BusinessObjects.ViewModels.POI;
using Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IPaymentService
    {
        Task<APIResponseModel> GetAllPayments();
        Task<APIResponseModel> GetPaymentsByStatus();
        Task<APIResponseModel> GetPaymentById(string id);
        Task<APIResponseModel> CreatePayment(PaymentMethodCreateModel paymentMethodCreateModel);
        Task<APIResponseModel> UpdatePayment(PaymentMethodUpdateModel paymentMethodUpdateModel);
        Task<APIResponseModel> DeletePayment(string id);
    }
}
