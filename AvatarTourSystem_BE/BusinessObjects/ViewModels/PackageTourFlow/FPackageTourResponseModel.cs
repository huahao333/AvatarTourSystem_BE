using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels.PackageTourFlow
{
    public class FPackageTourResponseModel
    {
        public string PackageTourId { get; set; }
        public string PackageTourName { get; set; }
        public float PackageTourPrice { get; set; }
        public string PackageTourImgUrl { get; set; }
        public List<FDestinationResponseModel> Destinations { get; set; }
        public List<FTicketType> TicketTypes { get; set; }

    }
}
