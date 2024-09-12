using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models
{
    public partial class DailyTour
    {
        public DailyTour()
        {
            Bookings = new HashSet<Booking>();
            DailyTickets = new HashSet<DailyTicket>();
        }
        public string? DailyTourId { get; set; }
        public string? PackageTourId { get; set; }
        public string? DailyTourName { get; set; }
        public string? Description { get; set; }
        public float? DailyTourPrice { get; set; }
        public string? ImgUrl { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? Discount { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? Status { get; set; }

        public virtual PackageTour? PackageTours { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }
        public virtual ICollection<DailyTicket> DailyTickets { get; set; }
    }
}
