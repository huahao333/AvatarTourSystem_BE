using BusinessObjects.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels.Notification
{
    public class NotificationCreateByZaloIdModel
    {
        public string? ZaloId { get; set; }
        public string? Message { get; set; } = "";
        public string? Type { get; set; } = "";
        public string? Title { get; set; } = "";
    }
}
