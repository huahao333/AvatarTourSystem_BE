using BusinessObjects.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels.Booking
{
    public class BookingCreateModel
    {
      //  [FromForm(Name = "user-id")]
        public string? UserId { get; set; } = "";

      //  [FromForm(Name = "daily-tour-id")]
        public string? DailyTourId { get; set; } = "";

        //   [FromForm(Name = "payment-id")]
        public string? PaymentId { get; set; } = "";

        //  [FromForm(Name = "total-price")]
        public float? TotalPrice { get; set; } = 0;

      //  [FromForm(Name = "status")]
        public EStatus? Status { get; set; }
    }
}
