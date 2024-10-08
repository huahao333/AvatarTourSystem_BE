using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels.Location
{
    public class LocationModel
    {
     //   [FromForm(Name = "location-id")]
        public string? LocationId { get; set; }
     //   [FromForm(Name = "location-name")]
        public string? LocationName { get; set; }
      //  [FromForm(Name = "location-type")]
        public int? LocationType { get; set; }
      //  [FromForm(Name = "location-img-url")]
        public string? LocationImgUrl { get; set; }
      //  [FromForm(Name = "destination-id")]
        public string? DestinationId { get; set; }
      //  [FromForm(Name = "create-date")]
        public DateTime? CreateDate { get; set; }
     //   [FromForm(Name = "update-date")]
        public DateTime? UpdateDate { get; set; }
      //  [FromForm(Name = "status")]
        public int? Status { get; set; }

    }
}
