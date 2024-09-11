using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models
{
    public partial class CustomerSupport
    {
        public string? CusSupportId { get; set; }
        public string? UserId { get; set; }
        public string? RequestTypeId { get; set; }
        public string? Description { get; set; }
        public DateTime? DateResolved { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? Status { get; set; }

        public virtual Account? Accounts { get; set; }
        public virtual RequestType? RequestTypes { get; set; }
    }
}
