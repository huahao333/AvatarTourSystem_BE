using BusinessObjects.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels.City
{
    public class CityCreateModel
    {
        public string? CityName { get; set; }
        public EStatus? Status { get; set; }
    }
}
