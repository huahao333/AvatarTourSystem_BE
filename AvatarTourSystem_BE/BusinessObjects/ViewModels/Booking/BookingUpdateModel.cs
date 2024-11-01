using BusinessObjects.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels.Booking
{
    public class BookingUpdateModel
    {
        [Required]
      //  [FromForm(Name = "booking-id")]
        public Guid BookingId { get; set; }

     //   [FromForm(Name = "user-id")]
        public string? UserId { get; set; }

     //   [FromForm(Name = "daily-tour-id")]
        public string? DailyTourId { get; set; }

      //  [FromForm(Name = "payment-id")]
        public string? PaymentId { get; set; }

      //  [FromForm(Name = "total-price")]
        public float? TotalPrice { get; set; }

      //  [FromForm(Name = "status")]
        public EStatus? Status { get; set; }
    }

    public class BookingPhoneNumberShareTicket
    {
        public string TicketId { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class TicketUsageViewModel
    {
        public string TicketId { get; set; } 
        public string MobileDestinationId { get; set; }
        public string DailyTourId { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public List<ListDestinationModel> Destination { get; set; }
    }
    public class ListDestinationModel
    {
        public string DestinationIds { get; set; }
    }
}
