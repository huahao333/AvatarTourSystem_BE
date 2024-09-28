using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels.Service
{
    public class ServiceModel
    {
        public string? ServiceId { get; set; }
        public string? ServiceName { get; set; }
        public float? ServicePrice { get; set; }
        public string? ServiceImgUrl{ get; set; }
        public string? ServiceTypeId { get; set; }
        public string? LocationId { get; set; }
        public string? SupplierId { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? Status { get; set; }
    }
}
