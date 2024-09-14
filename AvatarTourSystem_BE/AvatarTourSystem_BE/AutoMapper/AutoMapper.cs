using AutoMapper;
using BusinessObjects.Models;
using BusinessObjects.ViewModels.DailyTicket;
using BusinessObjects.ViewModels.PackageTour;
using BusinessObjects.ViewModels.PaymentMethod;
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

        }
    }
}
