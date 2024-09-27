using BusinessObjects.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels.Revenue
{
    public class RevenueCreateModel
    {
        [FromForm(Name = "total-revenue")]
        public float? TotalRevenue { get; set; }

        [FromForm(Name = "status")]
        public EStatus? Status { get; set; }
    }
}
