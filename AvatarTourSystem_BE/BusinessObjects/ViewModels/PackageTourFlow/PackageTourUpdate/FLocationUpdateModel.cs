using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels.PackageTourFlow.PackageTourUpdate
{
    public class FLocationUpdateModel
    {
        public string? LocationId { get; set; }

      

        public int? Status { get; set; }
        public List<FServiceUpdateModel> Services { get; set; } = new List<FServiceUpdateModel>();
    }
}
