using BusinessObjects.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels.Revenue
{
    public class RevenueUpdateModel
    {
        public Guid RevenueId { get; set; }
        public float? TotalRevenue { get; set; }
        public EStatus? Status { get; set; }
    }
}
