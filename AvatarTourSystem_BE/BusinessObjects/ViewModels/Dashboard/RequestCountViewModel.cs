using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels.Dashboard
{
    public class RequestCountViewModel
    {
        public int RequestInProgress { get; set; }
        public int RequestCompleted { get; set; }
        public int RequestDeleted { get; set; }
        public int Total { get; set; }
    }
}
