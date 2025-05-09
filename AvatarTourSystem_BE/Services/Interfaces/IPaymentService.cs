﻿using Services.Common;
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
    }
}
