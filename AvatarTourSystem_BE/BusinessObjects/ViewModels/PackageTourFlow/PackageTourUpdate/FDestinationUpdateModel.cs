using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels.PackageTourFlow.PackageTourUpdate
{
    public class FDestinationUpdateModel
    {

        [Required(ErrorMessage = "DestinationId is required")]
        public string? DestinationId { get; set; }
        [JsonIgnore]
        public string? CityId { get; set; }

        public int? Status { get; set; }
        public List<FLocationUpdateModel> Locations { get; set; } = new List<FLocationUpdateModel>();
    }
}
