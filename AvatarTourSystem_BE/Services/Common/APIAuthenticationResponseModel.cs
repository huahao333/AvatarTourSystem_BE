using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Common
{
    public class APIAuthenticationResponseModel
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public string JwtToken { get; set; }
    }
}
