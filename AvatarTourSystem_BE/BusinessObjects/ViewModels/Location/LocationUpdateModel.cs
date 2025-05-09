﻿using BusinessObjects.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels.Location
{
    public class LocationUpdateModel
    {
      //  [FromForm(Name = "location-id")]
        public Guid LocationId { get; set; }
     //  [FromForm(Name = "location-name")]
        public string? LocationName { get; set; }
     //   [FromForm(Name = "location-type")]
      //  [FromForm(Name = "location-img-url")]
        public string? LocationImgUrl { get; set; }
     //   [FromForm(Name = "destination-id")]
        public string? DestinationId { get; set; }
        public DateTime? LocationOpeningHours { get; set; }
        public DateTime? LocationClosingHours { get; set; }
        //   [FromForm(Name = "status")]
        public EStatus? Status { get; set; }
    }

    public class LocationUpdateViewModel
    {
        public string LocationId { get; set; }
        public string? LocationName { get; set; }
        public string? LocationImgUrl { get; set; }
        public string? LocationHotline { get; set; }
        public string? LocationAddress { get; set; }
        public string? DestinationId { get; set; }
        public DateTime? LocationOpeningHours { get; set; }
        public DateTime? LocationClosingHours { get; set; }
        public int Status { get; set; }

    }
}
