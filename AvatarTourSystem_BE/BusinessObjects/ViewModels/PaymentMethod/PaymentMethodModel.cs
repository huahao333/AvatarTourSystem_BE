using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels.PaymentMethod
{
    public class PaymentMethodModel
    {
        public Guid? PaymentId { get; set; }
        public string? PaymentType { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int Status { get; set; }
    }
}
