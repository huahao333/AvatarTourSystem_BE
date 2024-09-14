using BusinessObjects.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels.TicketType
{
    public class TicketTypeUpdateModel
    {
        [Required]
        public Guid TicketTypeId { get; set; }
        public string? PackageTourId { get; set; }
        public string? TicketTypeName { get; set; }
        public EStatus? Status { get; set; }
    }
}
