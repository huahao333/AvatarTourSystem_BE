using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels.PaymentMethod
{
    public class PaymentMethodModel
    {
        [FromForm(Name = "payment-id")]
        public Guid? PaymentId { get; set; }
        [FromForm(Name = "payment-type")]
        public string? PaymentType { get; set; }
        [FromForm(Name = "create-date")]
        public DateTime? CreateDate { get; set; }
        [FromForm(Name = "update-date")]
        public DateTime? UpdateDate { get; set; }
        [FromForm(Name = "status")]
        public int Status { get; set; }
    }
}
