using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels.PackageTourFlow.PackageTourGet
{
    public class GetLocationByDestinationModel
    {
        [Required(ErrorMessage = "DestinationId is required")]
        public string DestinationId { get; set; }
    }
}
