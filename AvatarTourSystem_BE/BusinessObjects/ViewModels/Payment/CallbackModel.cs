using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels.Payment
{
    public class CallbackModel
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
}
