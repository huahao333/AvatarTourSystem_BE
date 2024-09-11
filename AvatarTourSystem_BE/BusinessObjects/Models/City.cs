using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models
{
    public partial class City
    {
        public City()
        {
            Destinations = new HashSet<Destination>();
            PackageTours = new HashSet<PackageTour>();
        }
        public string? CityId { get; set; }
        public string? CityName { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? Status { get; set; }

        public virtual ICollection<Destination> Destinations { get; set; }
        public virtual ICollection<PackageTour> PackageTours { get; set; }
    }
}
