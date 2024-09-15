using BusinessObjects.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels.BookingByRevenue
{
    public class BookingByRevenueUpdateModel
    {
        public Guid BookingByRevenueId { get; set; }
        public string? RevenueId { get; set; }
        public string? BookingId { get; set; }
        public EStatus? Status { get; set; }
    }
}
