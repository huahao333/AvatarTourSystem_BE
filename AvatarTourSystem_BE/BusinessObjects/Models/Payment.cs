using Google.Apis.Storage.v1.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models
{
    public class Payment
    {
        public string? PaymentId { get; set; }
        public string? PaymentMethodId { get; set; }
        public string? BookingId { get; set; }
        public string? AppId { get; set; }
        public string? OrderId { get; set; }
        public string? TransId { get; set; }
        public long? TransTime { get; set; }
        public float? Amount { get; set; }
        public string? MerchantTransId { get; set; }
        public string? Description { get; set; }
        public int? ResultCode { get; set; }
        public string? Message { get; set; }
        public string? ExtraData { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? Status { get; set; }

        public virtual Booking? Booking { get; set; }
        public virtual PaymentMethod? PaymentMethod { get; set; }
    }
}
