using BusinessObjects.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels.TourSegment
{
    public class TourSegmentCreateModel
    {
        public string? DestinationId { get; set; }
        public string? PackageTourId { get; set; }
        public EStatus? Status { get; set; }
    }
}
