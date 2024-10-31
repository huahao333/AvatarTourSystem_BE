using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels.Booking
{
    public class BookingFlowModel
    {
        public string? BookingId { get; set; }
       // public string? UserId { get; set; }
    }

    public class DecryptBooking
    {
        public string EncryptedQr { get; set; }
    }
    public class BookingFlowDataModel
    {
        public string BookingId { get; set; }
        public string ZaloUser { get; set; }
        public string DailyTourId { get; set; }
        public string TourName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Discount { get; set; }
        public decimal DailyTourPrice { get; set; }
        public string City { get; set; }
        public string DestinationId { get; set; }
        public string LocationId { get; set; }
        public string ServiceId { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime BookingDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string TicketTypeId { get; set; }
        public string DailyTicketId { get; set; }
        public string TicketName { get; set; }
        public decimal Price { get; set; }
    }
}
