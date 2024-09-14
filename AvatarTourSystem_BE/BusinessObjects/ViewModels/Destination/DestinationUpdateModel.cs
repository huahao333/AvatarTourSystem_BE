using BusinessObjects.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels.Destination
{
    public class DestinationUpdateModel
    {
        [Required]
        public Guid DestinationId { get; set; }
        public string? DestinationName { get; set; }
        public float? PriceDestination { get; set; }
        public EStatus? Status { get; set; }
    }
}
