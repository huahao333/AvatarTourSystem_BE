using BusinessObjects.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels.TourSegment
{
    public class TourSegmentUpdateModel
    {
        [Required]
        public Guid? TourSegmentId { get; set; }
        public string? DestinationId { get; set; }
        public string? PackageTourId { get; set; }
        public EStatus? Status { get; set; }
    }
}
