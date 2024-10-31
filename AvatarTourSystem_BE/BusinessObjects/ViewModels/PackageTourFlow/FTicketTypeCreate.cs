using BusinessObjects.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels.PackageTourFlow
{
    public class FTicketTypeCreate
    {
        [JsonIgnore]
        public string? TicketTypeId { get; set; }
        [JsonIgnore]
        public string? PackageTourId { get; set; }
        [Required(ErrorMessage = "TicketTypeName is required")]
        [StringLength(50, ErrorMessage = "TicketTypeName can't be longer than 50 characters")]
        public string? TicketTypeName { get; set; }
        [Required(ErrorMessage = "Default Price is required")]
        public float? PriceDefault { get; set; }
        [Required(ErrorMessage = "Min Buy Ticket is required")]
        public int? MinBuyTicket { get; set; }
        public int Status { get; set; }
    }
}
