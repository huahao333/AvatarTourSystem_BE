using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels.Supplier
{
    public class SupplierCreateModel
    {
        public string? SupplierId { get; set; }
        public string? SupplierName { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? Status { get; set; }
    }
}
