using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class GoogleMapsService
    {
        private readonly string _apiKey;

        public GoogleMapsService(string apiKey)
        {
            _apiKey = apiKey;
        }

        public async Task<string> GetEmbedCodesAsync(string address)
        {
            // Mã hóa địa chỉ để URL-safe
            var encodedAddress = Uri.EscapeDataString(address);

            // Tạo embed code với địa chỉ đã mã hóa
        //    var embedCode = $"<iframe width='600' height='450' style='border:0' loading='lazy' allowfullscreen src='https://www.google.com/maps?q={encodedAddress}&output=embed'></iframe>";
            var embedCode = $"https://www.google.com/maps?q={encodedAddress}&output=embed";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Gửi yêu cầu GET để lấy link đã "detect"
                    HttpResponseMessage response = await client.GetAsync(embedCode);
                    response.EnsureSuccessStatusCode();

                    // Trả về URL đã được xác thực
                    string detectedLink = response.RequestMessage.RequestUri.ToString();
                    return detectedLink;
                }
                catch (HttpRequestException ex)
                {
                    // Xử lý lỗi nếu có
                    return $"Error: {ex.Message}";
                }
            }
        }

    }
}
