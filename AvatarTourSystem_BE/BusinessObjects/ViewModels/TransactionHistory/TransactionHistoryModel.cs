using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels.TransactionHistory
{
    public class TransactionHistoryModel
    {
      //  [FromForm(Name = "transaction-id")]
        public string? TransactionId { get; set; }
     //   [FromForm(Name = "user-id")]
        public string? UserId { get; set; }
     //   [FromForm(Name = "booking-id")]
        public string? BookingId { get; set; }
     //   [FromForm(Name = "order-id")]
        public string? OrderId { get; set; }
      //  [FromForm(Name = "create-date")]
        public DateTime? CreateDate { get; set; }
     //   [FromForm(Name = "update-date")]
        public DateTime? UpdateDate { get; set; }
     //   [FromForm(Name = "status")]
        public int? Status { get; set; }

    }
}
