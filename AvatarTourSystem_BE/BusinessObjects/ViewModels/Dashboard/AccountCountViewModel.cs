using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels.Dashboard
{
    public class AccountCountViewModel
    {
        public int SuperAdmin { get; set; }
        public int Admin { get; set; }
        public int Staff { get; set; }
        public int Supplier { get; set; }
        public int Customer { get; set; }
        public int Total { get; set; }
    }
}
