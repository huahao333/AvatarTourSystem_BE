using BusinessObjects.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels.TourSegment
{
    public class TourSegmentCreateModel
    {
        [FromForm(Name = "destination-id")]
        public string? DestinationId { get; set; }

        [FromForm(Name = "package-tour-id")]
        public string? PackageTourId { get; set; }

        [FromForm(Name = "status")]
        public EStatus? Status { get; set; }
    }
}
