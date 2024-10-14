using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Services.Services
{
    public class ZaloServices
    {
        private readonly string _secretKeyZalo;
        private readonly HttpClient _httpClient;
        public ZaloServices( string secretKeyZalo, HttpClient httpClient)
        {
            _secretKeyZalo = secretKeyZalo;
            _httpClient = httpClient;

        }

        public async Task<string> CallZaloApiAsync(string accessToken, string phoneToken)
        {

            string url = "https://graph.zalo.me/v2.0/me/info";

            _httpClient.DefaultRequestHeaders.Add("access_token", accessToken);
            _httpClient.DefaultRequestHeaders.Add("code", phoneToken);
            _httpClient.DefaultRequestHeaders.Add("secret_key", _secretKeyZalo);

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            var responseBody = await response.Content.ReadAsStringAsync();

            Console.WriteLine("Response body: " + responseBody);

            try
            {
                var jsonDocument = JsonDocument.Parse(responseBody);
                if (jsonDocument.RootElement.TryGetProperty("data", out var dataElement) && dataElement.TryGetProperty("number", out var numberElement))
                {
                    string phoneNumber = numberElement.GetString();
                    return phoneNumber;
                }
                else
                {
                    Console.WriteLine("Không tìm thấy các thuộc tính mong đợi trong phản hồi.");
                    return null;
                }
            }
            catch (JsonException jsonEx)
            {
                Console.WriteLine("Lỗi phân tích JSON: " + jsonEx.Message);
                return null;
            }
        }
    }
}
