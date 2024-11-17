using BusinessObjects.Enums;
using BusinessObjects.ViewModels.Booking;
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
        public string? PackageTourId { get; set; } = "";
        public string? DailyTourName { get; set; } = "";
        public string? Description { get; set; } = "";
        public float? DailyTourPrice { get; set; } = 0;
        public string? ImgUrl { get; set; } = "";
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? Discount { get; set; } = 0;
        public string? TicketTypeIdAdult { get; set; } = "";
        public int? CapacityByAdult { get; set; } = 0;
        public float? PriceByAdult { get; set; } = 0;
        public string? TicketTypeIdChildren { get; set; } = "";
        public int? CapacityByChildren { get; set; } = 0;
        public float? PriceByChildren { get; set; } = 0;
    }

    public class DailyToursFlowModel
    {
        public string? PackageTourId { get; set; } = "";
        public string? DailyTourName { get; set; } = "";
        public string? Description { get; set; } = "";
        public float? DailyTourPrice { get; set; } = 0;
        public string? ImgUrl { get; set; } = "";
        public DateTime? ExpirationDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? Discount { get; set; } = 0;
        public List<DailyTicketTypes> DailyTicketTypes { get; set; }
    }
    public class DailyTicketTypes
    {
        public string TicketTypeId { get; set; }
        public int? Capacity { get; set; }
        public float? Price { get; set; }
    }


    public class UpdateDailyTourFlowModel
    {
        public string DailyTourId { get; set; } 
        public string? PackageTourId { get; set; } = "";
        public string? DailyTourName { get; set; } = "";
        public string? Description { get; set; } = "";
        public float? DailyTourPrice { get; set; } = 0;
        public string? ImgUrl { get; set; } = "";
        public DateTime? ExpirationDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? Discount { get; set; } = 0;
        public List<UpdateDailyTicketTypeModel> DailyTicketTypes { get; set; }
    }

    public class UpdateDailyTicketTypeModel
    {
        public string TicketTypeId { get; set; }
        public int? Capacity { get; set; }
        public float? Price { get; set; }
    }
}
