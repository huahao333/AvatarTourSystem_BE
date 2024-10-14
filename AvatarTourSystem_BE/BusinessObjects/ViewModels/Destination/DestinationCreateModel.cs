using BusinessObjects.Enums;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace BusinessObjects.ViewModels.Destination
{
    public class DestinationCreateModel
    {
     //   [FromForm(Name = "city-id")]
        public string? CityId { get; set; }
      //  [FromForm(Name = "destination-name")]
        public string? DestinationName { get; set; }

      //  [FromForm(Name = "destination-img-url")]
      //  public string? DestinationImgUrl { get; set; }
      //  [FromForm(Name = "status")]
        public int? Status { get; set; }
        public string? DestinationHotline { get; set; }
        public string? DestinationGoogleMap { get; set; }
        public string? DestinationImgUrl { get; set; }
        public string? DestinationAddress { get; set; }
        public DateTime? DestinationOpeningHours { get; set; }
        public DateTime? DestinationClosingHours { get; set; }
        public int? DestinationOpeningDate { get; set; }
        public int? DestinationClosingDate { get; set; }

        [JsonIgnore]
        public string? EmbedCode { get; set; }
    }
}
