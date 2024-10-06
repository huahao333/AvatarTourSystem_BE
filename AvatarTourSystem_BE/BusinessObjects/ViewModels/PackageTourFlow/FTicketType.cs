using BusinessObjects.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels.PackageTourFlow
{
    public class FTicketType
    {
    
        public string? TicketTypeId { get; set; }
        public string? PackageTourId { get; set; }
        public string? TicketTypeName { get; set; }
        public string? Status { get; set; }
    }
}
