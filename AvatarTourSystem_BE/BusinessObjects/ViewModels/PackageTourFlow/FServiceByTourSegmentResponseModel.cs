using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels.PackageTourFlow
{
    public class FServiceByTourSegmentResponseModel
    {
        public string? SBTSId { get; set; }
        public string? TourSegmentId { get; set; }
        public string? ServiceId { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? Status { get; set; }

        public FServiceResponseModel? Service { get; set; }
        public FTourSegmentResponseModel? TourSegmentResponseModel { get; set; }
        // Optional: If you want to include Service details
    }
}
