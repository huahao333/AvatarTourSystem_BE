using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models
{
    public partial class PaymentMethod
    {
        public PaymentMethod()
        {
            Bookings = new HashSet<Booking>();
        }

        public string? PaymentId { get; set; }
        public string? PaymentType { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? Status { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; }
    }
}
