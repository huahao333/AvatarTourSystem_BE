using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class ZaloPaymentService
    {
        private readonly HttpClient _httpClient;

        public ZaloPaymentService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<GetOrderStatusResponse> GetOrderStatusAsync(string orderId, string appId, string privateKey)
        {
            string mac = MacHelper.GenerateMac(appId, orderId, privateKey);

            var queryParams = new Dictionary<string, string>
        {
            { "orderId", orderId },
            { "appId", appId },
            { "mac", mac }
        };

            string queryString = string.Join("&", queryParams.Select(kvp => $"{kvp.Key}={Uri.EscapeDataString(kvp.Value)}"));
            string url = $"https://payment-mini.zalo.me/api/transaction/get-status?{queryString}";

            HttpResponseMessage response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<GetOrderStatusResponse>();
                return data ?? throw new Exception("Response content is empty");
            }
            else
            {
                string errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"API call failed: {response.StatusCode} - {errorContent}");
            }
        }
    }

    public class GetOrderStatusRequest
    {
        public string OrderId { get; set; }
        public string AppId { get; set; }
        public string Mac { get; set; }
    }

    public class GetOrderStatusResponse
    {
        public string TransId { get; set; }
        public string Method { get; set; }
        public int ReturnCode { get; set; }
        public string ReturnMessage { get; set; }
        public bool IsProcessing { get; set; }
        public long Amount { get; set; }
        public long TransTime { get; set; }
        public string MerchantTransId { get; set; }
        public string Extradata { get; set; }
    }
    public static class MacHelper
    {
        public static string GenerateMac(string appId, string orderId, string privateKey)
        {
            string data = $"appId={appId}&orderId={orderId}&privateKey={privateKey}";
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(privateKey));
            byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
            return Convert.ToHexString(hash).ToLower();
        }
    }
}
