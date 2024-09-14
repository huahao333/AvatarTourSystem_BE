using BusinessObjects.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels.DailyTicket
{
    public class DailyTicketCreateModel
    {
        public string? TicketTypeId { get; set; }
        public string? DailyTourId { get; set; }
        public int? Capacity { get; set; }
        public EStatus? Status { get; set; }
    }
}
