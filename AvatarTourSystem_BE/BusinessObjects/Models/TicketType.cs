using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models
{
    public partial class TicketType
    {
        public TicketType()
        {
            DailyTickets = new HashSet<DailyTicket>();
            Tickets = new HashSet<Ticket>();
        }
        public string? TicketTypeId { get; set; }
        public string? PackageTourId { get; set; }
        public string? TicketTypeName { get; set; }
        public int? MinBuyTicket { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? Status { get; set; }

        public virtual ICollection<DailyTicket> DailyTickets { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }
        public virtual PackageTour? PackageTours { get; set; }
    }
}
