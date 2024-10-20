using Microsoft.AspNetCore.Http;
using Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IVNPayService
    {
        Task<APIGenericResponseModel<string>> CreatePaymentRequestAsync(string bookingId);
        Task<APIResponseModel> ConfirmPaymentAsync(IQueryCollection queryString);
    }
}
