using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models
{
    public partial class ServiceUsedByTicket
    {
        public string? SUBTId { get; set; }
        public string? TicketId { get; set; }
        public string? ServiceId { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? Status { get; set; }

        public virtual Ticket? Tickets { get; set; }
        public virtual Service? Services { get; set; }
    }
}
