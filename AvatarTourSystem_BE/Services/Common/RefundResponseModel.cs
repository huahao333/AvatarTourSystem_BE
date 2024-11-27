using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Common
{
    public class RefundResponseModel
    {
        public int ReturnCode { get; set; }
        public string Message { get; set; }
        public string TransactionId { get; set; }
    }
}
