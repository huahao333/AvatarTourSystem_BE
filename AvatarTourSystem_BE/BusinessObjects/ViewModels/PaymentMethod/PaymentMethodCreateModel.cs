using BusinessObjects.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels.PaymentMethod
{
    public class PaymentMethodCreateModel
    {
        [Required]
      //  [FromForm(Name = "payment-type")]
        public string PaymentType { get; set; }
      //  [FromForm(Name = "status")]
        public EStatus? Status { get; set; }
    }
}
