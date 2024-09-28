using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels.Destination
{
    public class DestinationModel
    {
        public Guid DestinationId { get; set; }
        public string? CityId { get; set; }
        public string? DestinationName { get; set; }
        public string? DestinationImgUrl { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? Status { get; set; }
    }
}
