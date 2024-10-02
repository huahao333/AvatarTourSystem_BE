using BusinessObjects.ViewModels.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels.PackageTourFlow
{
    public class FDestinationModel
    {
        [JsonIgnore]
        public string? DestinationId { get; set; }
        public string? DestinationName { get; set; }
        public string? CityId { get; set; }
        public List<FLocationModel> Locations { get; set; } = new List<FLocationModel>();
    }
}
