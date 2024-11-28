using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels.Dashboard
{
    public class BookingCountViewModel
    {
        public int BookingActive { get; set; }
        public int BookingOverdue { get; set; }
        public int BookingCancelled { get; set; }
        public int BookingUsed { get; set; }
        public int BookingRefund { get; set; }
        public int BookingInProgress { get; set; }
        public int Total { get; set; }
    }
}
