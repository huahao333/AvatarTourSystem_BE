using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models
{
    public partial class POIType
    {
        public POIType()
        {
            PointOfInterests = new HashSet<PointOfInterest>();
        }
        public string? POITypeId { get; set; }
        public string? POITypeName { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? Status { get; set; }

        public virtual ICollection<PointOfInterest> PointOfInterests { get; set; }
    }
}
