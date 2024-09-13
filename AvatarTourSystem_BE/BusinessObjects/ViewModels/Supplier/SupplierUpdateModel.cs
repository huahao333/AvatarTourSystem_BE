using BusinessObjects.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels.Supplier
{
    public class SupplierUpdateModel
    {
        [Required]
        public string SupplierId { get; set; }
        public string? SupplierName { get; set; }
        public EStatus? Status { get; set; }
    }
}
