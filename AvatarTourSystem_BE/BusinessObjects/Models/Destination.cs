using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models
{
    public partial class Destination
    {
        public Destination()
        {
            Locations = new HashSet<Location>();
            TourSegments = new HashSet<TourSegment>();
        }
        public string? DestinationId { get; set; }
        public string? CityId { get; set; }
        public string? DestinationName { get; set; }
        public string? DestinationAddress { get; set; }
        public string? DestinationImgUrl { get; set; }
        public string? DestinationHotline { get; set; }
        public string? DestinationGoogleMap { get; set; }
        public DateTime? DestinationOpeningDate { get; set; }
        public DateTime? DestinationClosingDate { get; set; }
        public DateTime? DestinationOpeningHours { get; set; }
        public DateTime? DestinationClosingHours { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? Status { get; set; }

        public virtual City? Cities { get; set; }
        public virtual ICollection<Location> Locations { get; set; }
        public virtual ICollection<TourSegment> TourSegments { get; set; }
    }
}
