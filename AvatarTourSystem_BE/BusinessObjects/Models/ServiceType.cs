using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models
{
    public partial class ServiceType
    {
        public ServiceType()
        {
            Services = new HashSet<Service>();
        }
        public string? ServiceTypeId { get; set; }
        public string? ServiceTypeName { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? Status { get; set; }

        public virtual ICollection<Service> Services { get; set; }
    }
}
