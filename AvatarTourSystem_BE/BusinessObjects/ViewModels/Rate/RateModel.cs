using BusinessObjects.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels.Rate
{
    public class RateModel
    {
      //  [FromForm(Name = "rate-id")]
        public string? RateId { get; set; }
    //    [FromForm(Name = "user-id")]
        public string? UserId { get; set; }
     //   [FromForm(Name = "booking-id")]
        public string? BookingId { get; set; }
      //  [FromForm(Name = "rate-star")]
        public int? RateStar { get; set; }
     //   [FromForm(Name = "create-date")]
        public DateTime? CreateDate { get; set; }
     //   [FromForm(Name = "update-date")]
        public DateTime? UpdateDate { get; set; }
     //   [FromForm(Name = "status")]
        public EStatus? Status { get; set; }
    }
}
