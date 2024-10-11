using BusinessObjects.ViewModels.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        [Required(ErrorMessage = "LocationName is required")]
        [StringLength(150, ErrorMessage = "LocationName can't be longer than 150 characters")]
        public string? LocationName { get; set; } // Bắt buộc, giới hạn độ dài 150 ký tự

        [Range(0, 10, ErrorMessage = "LocationType must be between 0 and 10")]
        public int? LocationType { get; set; } // Không bắt buộc, nhưng phải nằm trong khoảng từ 0 đến 10
        public List<FServiceModel> Services { get; set; } = new List<FServiceModel>();
    }
}
