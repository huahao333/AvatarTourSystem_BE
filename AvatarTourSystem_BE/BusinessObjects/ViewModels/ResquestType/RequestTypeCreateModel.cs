using BusinessObjects.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels.ResquestType
{
    public class RequestTypeCreateModel
    {
      //  [FromForm(Name = "type-name")]
        public string? Type { get; set; } = "";
        //  [FromForm(Name = "priority")]
        public int? Priority { get; set; } = 0;
       // [FromForm(Name = "status")]
        public EStatus? Status { get; set; }
    }
}
