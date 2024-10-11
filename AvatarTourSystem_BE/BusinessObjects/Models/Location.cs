using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models
{
    public partial class Location
    {
        public Location()
        {
            PointOfInterests = new HashSet<PointOfInterest>();
            Services = new HashSet<Service>();
        }
        public string? LocationId { get; set; }
        public string? LocationName { get; set; }
        //public int? LocationType { get; set; }
        public string? LocationImgUrl { get; set; }
        public string? LocationHotline { get; set; }
        public string? LocationGoogleMap { get; set; }
        public string? DestinationId { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? Status { get; set; }

        public virtual Destination? Destinations { get; set; }
        public virtual ICollection<PointOfInterest> PointOfInterests { get; set; }
        public virtual ICollection<Service> Services { get; set; }
    }
}
