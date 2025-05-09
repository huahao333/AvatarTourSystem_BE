﻿using BusinessObjects.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels.TransactionHistory
{
    public class TransactionHistoryCreateModel
    {
      //  [FromForm(Name = "user-id")]
        public string UserId { get; set; } = "";
        //  [FromForm(Name = "booking-id")]
        public string BookingId { get; set; } = "";
        //  [FromForm(Name = "order-id")]
        public string? OrderId { get; set; } = "";
        //   [FromForm(Name = "status")]
        public EStatus? Status { get; set; }
    }
    public class GetTransactionHistory
    {
        public string ZaloId { get; set;}
    }
}
