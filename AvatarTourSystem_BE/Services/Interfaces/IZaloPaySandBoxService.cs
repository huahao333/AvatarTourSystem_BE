using BusinessObjects.ViewModels.Payment;
using Microsoft.AspNetCore.Mvc;
using Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IZaloPaySandBoxService
    {
        Task<APIResponseModel> GetAllPayment();
        Task<APIResponseModel> HandleCallback(object callbackData);
        Task<APIResponseModel> ProcessRefund(RefundModel refundModel);
    }
}
