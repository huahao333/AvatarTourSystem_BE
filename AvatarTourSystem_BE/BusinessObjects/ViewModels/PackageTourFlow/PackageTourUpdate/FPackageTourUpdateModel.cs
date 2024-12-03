using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels.PackageTourFlow.PackageTourUpdate
{
    public class FPackageTourUpdateModel
    {
        public Guid PackageTourId { get; set; }
        public string? PackageTourName { get; set; }
        public string? CityId { get; set; }
        [Required(ErrorMessage = "Package Price is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public float? PackageTourPrice { get; set; }
        public string? PackageTourImgURL { get; set; }
        [JsonIgnore]
        public DateTime? UpdateDate { get; set; }
        public int? Status { get; set; }
        // Danh sách các điểm đến
        public List<FDestinationUpdateModel> Destinations { get; set; } = new List<FDestinationUpdateModel>();
        public List<FTicketTypeUpdateModel> TicketType { get; set; } = new List<FTicketTypeUpdateModel>();
    }
}
