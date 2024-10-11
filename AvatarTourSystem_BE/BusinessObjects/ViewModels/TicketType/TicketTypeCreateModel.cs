using BusinessObjects.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels.TicketType
{
    public class TicketTypeCreateModel
    {
      //  [FromForm(Name = "package-tour-id")]
        public string? PackageTourId { get; set; }

      //  [FromForm(Name = "ticket-type-name")]
        public string? TicketTypeName { get; set; }
        public int? MinBuyTicket { get; set; }

        //  [FromForm(Name = "status")]
        public EStatus? Status { get; set; }
    }
}
