using BusinessObjects.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels.TransactionHistory
{
    public class TransactionHistoryCreateModel
    {
      
        public string UserId { get; set; }
        public string BookingId { get; set; }
        public EStatus? Status { get; set; }
    }
}
