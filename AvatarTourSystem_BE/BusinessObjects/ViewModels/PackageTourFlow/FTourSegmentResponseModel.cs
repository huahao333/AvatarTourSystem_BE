using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels.PackageTourFlow
{
    public class FTourSegmentResponseModel
    {
        public string? TourSegmentId { get; set; }
        public string? DestinationId { get; set; }
        public string? PackageTourId { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? Status { get; set; }

        //public ICollection<FServiceByTourSegmentResponseModel> ServiceByTourSegments { get; set; } = new List<FServiceByTourSegmentResponseModel>();
    }
}
