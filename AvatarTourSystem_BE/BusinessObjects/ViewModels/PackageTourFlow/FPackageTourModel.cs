using BusinessObjects.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels.PackageTourFlow
{
    public class FPackageTourModel
    {
        public string? PackageTourId { get; set; }
        public string? PackageTourName { get; set; }
        public float? PackageTourPrice { get; set; }
        public string? CityId { get; set; }
        public DateTime? createDate { get; set; }
        public DateTime? updateDate { get; set; }
        public string? Status { get; set; }
        public List<FTicketType> TicketTypes { get; set; } = new List<FTicketType>();
    }
}
