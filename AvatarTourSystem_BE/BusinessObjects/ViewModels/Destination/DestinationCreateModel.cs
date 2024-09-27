using BusinessObjects.Enums;
using Microsoft.AspNetCore.Mvc;

namespace BusinessObjects.ViewModels.Destination
{
    public class DestinationCreateModel
    {
        [FromForm(Name = "city-id")]
        public string? CityId { get; set; }
        [FromForm(Name = "destination-name")]
        public string? DestinationName { get; set; }
        [FromForm(Name = "price-destination")]
        public float? PriceDestination { get; set; }
        [FromForm(Name = "status")]
        public EStatus? Status { get; set; }
    }
}
