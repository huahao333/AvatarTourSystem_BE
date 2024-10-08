using BusinessObjects.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels.Ticket
{
    public class TicketCreateModel
    {
     //   [FromForm(Name = "booking-id")]
        public string? BookingId { get; set; }

      //  [FromForm(Name = "ticket-type-id")]
        public string? TicketTypeId { get; set; }

     //   [FromForm(Name = "ticket-name")]
        public string? TicketName { get; set; }

      //  [FromForm(Name = "quantity")]
        public int? Quantity { get; set; }

     //   [FromForm(Name = "qr")]
        public string? QR { get; set; }

      //  [FromForm(Name = "price")]
        public float? Price { get; set; }

      //  [FromForm(Name = "status")]
        public EStatus? Status { get; set; }
    }
}
