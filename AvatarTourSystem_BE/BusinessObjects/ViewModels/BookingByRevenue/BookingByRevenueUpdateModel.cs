using BusinessObjects.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels.BookingByRevenue
{
    public class BookingByRevenueUpdateModel
    {
        [Required]
        [FromForm(Name = "booking-by-revenue-id")]
        public Guid BookingByRevenueId { get; set; }

        [FromForm(Name = "revenue-id")]
        public string? RevenueId { get; set; }

        [FromForm(Name = "booking-id")]
        public string? BookingId { get; set; }

        [FromForm(Name = "status")]
        public EStatus? Status { get; set; }
    }
}
