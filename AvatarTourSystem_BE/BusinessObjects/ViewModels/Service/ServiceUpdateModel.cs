using BusinessObjects.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels.Service
{
    public class ServiceUpdateModel
    {
        [Required]
     //   [FromForm(Name = "service-id")]
        public Guid ServiceId { get; set; }

      //  [FromForm(Name = "service-name")]
        public string? ServiceName { get; set; }

     //   [FromForm(Name = "service-price")]
        public float? ServicePrice { get; set; }
    //    [FromForm(Name = "service-img-url")]
        public string? ServiceImgUrl { get; set; }
      //  [FromForm(Name = "service-type-id")]
        public string? ServiceTypeId { get; set; }

     //   [FromForm(Name = "location-id")]
        public string? LocationId { get; set; }

     //   [FromForm(Name = "supplier-id")]
        public string? SupplierId { get; set; }

     //   [FromForm(Name = "status")]
        public EStatus? Status { get; set; }
    }
}
