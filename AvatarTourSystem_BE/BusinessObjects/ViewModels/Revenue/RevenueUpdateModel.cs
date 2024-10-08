using BusinessObjects.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels.Revenue
{
    public class RevenueUpdateModel
    {
        [Required]
      //  [FromForm(Name = "revenue-id")]
        public Guid RevenueId { get; set; }

      //  [FromForm(Name = "total-revenue")]
        public float? TotalRevenue { get; set; }

      //  [FromForm(Name = "status")]
        public EStatus? Status { get; set; }
    }
}
