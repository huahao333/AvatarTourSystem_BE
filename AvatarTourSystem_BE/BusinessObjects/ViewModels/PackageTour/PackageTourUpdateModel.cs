using BusinessObjects.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels.PackageTour
{
    public class PackageTourUpdateModel
    {
        [Required]
      //  [FromForm(Name = "package-tour-id")]
        public Guid PackageTourId { get; set; }

     //   [FromForm(Name = "city-id")]
        public string? CityId { get; set; }

     //   [FromForm(Name = "package-tour-name")]
        public string? PackageTourName { get; set; }

     //   [FromForm(Name = "package-tour-price")]
        public float? PackageTourPrice { get; set; }
     //   [FromForm(Name = "package-img-url")]
        public string? PackageTourImgUrl { get; set; }
     //   [FromForm(Name = "Status")]
        public EStatus? Status { get; set; }
    }
}
