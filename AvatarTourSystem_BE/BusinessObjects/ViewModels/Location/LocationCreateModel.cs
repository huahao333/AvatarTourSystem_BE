using BusinessObjects.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels.Location
{
    public class LocationCreateModel
    {
        
     //   [FromForm(Name = "location-name")]
        public string? LocationName { get; set; }
     //   [FromForm(Name = "location-type")]
     //   [FromForm(Name = "location-img-url")]
        public string? LocationImgUrl { get; set; }
     //   [FromForm(Name = "destination-id")]
        public string? DestinationId { get; set; }
     //   [FromForm(Name = "status")]
        public EStatus? Status { get; set; }
    }
}
