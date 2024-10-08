using BusinessObjects.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels.POI
{
    public class POICreateModel
    {
      //  [FromForm(Name = "point-name")]
        public string PointName { get; set; }
      //  [FromForm(Name = "location-id")]
        public string? LocationId { get; set; }
       // [FromForm(Name = "poi-type-id")]
        public string? POITypeId { get; set; }
      //  [FromForm(Name = "create-date")]
        public EStatus? Status { get; set; }

    }
}
