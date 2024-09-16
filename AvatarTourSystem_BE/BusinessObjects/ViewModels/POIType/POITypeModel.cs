using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels.POIType
{
    public class POITypeModel
    {
        [FromForm(Name = "poi-type-id")]
        public string? POITypeId { get; set; }
        [FromForm(Name = "poi-type-name")]
        public string? POITypeName { get; set; }
        [FromForm(Name = "create-date")]
        public DateTime? CreateDate { get; set; }
        [FromForm(Name = "update-date")]
        public DateTime? UpdateDate { get; set; }
        [FromForm(Name = "status")]
        public int? Status { get; set; }
    }
}
