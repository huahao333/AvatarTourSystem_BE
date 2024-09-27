using BusinessObjects.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels.ServiceUsedByTicket
{
    public class ServiceUsedByTicketUpdateModel
    {
        [Required]
        [FromForm(Name = "subt-id")]
        public Guid SUBTId { get; set; }

        [FromForm(Name = "ticket-id")]
        public string? TicketId { get; set; }

        [FromForm(Name = "service-id")]
        public string? ServiceId { get; set; }

        [FromForm(Name = "status")]
        public EStatus? Status { get; set; }
    }
}
