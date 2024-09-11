using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models
{
    public partial class PointOfInterest
    {
        public string? PointId { get; set; }
        public string? PointName { get; set; }
        public string? LocationId { get; set; }
        public string? POITypeId { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? Status { get; set; }

        public virtual POIType? POITypes { get; set; }
        public virtual Location? Locations { get; set; }
    }
}
