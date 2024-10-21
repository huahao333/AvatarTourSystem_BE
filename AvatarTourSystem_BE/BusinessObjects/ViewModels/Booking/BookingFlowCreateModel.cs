using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels.Booking
{
    public class BookingFlowCreateModel
    {
        public string ZaloId { get; set; }
        public string DailyTourId { get; set; }
        public float TotalPrice { get; set; }
        public List<TicketModels> Tickets { get; set; }
    }
    public class TicketModels
    {
        public string TicketTypeId { get; set; }
        public int TotalQuantity { get; set; }
        public float TotalPrice { get; set; }
        public string TicketName { get; set; }= string.Empty;
     //   public string QR { get; set; }
     //   public float Price { get; set; }
    }
}
