using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels.PackageTourFlow.PackageTourGet
{
    public class GetDestinationByCityModel
    {
        [Required(ErrorMessage = "CityId is required")]
        public string CityId { get; set; }
    }
}
