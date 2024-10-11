using BusinessObjects.ViewModels.Location;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [Required(ErrorMessage = "DestinationName is required")]
        [StringLength(100, ErrorMessage = "DestinationName can't be longer than 100 characters")]
        public string? DestinationName { get; set; } 
        [JsonIgnore]
        public string? CityId { get; set; } 
        public List<FLocationModel> Locations { get; set; } = new List<FLocationModel>();
    }
}
