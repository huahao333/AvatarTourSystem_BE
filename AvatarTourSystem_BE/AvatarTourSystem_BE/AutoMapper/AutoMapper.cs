﻿using AutoMapper;
using BusinessObjects.Models;
using BusinessObjects.ViewModels.Account;
using BusinessObjects.ViewModels.Booking;
using BusinessObjects.ViewModels.City;
using BusinessObjects.ViewModels.CustomerSupport;
using BusinessObjects.ViewModels.DailyTicket;
using BusinessObjects.ViewModels.DailyTour;
using BusinessObjects.ViewModels.Destination;
using BusinessObjects.ViewModels.Feedback;
using BusinessObjects.ViewModels.Location;
using BusinessObjects.ViewModels.Notification;
using BusinessObjects.ViewModels.PackageTour;
using BusinessObjects.ViewModels.PackageTourFlow;
using BusinessObjects.ViewModels.PackageTourFlow.PackageTourGet;
using BusinessObjects.ViewModels.PaymentMethod;
using BusinessObjects.ViewModels.POI;
using BusinessObjects.ViewModels.Rate;
using BusinessObjects.ViewModels.ResquestType;
using BusinessObjects.ViewModels.Service;
using BusinessObjects.ViewModels.ServiceByTourSegment;
using BusinessObjects.ViewModels.ServiceType;
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
            CreateMap<DailyTicketType, DailyTicketModel>().ReverseMap();
            CreateMap<DailyTicketType, DailyTicketCreateModel>().ReverseMap();
            CreateMap<DailyTicketType, DailyTicketUpdateModel>().ReverseMap();

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
     //      CreateMap<Destination, DestinationCreateModel>().ReverseMap();
            CreateMap<Destination, DestinationUpdateModel>().ReverseMap();
            
            //Location
            CreateMap<Location, POIModel>().ReverseMap();
            CreateMap<Location, LocationCreateModel>().ReverseMap();
            CreateMap<Location, LocationUpdateModel>().ReverseMap();

            //Service
            CreateMap<Service, ServiceModel>().ReverseMap();
            CreateMap<Service, ServiceCreateModel>().ReverseMap();
            CreateMap<Service, ServiceUpdateModel>().ReverseMap();
            
            //ServiceType
            CreateMap<ServiceType, ServiceTypeModel>().ReverseMap();
            CreateMap<ServiceType, ServiceTypeCreateModel>().ReverseMap();
            CreateMap<ServiceType, ServiceTypeUpdateModel>().ReverseMap();
            
            //ServiceByTourSegment
            CreateMap<ServiceByTourSegment, ServiceByTourSegmentModel>().ReverseMap();
            CreateMap<ServiceByTourSegment, ServiceByTourSegmentCreateModel>().ReverseMap();
            CreateMap<ServiceByTourSegment, ServiceByTourSegmentUpdateModel>().ReverseMap();
            
            //ServiceUsedByTicket
            CreateMap<ServiceUsedByTicket, ServiceUsedByTicketModel>().ReverseMap();
            CreateMap<ServiceUsedByTicket, ServiceUsedByTicketCreateModel>().ReverseMap();
            CreateMap<ServiceUsedByTicket, ServiceUsedByTicketUpdateModel>().ReverseMap();

            //Booking
            CreateMap<Booking, BookingModel>().ReverseMap();
            CreateMap<Booking, BookingCreateModel>().ReverseMap();
            CreateMap<Booking, BookingUpdateModel>().ReverseMap();
            CreateMap<Booking, BookingFlowModel>().ReverseMap();

            //Revenue
            //CreateMap<Revenue, RevenueModel>().ReverseMap();
            //CreateMap<Revenue, RevenueCreateModel>().ReverseMap();
            //CreateMap<Revenue, RevenueUpdateModel>().ReverseMap();

            //BookingByRevenue
            //CreateMap<BookingByRevenue, BookingByRevenueModel>().ReverseMap();
            //CreateMap<BookingByRevenue, BookingByRevenueCreateModel>().ReverseMap();
            //CreateMap<BookingByRevenue, BookingByRevenueUpdateModel>().ReverseMap();

            //Feedback
            CreateMap<Feedback, FeedbackModel>().ReverseMap();
            CreateMap<Feedback, FeedbackCreateModel>().ReverseMap();
            CreateMap<Feedback, FeedbackUpdateModel>().ReverseMap();
            CreateMap<FeedbackCreateWithZaloModel, Feedback>()
           .ForMember(dest => dest.FeedbackMsg, opt => opt.MapFrom(src => src.FeedbackMsg))
           .ForMember(dest => dest.BookingId, opt => opt.MapFrom(src => src.BookingId))
           .ForMember(dest => dest.UserId, opt => opt.Ignore())  
           .ForMember(dest => dest.FeedbackId, opt => opt.Ignore())  
           .ForMember(dest => dest.CreateDate, opt => opt.Ignore())  
           .ForMember(dest => dest.UpdateDate, opt => opt.Ignore()) 
           .ForMember(dest => dest.Status, opt => opt.Ignore())    
           .ForMember(dest => dest.Accounts, opt => opt.Ignore())   
           .ForMember(dest => dest.Bookings, opt => opt.Ignore());  

            //Rate
            CreateMap<Rate, RateModel>().ReverseMap();
            CreateMap<Rate, RateCreateModel>().ReverseMap();
            CreateMap<Rate, RateUpdateModel>().ReverseMap();
            CreateMap<Rate, RateCreateWithZaloModel>().ReverseMap();

            //Account
            CreateMap<Account, AccountModel>().ReverseMap();
            CreateMap<Account, AccountCreateModel>() .ReverseMap();
            CreateMap<Account, AccountUpdateModel>() .ReverseMap();
            CreateMap<Account,AccountSignUpModel>() .ReverseMap();
            CreateMap<Account, AccountViewModel>().ReverseMap();
            CreateMap<Account, AccountViewModel>().ReverseMap();
            CreateMap<Account, AccountZaloIdModel>().ReverseMap();
            CreateMap<Account, AccountUpdateWithZaloIdModel>().ReverseMap();

            //CustomerSupport
            CreateMap<CustomerSupport, CustomerSupportModel>().ReverseMap();
            CreateMap<CustomerSupport, CustomerSupportCreateModel>().ReverseMap();
            CreateMap<CustomerSupport, CustomerSupportUpdateModel>().ReverseMap();

            //RequestType
            CreateMap<RequestType, RequestTypeModel>().ReverseMap();
            CreateMap<RequestType, RequestTypeCreateModel>().ReverseMap();
            CreateMap<RequestType, RequestTypeUpdateModel>().ReverseMap();

            //PointOfInterest
            CreateMap<PointOfInterest, POIModel>().ReverseMap();
            CreateMap<PointOfInterest, POICreateModel>().ReverseMap();
            CreateMap<PointOfInterest, POIUpdateModel>().ReverseMap();

            ////PointOfInterestTypes
            //CreateMap<POIType, POITypeModel>().ReverseMap();
            //CreateMap<POIType, POITypeCreateModel>().ReverseMap();
            //CreateMap<POIType, POITypeUpdateModel>().ReverseMap();

            //Notification
            CreateMap<Notification, NotificationModel>().ReverseMap();
            CreateMap<Notification, NotificationCreateModel>().ReverseMap();
            CreateMap<Notification, NotificationUpdateModel>().ReverseMap();

            //DailyTour
            CreateMap<DailyTour, DailyTourModel>().ReverseMap();
            CreateMap<DailyTour, DailyTourCreateModel>().ReverseMap();
            CreateMap<DailyTour, DailyTourUpdateModel>().ReverseMap();

            //Flow-Packagetour
            CreateMap<City, GetDestinationByCityModel>().ReverseMap();
            CreateMap<Destination, GetLocationByDestinationModel>().ReverseMap();
            CreateMap<Location, GetServiceByLocationModel>().ReverseMap();

            #region
            //       CreateMap<FPackageTourCreateModel, PackageTour>()
            //.ForMember(dest => dest.CreateDate, opt => opt.MapFrom(src => DateTime.Now))
            //.ForMember(dest => dest.UpdateDate, opt => opt.MapFrom(src => DateTime.Now))
            //.ForMember(dest => dest.TourSegments, opt => opt.MapFrom(src => src.Destinations.Select(d => new TourSegment
            //{
            //    DestinationId = d.DestinationId,
            //    Status = 1, // Set default status
            //    CreateDate = DateTime.Now,
            //    UpdateDate = DateTime.Now,
            //    ServiceByTourSegments = d.Locations.SelectMany(l => l.Services.Select(s => new ServiceByTourSegment
            //    {
            //        ServiceId = s.ServiceId,
            //        Status = 1, // Set default status
            //        CreateDate = DateTime.Now,
            //        UpdateDate = DateTime.Now
            //    })).ToList()
            //})));
            CreateMap<FPackageTourCreateModel, PackageTour>()
    .ForMember(dest => dest.CreateDate, opt => opt.MapFrom(src => DateTime.Now))
    .ForMember(dest => dest.UpdateDate, opt => opt.MapFrom(src => DateTime.Now))
    .ForMember(dest => dest.TicketTypes, opt => opt.MapFrom(src => src.TicketTypesCreate.Select(d => new TicketType
    {
        TicketTypeId = d.TicketTypeId,
        Status = 1, // Set default status
        CreateDate = DateTime.Now,
        UpdateDate = DateTime.Now,
       
    })));

            // Map từ DestinationModel sang Destination Entity
            CreateMap<DestinationModel, Destination>()
                .ForMember(dest => dest.CreateDate, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.UpdateDate, opt => opt.MapFrom(src => DateTime.Now));

            // Map từ LocationModel sang Location Entity
            CreateMap<LocationModel, Location>()
                .ForMember(dest => dest.CreateDate, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.UpdateDate, opt => opt.MapFrom(src => DateTime.Now));

            // Map từ ServiceModel sang Service Entity
            CreateMap<ServiceModel, Service>()
                .ForMember(dest => dest.CreateDate, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.UpdateDate, opt => opt.MapFrom(src => DateTime.Now));
            CreateMap<PackageTour, FPackageTourResponseModel>()
            .ForMember(dest => dest.Destinations, opt => opt.MapFrom(src => src.TourSegments.Select(ts => ts.Destinations)));

            CreateMap<Destination, FDestinationResponseModel>();
            CreateMap<Location, FLocationResponseModel>();
            CreateMap<Service, FServiceResponseModel>();
            #endregion
        }
    }
}
