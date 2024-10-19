using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels.Booking
{
    public class BookingModel
    {
        public string? BookingId { get; set; }
        public string? UserId { get; set; }
        public DateTime? BookingDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string? DailyTourId { get; set; }
        public string? PaymentId { get; set; }
        public float? TotalPrice { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? Status { get; set; }
    }
}
