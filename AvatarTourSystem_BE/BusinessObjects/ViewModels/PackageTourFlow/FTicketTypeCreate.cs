using BusinessObjects.Enums;
using System;
using System.Collections.Generic;
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
        public string? TicketTypeName { get; set; }     
        public EStatus? Status { get; set; }
    }
}
