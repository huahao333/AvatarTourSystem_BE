using BusinessObjects.Enums;
using Microsoft.AspNetCore.Mvc;
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
        [FromForm(Name = "tour-segment-id")]
        public Guid TourSegmentId { get; set; }

        [FromForm(Name = "destination-id")]
        public string? DestinationId { get; set; }

        [FromForm(Name = "package-tour-id")]
        public string? PackageTourId { get; set; }

        [FromForm(Name = "status")]
        public EStatus? Status { get; set; }
    }
}
