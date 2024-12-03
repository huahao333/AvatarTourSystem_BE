using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels.POI
{
    public class POIModel
    {
      //  [FromForm(Name = "point-id")]
        public string? PointId { get; set; }
     //   [FromForm(Name = "point-name")]
        public string? PointName { get; set; }
      //  [FromForm(Name = "location-id")]
        public string? LocationId { get; set; }
        //  [FromForm(Name = "poi-type-id")]
        //  [FromForm(Name = "create-date")]
        [JsonIgnore]
        public DateTime? CreateDate { get; set; }
        [JsonIgnore]
      //  [FromForm(Name = "update-date")]
        public DateTime? UpdateDate { get; set; }
      //  [FromForm(Name = "status")]
        public int? Status { get; set; }
    }
}
