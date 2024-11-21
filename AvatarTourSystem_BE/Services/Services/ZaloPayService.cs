using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Services.Common;
using Services.Interfaces;
using Services.Common.ZaloPayHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class ZaloPayService : IZaloPayService
    {
        private readonly string _key2;
        private readonly ILogger<ZaloPayService> _logger;

        public ZaloPayService(IConfiguration configuration, ILogger<ZaloPayService> logger)
        {
            _key2 = configuration["ZaloPay:Key2"];
            _logger = logger;
        }

        public bool VerifyCallback(string data, string requestMac)
        {
            try
            {
                var mac = ZaloPayHelper.HmacHelper.Compute(ZaloPayHelper.ZaloPayHMAC.HMACSHA256, _key2, data);
                return mac.Equals(requestMac, StringComparison.OrdinalIgnoreCase);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error verifying ZaloPay callback");
                return false;
            }
        }

        public async Task HandleCallbackAsync(dynamic callbackData)
        {
            try
            {
                // Update the order's status
                var appTransId = callbackData["app_trans_id"];
                Console.WriteLine("Update order's status = success where app_trans_id = {0}", appTransId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling ZaloPay callback");
                throw;
            }
        }
    }
}
