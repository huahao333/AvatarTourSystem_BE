using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models
{
    public partial class BookingByRevenue
    {
        public string? BookingByRevenueId { get; set; }
        public string? RevenueId { get; set; }
        public string? BookingId { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? Status { get; set; }

        public virtual Revenue? Revenues { get; set; }
        public virtual Booking? Bookings { get; set; }
    }
}
