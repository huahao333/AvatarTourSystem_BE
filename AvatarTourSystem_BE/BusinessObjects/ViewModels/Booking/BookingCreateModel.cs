using BusinessObjects.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels.Booking
{
    public class BookingCreateModel
    {
        public string? UserId { get; set; }
        public string? DailyTourId { get; set; }
        public string? PaymentId { get; set; }
        public float? TotalPrice { get; set; }
        public EStatus? Status { get; set; }
    }
}
