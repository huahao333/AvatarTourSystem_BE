using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BusinessObjects.Models
{
    public partial class Booking
    {
        public Booking()
        {
            Feedbacks = new HashSet<Feedback>();
            Rates = new HashSet<Rate>();
            Tickets = new HashSet<Ticket>();
            TransactionsHistories = new HashSet<TransactionsHistory>();
            Payments = new HashSet<Payment>();
        }
        public string? BookingId { get; set; }
        public string? UserId { get; set; }
        public DateTime? BookingDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string? DailyTourId { get; set; }
     //   public string? PaymentId { get; set; }
        public float? TotalPrice { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? Status { get; set; }


        public virtual Account? Accounts { get; set; }
      //  public virtual PaymentMethod? PaymentMethods { get; set; }
        public virtual DailyTour? DailyTours { get; set; }
        public virtual ICollection<Feedback> Feedbacks { get; set; }
        public virtual ICollection<Rate> Rates { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }
        public virtual ICollection<TransactionsHistory> TransactionsHistories { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
        //  public virtual ICollection<BookingByRevenue> BookingByRevenues { get; set; }
    }
}
