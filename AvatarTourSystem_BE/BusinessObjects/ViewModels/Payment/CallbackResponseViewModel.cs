using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels.Payment
{
    public class CallbackResponseViewModel
    {
        public CallbackData Data { get; set; }
        public string overallMac { get; set; }
        public string Mac { get; set; }
    }
    public class CallbackData
    {
        public decimal Amount { get; set; }
        public int TransTime { get; set; }
        public string Method { get; set; }
        public string OrderId { get; set; }
        public string TransId { get; set; }
        public string AppId { get; set; }
        public string Extradata { get; set; }
        public int ResultCode { get; set; } 
        public string Description { get; set; }
        public string MerchantTransId { get; set; }
        public string Message { get; set; }
        public string PaymentChannel { get; set; }
    }

    public class ExtraDatas
    {
        public string ZaloId { get; set; }
        public string BookingId { get; set; }
    }

    public class ExtraData
    {
        public string ZaloId { get; set; }
     //   public string BookingId { get; set; }
        public string DailyTourId { get; set; }
        public List<TicketExtraModel> Tickets { get; set; }
    }
    public class TicketExtraModel
    {
        public string DailyTicketId { get; set; }
        public int TotalQuantity { get; set; }
        public float TotalPrice { get; set; }
        public string TicketName { get; set; }
    }
}
