using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BusinessObjects.Models
{
    public partial class Account : IdentityUser
    {
        public Account()
        {
            CustomerSupports = new HashSet<CustomerSupport>();
            Notifications = new HashSet<Notification>();
            TransactionsHistorys = new HashSet<TransactionsHistory>();
            Feedbacks = new HashSet<Feedback>();
            Rates = new HashSet<Rate>();
            Bookings = new HashSet<Booking>();
        }
     //   public string? UserId { get; set; }
     //   public string? UserName { get; set; }
        public bool? Gender { get; set; }
        public DateTime? Dob { get; set; }
        public string? Address { get; set; }
        public string? FullName { get; set; }
        public string? Password { get; set; }
        public string? PhoneNumber { get; set; }
        public string? AvatarUrl { get; set; }
        public string? ZaloUser { get; set; }
        public string? Email { get; set; }
        public string? RefreshToken { get; set; }
        public string? NormalizedUserName { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? Status { get; set; }
        public int? Roles { get; set; }

        public virtual ICollection<CustomerSupport> CustomerSupports { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
        public virtual ICollection<TransactionsHistory> TransactionsHistorys { get; set; }
        public virtual ICollection<Feedback> Feedbacks { get; set; }
        public virtual ICollection<Rate> Rates { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }
    }
}
