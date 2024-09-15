using BusinessObjects.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels.Feedback
{
    public class FeedbackUpdateModel
    {
        [Required]
        [FromForm(Name = "feedback-id")]      
        public Guid FeedbackId { get; set; }
        [FromForm(Name = "user-id")]
        public string? UserId { get; set; }
        [FromForm(Name = "booking-id")]
        public string? BookingId { get; set; }
        [FromForm(Name = "feedback-msg")]
        public string? FeedbackMsg { get; set; }
        [FromForm(Name = "status")]
        public EStatus? Status { get; set; }
    }
}
