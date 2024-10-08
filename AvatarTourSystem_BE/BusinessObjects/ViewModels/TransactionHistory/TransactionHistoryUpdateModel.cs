using BusinessObjects.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels.TransactionHistory
{
    public class TransactionHistoryUpdateModel
    {
     //   [FromForm(Name = "transaction-history-id")]
        public Guid TransactionHistoryId { get; set; }
     //   [FromForm(Name = "user-id")]
        public string UserId { get; set; }
     //   [FromForm(Name = "booking-id")]
        public string BookingId { get; set; }
     //   [FromForm(Name = "order-id")]
        public string? OrderId { get; set; }
     //   [FromForm(Name = "status")]
        public EStatus? Status { get; set; }
    }
}
