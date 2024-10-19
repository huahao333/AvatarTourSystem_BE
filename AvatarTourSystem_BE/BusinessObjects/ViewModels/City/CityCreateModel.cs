using BusinessObjects.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels.City
{
    public class CityCreateModel
    {
       // [FromForm(Name = "city-name")]
        public string? CityName { get; set; } = "";
        // [FromForm(Name = "status")]
        public EStatus? Status { get; set; }
    }
}
