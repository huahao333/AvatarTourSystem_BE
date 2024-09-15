using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels.Revenue
{
    public class RevenueModel
    {
        public string? RevenueId { get; set; }
        public float? TotalRevenue { get; set; }
        public DateTime? RevenueDate { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? Status { get; set; }
    }
}
