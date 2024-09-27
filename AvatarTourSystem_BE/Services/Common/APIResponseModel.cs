using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Common
{
    public class APIResponseModel
    {
        public string Message { get; set; } = string.Empty;
        public bool IsSuccess { get; set; } 
        public object? Data { get; set; }
    }
}
