using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels.PackageTour
{
    public class PackageTourModel
    {
        public string? PackageTourId { get; set; }
        public string? CityId { get; set; }
        public string? PackageTourName { get; set; }
        public float? PackageTourPrice { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? Status { get; set; }
    }
}
