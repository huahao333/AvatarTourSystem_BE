using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

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
            var encodedAddress = Uri.EscapeDataString(address);

        //    var embedCode = $"<iframe width='600' height='450' style='border:0' loading='lazy' allowfullscreen src='https://www.google.com/maps?q={encodedAddress}&output=embed'></iframe>";
            var embedCode = $"https://www.google.com/maps?q={encodedAddress}&output=embed";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(embedCode);
                    response.EnsureSuccessStatusCode();

                    string detectedLink = response.RequestMessage.RequestUri.ToString();
                    return detectedLink;
                }
                catch (HttpRequestException ex)
                {
                    return $"Error: {ex.Message}";
                }
            }
        }

    }
}
