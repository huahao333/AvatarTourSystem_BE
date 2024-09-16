using BusinessObjects.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels.ResquestType
{
    public class RequestTypeModel
    {
        [FromForm(Name = "request-type-id")]
        public string? RequestTypeId { get; set; }
        [FromForm(Name = "type-name")]
        public string? Type { get; set; }
        [FromForm(Name = "priority")]
        public int? Priority { get; set; }
        [FromForm(Name = "create-date")]
        public DateTime? CreateDate { get; set; }
        [FromForm(Name = "update-date")]
        public DateTime? UpdateDate { get; set; }
        [FromForm(Name = "status")]
        public EStatus? Status { get; set; }
    }
}
