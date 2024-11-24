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
using System.Security.Cryptography;

namespace Services.Services
{
    public class ZaloPayService : IZaloPayService
    {
        private readonly string _privateKey;

        public ZaloPayService(IConfiguration configuration)
        {
            _privateKey = configuration["ZaloPay:PrivateKey"];
        }

        public bool ValidateCallback(ZaloMiniAppCallback data)
        {
            var mac = GenerateMac(data);
            var overallMac = GenerateOverallMac(data);

            // Implement your validation logic here
            return true; // Replace with actual validation
        }

        public string GenerateMac(ZaloMiniAppCallback data)
        {
            var dataStr = $"appId={data.appId}&amount={data.amount}&description={data.description}" +
                         $"&orderId={data.orderid}&message={data.message}&resultCode={data.resultCode}" +
                         $"&transId={data.transId}";

            return ComputeHmacSha256(dataStr, _privateKey);
        }

        public string GenerateOverallMac(ZaloMiniAppCallback data)
        {
            var properties = data.GetType().GetProperties()
                .OrderBy(p => p.Name)
                .Select(p => $"{p.Name.ToLowerFirst()}={p.GetValue(data)}")
                .ToList();

            var dataStr = string.Join("&", properties);
            return ComputeHmacSha256(dataStr, _privateKey);
        }

        private string ComputeHmacSha256(string message, string secret)
        {
            var encoding = Encoding.UTF8;
            var keyBytes = encoding.GetBytes(secret);
            var messageBytes = encoding.GetBytes(message);

            using (var hmac = new HMACSHA256(keyBytes))
            {
                var hashBytes = hmac.ComputeHash(messageBytes);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }
    }

    public static class StringExtensions
    {
        public static string ToLowerFirst(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return str;

            return char.ToLower(str[0]) + str[1..];
        }
    }
}
