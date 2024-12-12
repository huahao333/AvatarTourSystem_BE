using BusinessObjects.Enums;
using BusinessObjects.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels.CustomerSupport
{
    public class CustomerSupportModel
    {
      //  [FromForm(Name = "customer-support-id")]
        public string? CusSupportId { get; set; }
      //  [FromForm(Name = "user-id")]
        public string? UserId { get; set; }
      //  [FromForm(Name = "request-type-id")]
        public string? RequestTypeId { get; set; }
       // [FromForm(Name = "description")]
        public string? Description { get; set; }
      //  [FromForm(Name = "date-resolved")]
        public DateTime? DateResolved { get; set; }
       // [FromForm(Name = "create-date")]
        public DateTime? CreateDate { get; set; }
      //  [FromForm(Name = "update-date")]
        public DateTime? UpdateDate { get; set; }
      //  [FromForm(Name = "status")]
        public EStatus? Status { get; set; }
    }

    public class CustomerSupportStatusViewModel
    {
        public string CusSupportId { get; set; }
        public string UserId { get; set; }
        public string NotificationDescription { get; set; }
        public int Status { get; set; }

    }

    public class CustomerSupportRequestCreateModel
    {

        public string? ZaloUser { get; set; } = "";
        public string? Description { get; set; } = "";
        public string? BookingId { get; set; } = "";
    }

    public class SupportRequestByZaloIdViewModel
    {
        public string? ZaloUser { get; set; }
        public string? Description { get; set; }
        public string? RequestTypeId { get; set; }

    }
    public class GetListRequestViewModel
    {
        public string? ZaloUser { get; set; }
    }
}
