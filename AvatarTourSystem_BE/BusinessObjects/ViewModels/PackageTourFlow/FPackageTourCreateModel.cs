using BusinessObjects.Enums;
using BusinessObjects.ViewModels.Destination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels.PackageTourFlow
{
    public class FPackageTourCreateModel
    {
        public string? PackageTourName { get; set; }
        public float? PackageTourPrice { get; set; }
        public string? CityId { get; set; }
        public EStatus? Status { get; set; }

        // Danh sách các điểm đến
        //public List<FDestinationModel> Destinations { get; set; } = new List<FDestinationModel>();
        public List<FTicketTypeCreate> TicketTypesCreate { get; set; } = new List<FTicketTypeCreate>();


    }
}
