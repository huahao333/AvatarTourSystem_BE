using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models
{
    public partial class PackageTour
    {
        public PackageTour()
        {
            TourSegments = new HashSet<TourSegment>();
            DailyTours = new HashSet<DailyTour>();
            TicketTypes = new HashSet<TicketType>();
        }
        public string? PackageTourId { get; set; }
        public string? CityId { get; set; }
        public string? PackageTourName { get; set; }
        public float? PackageTourPrice { get; set; }
        public string? PackageTourImgUrl{ get; set; }

        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? Status { get; set; }

        public virtual City? Cities { get; set; }
        public virtual ICollection<TourSegment> TourSegments { get; set; }
        public virtual ICollection<DailyTour> DailyTours { get; set; }
        public virtual ICollection<TicketType> TicketTypes { get; set; }
    }
}
