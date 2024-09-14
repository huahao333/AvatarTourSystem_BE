using BusinessObjects.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels.PackageTour
{
    public class PackageTourCreateModel
    {
        public string? CityId { get; set; }
        public string? PackageTourName { get; set; }
        public float? PackageTourPrice { get; set; }
        public EStatus? Status { get; set; }
    }
}
