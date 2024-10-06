using BusinessObjects.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels.PackageTourFlow
{
    public class FServiceByTourSegment
    {
        public string? SBTSId { get; set; }
        public string? TourSegmentId { get; set; }
        public string? ServiceId { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public EStatus Status { get; set; }
    }
}
