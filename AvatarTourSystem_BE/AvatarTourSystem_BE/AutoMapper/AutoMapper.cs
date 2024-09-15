using AutoMapper;
using BusinessObjects.Models;
using BusinessObjects.ViewModels.Booking;
using BusinessObjects.ViewModels.BookingByRevenue;
using BusinessObjects.ViewModels.City;
using BusinessObjects.ViewModels.DailyTicket;
using BusinessObjects.ViewModels.Destination;
using BusinessObjects.ViewModels.Feedback;
using BusinessObjects.ViewModels.PackageTour;
using BusinessObjects.ViewModels.PaymentMethod;
using BusinessObjects.ViewModels.Revenue;
using BusinessObjects.ViewModels.ServiceUsedByTicket;
using BusinessObjects.ViewModels.Supplier;
using BusinessObjects.ViewModels.Ticket;
using BusinessObjects.ViewModels.TicketType;
using BusinessObjects.ViewModels.TourSegment;
using BusinessObjects.ViewModels.TransactionHistory;
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

            //DailyTicket
            CreateMap<DailyTicket, DailyTicketModel>().ReverseMap();
            CreateMap<DailyTicket, DailyTicketCreateModel>().ReverseMap();
            CreateMap<DailyTicket, DailyTicketUpdateModel>().ReverseMap();

            //PaymentMethod
            CreateMap<PaymentMethod, PaymentMethodModel>().ReverseMap();
            CreateMap<PaymentMethod, PaymentMethodCreateModel>().ReverseMap();
            CreateMap<PaymentMethod, PaymentMethodUpdateModel>().ReverseMap();


            //Ticket
            CreateMap<Ticket, TicketModel>().ReverseMap();
            CreateMap<Ticket, TicketCreateModel>().ReverseMap();
            CreateMap<Ticket, TicketUpdateModel>().ReverseMap();

            //TransactionHistory
            CreateMap<TransactionsHistory, TransactionHistoryModel>().ReverseMap();
            CreateMap<TransactionsHistory, TransactionHistoryCreateModel>().ReverseMap();
            CreateMap<TransactionsHistory, TransactionHistoryUpdateModel>().ReverseMap();

            //City
            CreateMap<City, CityModel>().ReverseMap();
            CreateMap<City, CityCreateModel>().ReverseMap();
            CreateMap<City, CityUpdateModel>().ReverseMap();
            
            //Destination
            CreateMap<Destination, DestinationModel>().ReverseMap();
            CreateMap<Destination, DestinationCreateModel>().ReverseMap();
            CreateMap<Destination, DestinationUpdateModel>().ReverseMap();

            //ServiceUsedByTicket
            CreateMap<ServiceUsedByTicket, ServiceUsedByTicketModel>().ReverseMap();
            CreateMap<ServiceUsedByTicket, ServiceUsedByTicketCreateModel>().ReverseMap();
            CreateMap<ServiceUsedByTicket, ServiceUsedByTicketUpdateModel>().ReverseMap();

            //Booking
            CreateMap<Booking, BookingModel>().ReverseMap();
            CreateMap<Booking, BookingCreateModel>().ReverseMap();
            CreateMap<Booking, BookingUpdateModel>().ReverseMap();

            //Revenue
            CreateMap<Revenue, RevenueModel>().ReverseMap();
            CreateMap<Revenue, RevenueCreateModel>().ReverseMap();
            CreateMap<Revenue, RevenueUpdateModel>().ReverseMap();

            //BookingByRevenue
            CreateMap<BookingByRevenue, BookingByRevenueModel>().ReverseMap();
            CreateMap<BookingByRevenue, BookingByRevenueCreateModel>().ReverseMap();
            CreateMap<BookingByRevenue, BookingByRevenueUpdateModel>().ReverseMap();

            //Feedback
            CreateMap<Feedback, FeedbackModel>().ReverseMap();
            CreateMap<Feedback, FeedbackCreateModel>().ReverseMap();
            CreateMap<Feedback, FeedbackUpdateModel>().ReverseMap();

        }
    }
}
