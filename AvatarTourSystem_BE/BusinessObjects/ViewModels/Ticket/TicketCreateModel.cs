using BusinessObjects.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels.Ticket
{
    public class TicketCreateModel
    {
        public string? BookingId { get; set; }
        public string? TicketTypeId { get; set; }
        public string? TicketName { get; set; }
        public int? Quantity { get; set; }
        public string? QR { get; set; }
        public float? Price { get; set; }
        public EStatus? Status { get; set; }
    }
}
