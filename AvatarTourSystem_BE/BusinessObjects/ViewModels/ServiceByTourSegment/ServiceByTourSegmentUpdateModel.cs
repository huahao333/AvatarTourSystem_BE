using BusinessObjects.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels.ServiceByTourSegment
{
    public class ServiceByTourSegmentUpdateModel
    {
        [Required]
        [FromForm(Name = "sbts-id")]
        public Guid SBTSId { get; set; }
        [FromForm(Name = "tour-segment-id")]
        public string? TourSegmentId { get; set; }
        [FromForm(Name = "service-id")]
        public string? ServiceId { get; set; }
        [FromForm(Name = "status")]
        public EStatus? Status { get; set; }
    }
}
