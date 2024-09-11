using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models
{
    public partial class TransactionsHistory
    {
        public string? TransactionId { get; set; }
        public string? UserId { get; set; }
        public string? BookingId { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? Status { get; set; }

        public virtual Account? Accounts { get; set; }
        public virtual Booking? Bookings { get; set; }
    }
}
