using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels.PackageTourFlow.PackageTourUpdate
{
    public class FLocationUpdateModel
    {
        public string? LocationId { get; set; }

      

        [Range(0, 10, ErrorMessage = "LocationType must be between 0 and 10")]
        public int? Status { get; set; }
        public List<FServiceUpdateModel> Services { get; set; } = new List<FServiceUpdateModel>();
        public List<FPOIUpdateModel> POI { get; set; } = new List<FPOIUpdateModel>();
    }
}
