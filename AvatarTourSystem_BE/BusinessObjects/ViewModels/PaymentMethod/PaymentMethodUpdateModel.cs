using BusinessObjects.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels.PaymentMethod
{
    public class PaymentMethodUpdateModel
    {
       
        [Required]
        public string PaymentType { get; set; }
        public EStatus? Status { get; set; }
        
    }
}
