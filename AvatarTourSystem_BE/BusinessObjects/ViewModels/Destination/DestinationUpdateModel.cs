using BusinessObjects.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels.Destination
{
    public class DestinationUpdateModel
    {
        [Required]
      //  [FromForm(Name = "destination-id")]
        public Guid DestinationId { get; set; }
     //   [FromForm(Name = "destination-name")]
        public string? DestinationName { get; set; }
     //   [FromForm(Name = "destination-img-url")]
        public string? DestinationImgUrl { get; set; }
        public string? DestinationAddress { get; set; }
        public string? DestinationHotline { get; set; }
        public string? DestinationGoogleMap { get; set; }
        public DateTime? DestinationOpeningHours { get; set; }
        public DateTime? DestinationClosingHours { get; set; }
        public DateTime? DestinationOpeningDate { get; set; }
        public DateTime? DestinationClosingDate { get; set; }
        //  [FromForm(Name = "staus")]
        public EStatus? Status { get; set; }
    }
}
