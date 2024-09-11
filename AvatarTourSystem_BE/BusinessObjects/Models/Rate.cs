using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models
{
    public partial class Rate
    {
        public string? RateId { get; set; }
        public string? UserId { get; set; }
        public string? BookingId { get; set; }
        public int? RateStar { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? Status { get; set; }

        public virtual Account? Accounts { get; set; }
        public virtual Booking? Bookings { get; set; }
    }
}
