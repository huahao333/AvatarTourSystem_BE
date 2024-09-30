using BusinessObjects.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels.DailyTour
{
    public class DailyTourFlowModel
    {
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
        [FromForm(Name = "end-date")]
        public DateTime? EndDate { get; set; }
        [FromForm(Name = "discount")]
        public int? Discount { get; set; }

        [FromForm(Name = "ticket-type-id-adult")]
        public string? TicketTypeIdAdult { get; set; }
        [FromForm(Name = "capacity-by-adult")]
        public int? CapacityByAdult { get; set; }
        [FromForm(Name = "price-by-adult")]
        public float? PriceByAdult { get; set; }

        [FromForm(Name = "ticket-type-id-children")]
        public string? TicketTypeIdChildren { get; set; }
        [FromForm(Name = "capacity-by-children")]
        public int? CapacityByChildren { get; set; }
        [FromForm(Name = "price-by-children")]
        public float? PriceByChildren { get; set; }
    }
}
