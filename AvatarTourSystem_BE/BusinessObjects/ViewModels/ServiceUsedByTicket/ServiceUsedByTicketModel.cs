using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels.ServiceUsedByTicket
{
    public class ServiceUsedByTicketModel
    {
        public string? SUBTId { get; set; }
        public string? TicketId { get; set; }
        public string? ServiceId { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? Status { get; set; }
    }
}
