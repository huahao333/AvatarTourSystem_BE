using BusinessObjects.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels.Notification
{
    public class NotificationModel
    {
      //  [FromForm(Name = "notify-id")]
        public string? NotifyId { get; set; }
     //   [FromForm(Name = "user-id")]
        public string? UserId { get; set; }
      //  [FromForm(Name = "send-date")]
        public DateTime? SendDate { get; set; }
      //  [FromForm(Name = "message")]
        public string? Message { get; set; }
     //   [FromForm(Name = "type")]
        public string? Type { get; set; }
      //  [FromForm(Name = "title")]
        public string? Title { get; set; }
     //   [FromForm(Name = "create-date")]
        public DateTime? CreateDate { get; set; }
     //   [FromForm(Name = "update-date")]
        public DateTime? UpdateDate { get; set; }
     //   [FromForm(Name = "status")]
        public EStatus? Status { get; set; }
    }
}
