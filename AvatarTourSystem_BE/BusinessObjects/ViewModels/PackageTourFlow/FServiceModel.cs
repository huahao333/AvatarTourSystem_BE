using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels.PackageTourFlow
{
    public class FServiceModel
    {
        [JsonIgnore]
        public string? ServiceId { get; set; } // Không bắt buộc

        [Required(ErrorMessage = "ServiceName is required")]
        [StringLength(100, ErrorMessage = "ServiceName can't be longer than 100 characters")]
        public string? ServiceName { get; set; } // Bắt buộc, giới hạn độ dài 100 ký tự

        [Range(0.01, double.MaxValue, ErrorMessage = "ServicePrice must be greater than 0")]
        public float? ServicePrice { get; set; } // Không bắt buộc, phải lớn hơn 0

    }
}
