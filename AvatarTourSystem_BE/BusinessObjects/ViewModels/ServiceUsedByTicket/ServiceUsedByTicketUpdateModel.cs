using BusinessObjects.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels.ServiceUsedByTicket
{
    public class ServiceUsedByTicketUpdateModel
    {
        public Guid SUBTId { get; set; }
        public string? TicketId { get; set; }
        public string? ServiceId { get; set; }
        public EStatus? Status { get; set; }
    }
}
