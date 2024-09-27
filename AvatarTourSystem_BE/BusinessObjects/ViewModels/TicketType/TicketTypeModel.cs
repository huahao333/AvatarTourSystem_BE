using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels.TicketType
{
    public class TicketTypeModel
    {
        public string? TicketTypeId { get; set; }
        public string? PackageTourId { get; set; }
        public string? TicketTypeName { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? Status { get; set; }
    }
}
