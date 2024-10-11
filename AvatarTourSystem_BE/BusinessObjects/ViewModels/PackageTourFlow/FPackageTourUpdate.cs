using BusinessObjects.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels.PackageTourFlow
{
    public class FPackageTourUpdate
    {
        public Guid PackageTourId { get; set; }
        [StringLength(100, ErrorMessage = "PackageTourName can't be longer than 100 characters")]
        public string PackageTourName { get; set; }

        public string? CityId { get; set; } // Không bắt buộc, nhưng có thể thêm kiểm tra nếu cần

        [JsonIgnore]
        public float PackageTourPrice { get; set; }

        [Url(ErrorMessage = "PackageTourImgUrl must be a valid URL")]
        public string? PackageTourImgUrl { get; set; }

        [JsonIgnore]
        public DateTime? UpdateDate { get; set; }
        [JsonIgnore]
        public int? Status { get; set; }
        // Danh sách các điểm đến
        public List<FDestinationModel> Destinations { get; set; } = new List<FDestinationModel>();
        //public List<FTicketTypeCreate> TicketTypesCreate { get; set; } = new List<FTicketTypeCreate>();

    }
}
