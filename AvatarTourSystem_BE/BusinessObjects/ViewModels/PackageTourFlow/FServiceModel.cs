using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels.PackageTourFlow
{
    public class FServiceModel
    {
        [JsonIgnore]
        public string? ServiceId { get; set; }
        public string? ServiceName { get; set; }
        public float? ServicePrice { get; set; }
    }
}
