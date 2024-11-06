using BusinessObjects.ViewModels.Location;
using BusinessObjects.ViewModels.PackageTour;
using BusinessObjects.ViewModels.Service;
using BusinessObjects.ViewModels.TicketType;
using BusinessObjects.ViewModels.TourSegment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels.Booking
{
    public class BookingModel
    {
        public string? BookingId { get; set; }
        public string? UserId { get; set; }
        public DateTime? BookingDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string? DailyTourId { get; set; }
        public string? PaymentId { get; set; }
        public float? TotalPrice { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? Status { get; set; }
    }
    public class DailyTourDetailModel
    {
        public string DailyTourId { get; set; }
        public string PackageTourId { get; set; }
        public string DailyTourName { get; set; }
        public string Description { get; set; }
        public float? DailyTourPrice { get; set; }
        public string ImgUrl { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? Discount { get; set; }
        public int? StatusDailyTour { get; set; }
     //   public List<TicketTypeModel> TicketTypes { get; set; }
        public PackageToursModel PackageTour { get; set; }
    }
    public class PackageToursModel
    {
        public string PackageTourId { get; set; }
        public string PackageTourName { get; set; }
        public string PackageTourImgUrl { get; set; }
        public string CityId { get; set; }
        public int? StatusPackageTour { get; set; }
        public string CityName { get; set; }
        public List<TourSegmentsModel> TourSegments { get; set; }
    }

    public class TourSegmentsModel
    {
        public string TourSegmentId { get; set; }
        public string DestinationId { get; set; }
        public string DestinationName { get; set; }
        public string DestinationAddress { get; set; }
        public string DestinationImgUrl { get; set; }
        public string DestinationHotline { get; set; }
        public string DestinationGoogleMap { get; set; }
        public int? DestinationOpeningDate { get; set; }
        public int? DestinationClosingDate { get; set; }
        public DateTime? DestinationOpeningHours { get; set; }
        public DateTime? DestinationClosingHours { get; set; }
        public List<LocationsModel> Locations { get; set; }
    }

    public class LocationsModel
    {
        public string LocationId { get; set; }
        public string LocationName { get; set; }
        public string LocationImgUrl { get; set; }
        public string LocationHotline { get; set; }
        public DateTime? LocationOpeningHours { get; set; }
        public DateTime? LocationClosingHours { get; set; }
        public string LocationGoogleMap { get; set; }
        public string DestinationId { get; set; }
        public List<ServicesModel> Services { get; set; }
    }

    public class ServicesModel
    {
        public string ServiceId { get; set; }
        public string ServiceName { get; set; }
        public float? ServicePrice { get; set; }
        public string SupplierName { get; set; }
        public string LocationId { get; set; }
        public string ServiceTypeName { get; set; }
    }
}
