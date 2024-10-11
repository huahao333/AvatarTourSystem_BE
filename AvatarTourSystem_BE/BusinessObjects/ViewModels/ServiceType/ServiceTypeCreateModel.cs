using BusinessObjects.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels.ServiceType
{
    public class ServiceTypeCreateModel
    {
     //   [FromForm(Name = "service-name")]
        public string? ServiceTypeName { get; set; }
      //  [FromForm(Name = "status")]
        public EStatus? Status { get; set; }
    }
}
