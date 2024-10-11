using BusinessObjects.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels.DailyTicket
{
    public class DailyTicketUpdateModel
    {
        [Required]
      //  [FromForm(Name = "daily-ticket-id")]
        public Guid DailyTicketId { get; set; }

      //  [FromForm(Name = "ticket-type-id")]
        public string? TicketTypeId { get; set; }

     //   [FromForm(Name = "daily-tour-id")]
        public string? DailyTourId { get; set; }

     //   [FromForm(Name = "capacity")]
        public int? Capacity { get; set; }
     //   [FromForm(Name = "daily-ticket-price")]
        public float? DailyTicketPrice { get; set; }
      //  [FromForm(Name = "status")]
        public EStatus? Status { get; set; }
    }
}
