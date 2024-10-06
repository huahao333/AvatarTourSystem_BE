using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels.PackageTourFlow.PackageTourUpdate
{
    public class FDestinationUpdateModel
    {
     
        public string? DestinationId { get; set; } 
        [Required(ErrorMessage = "DestinationName is required")]        
        public string? CityId { get; set; } 
        public List<FLocationUpdateModel> Locations { get; set; } = new List<FLocationUpdateModel>();
    }
}
