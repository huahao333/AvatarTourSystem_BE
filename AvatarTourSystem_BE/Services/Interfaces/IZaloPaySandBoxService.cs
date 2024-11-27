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
        Task<APIResponseModel> HandleCallback([FromBody] object callbackData);
        Task<APIResponseModel> ProcessRefund(string zptransid, long amount, string description);
    }
}
