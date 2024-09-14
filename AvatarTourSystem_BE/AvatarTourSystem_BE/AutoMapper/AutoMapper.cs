using AutoMapper;
using BusinessObjects.Models;
using BusinessObjects.ViewModels.PackageTour;
using BusinessObjects.ViewModels.Supplier;
using BusinessObjects.ViewModels.TicketType;
using BusinessObjects.ViewModels.TourSegment;
using Microsoft.OpenApi.Writers;

namespace AvatarTourSystem_BE.AutoMapper
{
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {
            //Supplier
            CreateMap<Supplier, SupplierModel>().ReverseMap();
            CreateMap<Supplier, SupplierCreateModel>().ReverseMap();
            CreateMap<Supplier, SupplierUpdateModel>().ReverseMap();

            //TourSegment
            CreateMap<TourSegment, TourSegmentModel>().ReverseMap();
            CreateMap<TourSegment, TourSegmentCreateModel>().ReverseMap();
            CreateMap<TourSegment, TourSegmentUpdateModel>().ReverseMap();

            //PackageTour
            CreateMap<PackageTour, PackageTourModel>().ReverseMap();
            CreateMap<PackageTour, PackageTourCreateModel>().ReverseMap();
            CreateMap<PackageTour, PackageTourUpdateModel>().ReverseMap();

            //TicketType
            CreateMap<TicketType, TicketTypeModel>().ReverseMap();
            CreateMap<TicketType, TicketTypeCreateModel>().ReverseMap();
            CreateMap<TicketType, TicketTypeUpdateModel>().ReverseMap();
        }
    }
}
