using BusinessObjects.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels.POIType
{
    public class POITypeUpdateModel
    {
        [Required]
        [FromForm(Name = "poi-type-id")]
        public Guid POITypeId { get; set; }
        [FromForm(Name = "poi-type-name")]
        public string? POITypeName { get; set; }
        [FromForm(Name = "status")]
        public EStatus? Status { get; set; }
    }
}
