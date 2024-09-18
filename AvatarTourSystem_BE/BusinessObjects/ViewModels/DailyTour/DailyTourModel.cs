using BusinessObjects.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels.DailyTour
{
    public class DailyTourModel
    {
        [FromForm(Name = "daily-tour-id")]
        public string? DailyTourId { get; set; }
        [FromForm(Name = "package-tour-id")]
        public string? PackageTourId { get; set; }
        [FromForm(Name = "daily-tour-name")]
        public string? DailyTourName { get; set; }
        [FromForm(Name = "description")]
        public string? Description { get; set; }
        [FromForm(Name = "daily-tour-price")]
        public float? DailyTourPrice { get; set; }
        [FromForm(Name = "img-url")]
        public string? ImgUrl { get; set; }
        [FromForm(Name = "start-date")]
        public DateTime? StartDate { get; set; }
        [FromForm(Name = "create-date")]
        public DateTime? CreateDate { get; set; }
        [FromForm(Name = "end-date")]
        public DateTime? EndDate { get; set; }
        [FromForm(Name = "discount")]
        public int? Discount { get; set; }
        [FromForm(Name = "update-date")]
        public DateTime? UpdateDate { get; set; }
        [FromForm(Name = "status")]
        public EStatus? Status { get; set; }
    }
}
