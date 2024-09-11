using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models
{
    public partial class RequestType
    {
        public RequestType()
        {
            CustomerSupports = new HashSet<CustomerSupport>();
        }
        public string? RequestTypeId { get; set; }
        public string? Type { get; set; }
        public int? Priority { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? Status { get; set; }

        public virtual ICollection<CustomerSupport> CustomerSupports { get; set; }
    }
}
