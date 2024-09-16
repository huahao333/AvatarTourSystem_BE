using BusinessObjects.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels.CustomerSupport
{
    public class CustomerSupportCreateModel
    {
        
        [FromForm(Name = "user-id")]
        public string? UserId { get; set; }
        [FromForm(Name = "request-type-id")]
        public string? RequestTypeId { get; set; }
        [FromForm(Name = "description")]
        public string? Description { get; set; }
        [FromForm(Name = "date-resolved")]
        public DateTime? DateResolved { get; set; }      
        [FromForm(Name = "status")]
        public EStatus? Status { get; set; }
    }
}
