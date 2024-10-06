using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels.PackageTourFlow.PackageTourUpdate
{
    public class FServiceUpdateModel
    {
        public string? ServiceId { get; set; }     
        public int? Status { get; set; }
        
    }
}
