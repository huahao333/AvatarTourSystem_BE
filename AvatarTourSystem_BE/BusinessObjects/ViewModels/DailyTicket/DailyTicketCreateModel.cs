using BusinessObjects.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels.DailyTicket
{
    public class DailyTicketCreateModel
    {
      //  [FromForm(Name = "ticket-type-id")]
        public string? TicketTypeId { get; set; } = "";

        //  [FromForm(Name = "daily-tour-id")]
        public string? DailyTourId { get; set; } = "";

        //  [FromForm(Name = "capacity")]
        public int? Capacity { get; set; } = 0;
        //   [FromForm(Name = "daily-ticket-price")]
        public float? DailyTicketPrice { get; set; } = 0;
      //  [FromForm(Name = "status")]
        public EStatus? Status { get; set; }
    }
}
