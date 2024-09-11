using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models
{
    public partial class Feedback
    {
        public string? FeedbackId { get; set; }
        public string? UserId { get; set; }
        public string? BookingId { get; set; }
        public string? FeedbackMsg { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? Status { get; set; }

        public virtual Account? Accounts { get; set; }
        public virtual Booking? Bookings { get; set; }
    }
}
