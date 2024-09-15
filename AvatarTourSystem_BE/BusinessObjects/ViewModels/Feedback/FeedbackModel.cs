using BusinessObjects.Enums;
using BusinessObjects.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels.Feedback
{
    public class FeedbackModel
    {
        [FromForm(Name = "feedback-id")]
        public string? FeedbackId { get; set; }
        [FromForm(Name = "user-id")]
        public string? UserId { get; set; }
        [FromForm(Name = "booking-id")]
        public string? BookingId { get; set; }
        [FromForm(Name = "feedback-msg")]
        public string? FeedbackMsg { get; set; }
        [FromForm(Name = "create-date")]
        public DateTime? CreateDate { get; set; }
        [FromForm(Name = "update-date")]
        public DateTime? UpdateDate { get; set; }
        [FromForm(Name = "status")]
        public EStatus? Status { get; set; }
      
    }
}
