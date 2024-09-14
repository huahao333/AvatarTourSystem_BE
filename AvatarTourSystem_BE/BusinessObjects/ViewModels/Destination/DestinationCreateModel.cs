using BusinessObjects.Enums;

namespace BusinessObjects.ViewModels.Destination
{
    public class DestinationCreateModel
    {
        public string? CityId { get; set; }
        public string? DestinationName { get; set; }
        public float? PriceDestination { get; set; }
        public EStatus? Status { get; set; }
    }
}
