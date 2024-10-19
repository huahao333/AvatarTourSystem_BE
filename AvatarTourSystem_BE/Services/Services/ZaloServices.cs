using Services.Common;
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

        public async Task<(string PhoneNumber, string AccessToken, string PhoneToken)> CallZaloApiAsync(string accessToken, string phoneToken)
        {
            Console.WriteLine("Access Token: " + accessToken);
            Console.WriteLine("Phone Token: " + phoneToken);

            string url = $"https://graph.zalo.me/v2.0/me/info?access_token={accessToken}&code={phoneToken}&secret_key={_secretKeyZalo}";

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("Access Token sau khi gọi API (thất bại): " + accessToken);
                Console.WriteLine("Phone Token sau khi gọi API (thất bại): " + phoneToken);
                return (null, accessToken, phoneToken); 
            }

            var responseBody = await response.Content.ReadAsStringAsync();

            Console.WriteLine("Access Token sau khi gọi API (thành công): " + accessToken);
            Console.WriteLine("Phone Token sau khi gọi API (thành công): " + phoneToken);
            Console.WriteLine("Response body: " + responseBody);

            try
            {
                var jsonDocument = JsonDocument.Parse(responseBody);
                if (jsonDocument.RootElement.TryGetProperty("data", out var dataElement) && dataElement.TryGetProperty("number", out var numberElement))
                {
                    string phoneNumber = numberElement.GetString();
                    return (phoneNumber, accessToken, phoneToken); 
                }
                else
                {
                    Console.WriteLine("Không tìm thấy các thuộc tính mong đợi trong phản hồi.");
                    return (null, accessToken, phoneToken); 
                }
            }
            catch (JsonException jsonEx)
            {
                Console.WriteLine("Lỗi phân tích JSON: " + jsonEx.Message);
                return (null, accessToken, phoneToken); 
            }
        }
    }
}
