using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        GenericRepository<Supplier> SupplierRepository { get; }
        GenericRepository<TourSegment> TourSegmentRepository { get; }
        GenericRepository<PackageTour> PackageTourRepository { get; }
        GenericRepository<TicketType> TicketTypeRepository { get; }
        GenericRepository<DailyTicketType> DailyTicketRepository { get; }
        GenericRepository<Ticket> TicketRepository { get; }
        GenericRepository<Payment> PaymentRepository { get; }
        GenericRepository<PaymentMethod> PaymentMethodRepository { get; }
        GenericRepository<TransactionsHistory> TransactionsHistoryRepository { get; }
        GenericRepository<City> CityRepository { get; }
        GenericRepository<Destination> DestinationRepository { get; }
        GenericRepository<Location> LocationRepository { get; }
        GenericRepository<Service> ServiceRepository { get; }
        GenericRepository<ServiceType> ServiceTypeRepository { get; }
        GenericRepository<ServiceByTourSegment> ServiceByTourSegmentRepository { get; }
        GenericRepository<ServiceUsedByTicket> ServiceUsedByTicketRepository { get; }
        GenericRepository<Booking> BookingRepository { get; }
        //GenericRepository<Revenue> RevenueRepository { get; }
        //GenericRepository<BookingByRevenue> BookingByRevenueRepository { get; }
        GenericRepository<Feedback> FeedbackRepository { get; }
        GenericRepository<Rate> RateRepository { get; }
        GenericRepository<Account> AccountRepository { get; }
        GenericRepository<CustomerSupport> CustomerSupportRepository { get; }
        GenericRepository<RequestType> RequestTypeRepository { get; }
        GenericRepository<PointOfInterest> PointOfInterestRepository { get; }
        //GenericRepository<POIType> POITypeRepository { get; }
        GenericRepository<Notification> NotificationRepository { get; }
        GenericRepository<DailyTour> DailyTourRepository { get; }
        //làm table nào thêm repo của table đó 

        IDbContextTransaction BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
        int Save();
    }
}
