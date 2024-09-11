using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models
{
    public partial class ServiceByTourSegment
    {
        public string? SBTSId { get; set; }
        public string? TourSegmentId { get; set; }
        public string? ServiceId { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? Status { get; set; }

        public virtual TourSegment? TourSegments { get; set; }
        public virtual Service? Services { get; set; }
    }
}
