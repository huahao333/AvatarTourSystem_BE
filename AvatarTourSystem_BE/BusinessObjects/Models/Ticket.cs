using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models
{
    public partial class Ticket
    {
        public Ticket()
        {
            ServiceUsedByTickets = new HashSet<ServiceUsedByTicket>();
        }
        public string? TicketId { get; set; }
        public string? BookingId { get; set; }
        public string? DailyTicketId { get; set; }
        public string? TicketName { get; set; }
        public int? Quantity { get; set; }
        public string? QRImgUrl { get; set; }
        public string? PhoneNumberReference { get; set; }
        public float? Price { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? Status { get; set; }

        public virtual Booking? Bookings { get; set; }
        public virtual DailyTicketType? DailyTicketType { get; set; }
        public virtual ICollection<ServiceUsedByTicket> ServiceUsedByTickets { get; set; }
    }
}
