using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Common
{
    public class ZaloPayResponseModel
    {
        public string appId { get; set; }
        public string orderid { get; set; }
        public string transId { get; set; }
        public string method { get; set; }
        public long transTime { get; set; }
        public string merchantTransId { get; set; }
        public long amount { get; set; }
        public string description { get; set; }
        public int resultCode { get; set; }
        public string message { get; set; }
        public string? extradata { get; set; }
    }
    public class ZaloPayCallback
    {
        public string Data { get; set; } = string.Empty;
        public string Mac { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
    }
    public class ZaloPayCallbackData
    {
        public string AppId { get; set; } = string.Empty;
        public string AppTransId { get; set; } = string.Empty;
        public string AppTime { get; set; } = string.Empty;
        public string AppUser { get; set; } = string.Empty;
        public string Amount { get; set; } = string.Empty;
        public string EmbedData { get; set; } = string.Empty;
        public string Item { get; set; } = string.Empty;
        public string ZpTransId { get; set; } = string.Empty;
        public string ServerTime { get; set; } = string.Empty;
        public string Channel { get; set; } = string.Empty;
        public string MerchantUserId { get; set; } = string.Empty;
        public string UserFeeAmount { get; set; } = string.Empty;
        public string DiscountAmount { get; set; } = string.Empty;
        public int Status { get; set; }
    }
}
