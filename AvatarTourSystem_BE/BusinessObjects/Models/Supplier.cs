using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models
{
    public partial class Supplier
    {
        public Supplier()
        {
            Services = new HashSet<Service>();
        }
        public string? SupplierId { get; set; }
        public string? SupplierName { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? Status { get; set; }

        public virtual ICollection<Service> Services { get; set; }
    }
}
