using BusinessObjects.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels.TicketType
{
    public class TicketTypeCreateModel
    {
        public string? PackageTourId { get; set; }
        public string? TicketTypeName { get; set; }
        public EStatus? Status { get; set; }
    }
}
