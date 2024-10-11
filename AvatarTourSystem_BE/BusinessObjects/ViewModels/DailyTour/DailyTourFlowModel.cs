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
        public string? PackageTourId { get; set; }
        public string? DailyTourName { get; set; }
        public string? Description { get; set; }
        public float? DailyTourPrice { get; set; }
        public string? ImgUrl { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? Discount { get; set; }
        public string? TicketTypeIdAdult { get; set; }
        public int? CapacityByAdult { get; set; }
        public float? PriceByAdult { get; set; }
        public string? TicketTypeIdChildren { get; set; }
        public int? CapacityByChildren { get; set; }
        public float? PriceByChildren { get; set; }
    }
}
