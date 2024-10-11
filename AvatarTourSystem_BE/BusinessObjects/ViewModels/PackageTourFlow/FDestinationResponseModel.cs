using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels.PackageTourFlow
{
    public class FDestinationResponseModel
    {
        public string DestinationId { get; set; }
        public string DestinationName { get; set; }
        public List<FLocationResponseModel> Locations { get; set; }
    }
}
