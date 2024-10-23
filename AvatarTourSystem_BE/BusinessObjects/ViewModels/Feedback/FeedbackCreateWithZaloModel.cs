using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels.Feedback
{
    public class FeedbackCreateWithZaloModel
    {
        public string? ZaloUser { get; set; }
        public string? BookingId { get; set; }
        public string? FeedbackMsg { get; set; }
    }
}
