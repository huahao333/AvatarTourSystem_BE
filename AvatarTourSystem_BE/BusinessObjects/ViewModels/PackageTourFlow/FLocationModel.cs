using BusinessObjects.ViewModels.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels.PackageTourFlow
{
    public class FLocationModel
    {
        [JsonIgnore]
        public string? LocationId { get; set; }
        public string? LocationName { get; set; }
        public int? LocationType { get; set; }
        public List<FServiceModel> Services { get; set; } = new List<FServiceModel>();
    }
}
