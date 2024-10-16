using BusinessObjects.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels.DailyTour
{
    public class DailyTourCreateModel
    {
       
      //  [FromForm(Name = "package-tour-id")]
        public string? PackageTourId { get; set; } = "";
        //   [FromForm(Name = "daily-tour-name")]
        public string? DailyTourName { get; set; } = "";
        //  [FromForm(Name = "description")]
        public string? Description { get; set; } = "";
        //  [FromForm(Name = "daily-tour-price")]
        public float? DailyTourPrice { get; set; } = 0;
      //  [FromForm(Name = "img-url")]
        public string? ImgUrl { get; set; } = "";
        //  [FromForm(Name = "start-date")]
        public DateTime? StartDate { get; set; }
      //  [FromForm(Name = "end-date")]
        public DateTime? EndDate { get; set; }
      //  [FromForm(Name = "discount")]
        public int? Discount { get; set; } = 0;
      //  [FromForm(Name = "status")]
        public EStatus? Status { get; set; }
    }
}
