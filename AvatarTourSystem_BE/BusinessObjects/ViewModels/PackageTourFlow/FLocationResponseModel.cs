using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels.PackageTourFlow
{
    public class FLocationResponseModel
    {
        public string LocationId { get; set; }
        public string LocationName { get; set; }
        public List<FServiceResponseModel> Services { get; set; }
    }
}
