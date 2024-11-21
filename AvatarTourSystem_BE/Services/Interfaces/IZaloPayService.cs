using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IZaloPayService
    {
        public bool VerifyCallback(string data, string mac);
        public Task HandleCallbackAsync(dynamic callbackData);
    }
}
