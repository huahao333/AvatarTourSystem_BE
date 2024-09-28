using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels.DailyTicket
{
    public class DailyTicketModel
    {
        public string? DailyTicketId { get; set; }
        public string? TicketTypeId { get; set; }
        public string? DailyTourId { get; set; }
        public int? Capacity { get; set; }
        public float? DailyTicketPrice { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? Status { get; set; }
    }
}
