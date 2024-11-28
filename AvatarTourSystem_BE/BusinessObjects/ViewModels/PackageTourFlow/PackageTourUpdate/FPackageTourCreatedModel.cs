using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels.PackageTourFlow.PackageTourUpdate
{
    public class FPackageTourCreatedModel
    {
        [Required(ErrorMessage = "PackageTourName is required")]
        [StringLength(100, ErrorMessage = "PackageTourName can't be longer than 100 characters")]
        public string? PackageTourName { get; set; }
        public string? PackageTourImgURL { get; set; }

        public string? CityId { get; set; }
        [JsonIgnore]
        public DateTime? UpdateDate { get; set; }
        [JsonIgnore]
        public int? Status { get; set; }
        // Danh sách các điểm đến
        public List<FDestinationUpdateModel> Destinations { get; set; } = new List<FDestinationUpdateModel>();
        public List<FTicketTypeCreate> TicketTypesCreate { get; set; } = new List<FTicketTypeCreate>();

    }
}
