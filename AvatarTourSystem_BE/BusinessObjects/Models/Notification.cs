using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models
{
    public partial class Notification
    {
        public string? NotifyId { get; set; }
        public string? UserId { get; set; }
        public DateTime? SendDate { get; set; }
        public string? Message { get; set; }
        public string? Type { get; set; }
        public string? Title { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? Status { get; set; }

        public virtual Account? Accounts { get; set; }
    }
}
