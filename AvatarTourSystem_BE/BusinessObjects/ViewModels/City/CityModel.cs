using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels.City
{
    public class CityModel
    {
        [FromForm(Name = "city-id")]
        public string? CityId { get; set; }
        [FromForm(Name = "city-name")]
        public string? CityName { get; set; }
        [FromForm(Name = "create-date")]
        public DateTime? CreateDate { get; set; }
        [FromForm(Name = "update-date")]
        public DateTime? UpdateDate { get; set; }
        [FromForm(Name = "status")]
        public int? Status { get; set; }
    }
}
