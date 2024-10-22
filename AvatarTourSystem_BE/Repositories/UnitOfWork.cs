using AutoMapper;
using Azure.Core;
using BusinessObjects.Models;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AvatarTourDBContext _context = new AvatarTourDBContext();
        private readonly IMapper _mapper;
        private GenericRepository<Account> _accountRepository;
        private GenericRepository<Booking> _bookingtRepository;
     //   private GenericRepository<BookingByRevenue> _bookingByRevenueRepository;
        private GenericRepository<City> _cityRepository;
        private GenericRepository<CustomerSupport> _customerSupportRepository;
        private GenericRepository<DailyTicket> _dailyTicketRepository;
        private GenericRepository<DailyTour> _dailyTourRepository;
        private GenericRepository<Destination> _destinationRepository;  
        private GenericRepository<Feedback> _feedbackRepository;
        private GenericRepository<Location> _locationRepository;
        private GenericRepository<Notification> _notificationRepository;
        private GenericRepository<PackageTour> _packageTourRepository;
        private GenericRepository<PaymentMethod> _paymentMethodRepository;
        private GenericRepository<PointOfInterest> _pointOfInterestRepository;
        //private GenericRepository<POIType> _poiTypeRepository;
        private GenericRepository<Rate> _rateRepository;
        private GenericRepository<RequestType> _requestTypeRepository;
     //   private GenericRepository<Revenue> _revenueRepository;
        private GenericRepository<Service> _serviceRepository;
        private GenericRepository<ServiceByTourSegment> _serviceByTourSegmentRepository;
        private GenericRepository<ServiceType> _serviceTypeRepository;
        private GenericRepository<ServiceUsedByTicket> _serviceUsedByTicketRepository;
        private GenericRepository<Supplier> _supplierRepository;
        private GenericRepository<Ticket> _ticketRepository;
        private GenericRepository<TicketType> _ticketTypeRepository;
        private GenericRepository<TourSegment> _tourSegmentRepository;
        private GenericRepository<TransactionsHistory> _transactionsHistoryRepository;


        public UnitOfWork(AvatarTourDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public GenericRepository<Account> AccountRepository
        {
            get
            {
                if (this._accountRepository == null)
                {
                    this._accountRepository = new GenericRepository<Account>(_context);
                }
                return _accountRepository;
            }
        }
        public GenericRepository<Booking> BookingRepository
        {
            get
            {
                if (this._bookingtRepository == null)
                {
                    this._bookingtRepository = new GenericRepository<Booking>(_context);
                }
                return _bookingtRepository;
            }
        }

        //public GenericRepository<BookingByRevenue> BookingByRevenueRepository
        //{
        //    get
        //    {
        //        if (this._bookingByRevenueRepository == null)
        //        {
        //            this._bookingByRevenueRepository = new GenericRepository<BookingByRevenue>(_context);
        //        }
        //        return _bookingByRevenueRepository;
        //    }
        //}

        public GenericRepository<City> CityRepository
        {
            get
            {
                if (this._cityRepository == null)
                {
                    this._cityRepository = new GenericRepository<City>(_context);
                }
                return _cityRepository;
            }
        }

        public GenericRepository<CustomerSupport> CustomerSupportRepository
        {
            get
            {
                if (this._customerSupportRepository == null)
                {
                    this._customerSupportRepository = new GenericRepository<CustomerSupport>(_context);
                }
                return _customerSupportRepository;
            }
        }

        public GenericRepository<DailyTicket> DailyTicketRepository
        {
            get
            {
                if (this._dailyTicketRepository == null)
                {
                    this._dailyTicketRepository = new GenericRepository<DailyTicket>(_context);
                }
                return _dailyTicketRepository;
            }
        }

        public GenericRepository<DailyTour> DailyTourRepository
        {
            get
            {
                if (this._dailyTourRepository == null)
                {
                    this._dailyTourRepository = new GenericRepository<DailyTour>(_context);
                }
                return _dailyTourRepository;
            }
        }

        public GenericRepository<Destination> DestinationRepository
        {
            get
            {
                if (this._destinationRepository == null)
                {
                    this._destinationRepository = new GenericRepository<Destination>(_context);
                }
                return _destinationRepository;
            }
        }

        public GenericRepository<Feedback> FeedbackRepository
        {
            get
            {
                if (this._feedbackRepository == null)
                {
                    this._feedbackRepository = new GenericRepository<Feedback>(_context);
                }
                return _feedbackRepository;
            }
        }

        public GenericRepository<Location> LocationRepository
        {
            get
            {
                if (this._locationRepository == null)
                {
                    this._locationRepository = new GenericRepository<Location>(_context);
                }
                return _locationRepository;
            }
        }

        public GenericRepository<Notification> NotificationRepository
        {
            get
            {
                if (this._notificationRepository == null)
                {
                    this._notificationRepository = new GenericRepository<Notification>(_context);
                }
                return _notificationRepository;
            }
        }

        public GenericRepository<PackageTour> PackageTourRepository
        {
            get
            {
                if (this._packageTourRepository == null)
                {
                    this._packageTourRepository = new GenericRepository<PackageTour>(_context);
                }
                return _packageTourRepository;
            }
        }

        public GenericRepository<PaymentMethod> PaymentMethodRepository
        {
            get
            {
                if (this._paymentMethodRepository == null)
                {
                    this._paymentMethodRepository = new GenericRepository<PaymentMethod>(_context);
                }
                return _paymentMethodRepository;
            }
        }

        public GenericRepository<PointOfInterest> PointOfInterestRepository
        {
            get
            {
                if (this._pointOfInterestRepository == null)
                {
                    this._pointOfInterestRepository = new GenericRepository<PointOfInterest>(_context);
                }
                return _pointOfInterestRepository;
            }
        }

        //public GenericRepository<POIType> POITypeRepository
        //{
        //    get
        //    {
        //        if (this._poiTypeRepository == null)
        //        {
        //            this._poiTypeRepository = new GenericRepository<POIType>(_context);
        //        }
        //        return _poiTypeRepository;
        //    }
        //}

        public GenericRepository<Rate> RateRepository
        {
            get
            {
                if (this._rateRepository == null)
                {
                    this._rateRepository = new GenericRepository<Rate>(_context);
                }
                return _rateRepository;
            }
        }

        public GenericRepository<RequestType> RequestTypeRepository
        {
            get
            {
                if (this._requestTypeRepository == null)
                {
                    this._requestTypeRepository = new GenericRepository<RequestType>(_context);
                }
                return _requestTypeRepository;
            }
        }

        //public GenericRepository<Revenue> RevenueRepository
        //{
        //    get
        //    {
        //        if (this._revenueRepository == null)
        //        {
        //            this._revenueRepository = new GenericRepository<Revenue>(_context);
        //        }
        //        return _revenueRepository;
        //    }
        //}

        public GenericRepository<Service> ServiceRepository
        {
            get
            {
                if (this._serviceRepository == null)
                {
                    this._serviceRepository = new GenericRepository<Service>(_context);
                }
                return _serviceRepository;
            }
        }

        public GenericRepository<ServiceByTourSegment> ServiceByTourSegmentRepository
        {
            get
            {
                if (this._serviceByTourSegmentRepository == null)
                {
                    this._serviceByTourSegmentRepository = new GenericRepository<ServiceByTourSegment>(_context);
                }
                return _serviceByTourSegmentRepository;
            }
        }

        public GenericRepository<ServiceType> ServiceTypeRepository
        {
            get
            {
                if (this._serviceTypeRepository == null)
                {
                    this._serviceTypeRepository = new GenericRepository<ServiceType>(_context);
                }
                return _serviceTypeRepository;
            }
        }

        public GenericRepository<ServiceUsedByTicket> ServiceUsedByTicketRepository
        {
            get
            {
                if (this._serviceUsedByTicketRepository == null)
                {
                    this._serviceUsedByTicketRepository = new GenericRepository<ServiceUsedByTicket>(_context);
                }
                return _serviceUsedByTicketRepository;
            }
        }

        public GenericRepository<Supplier> SupplierRepository
        {
            get
            {
                if (this._supplierRepository == null)
                {
                    this._supplierRepository = new GenericRepository<Supplier>(_context);
                }
                return _supplierRepository;
            }
        }

        public GenericRepository<Ticket> TicketRepository
        {
            get
            {
                if (this._ticketRepository == null)
                {
                    this._ticketRepository = new GenericRepository<Ticket>(_context);
                }
                return _ticketRepository;
            }
        }

        public GenericRepository<TicketType> TicketTypeRepository
        {
            get
            {
                if (this._ticketTypeRepository == null)
                {
                    this._ticketTypeRepository = new GenericRepository<TicketType>(_context);
                }
                return _ticketTypeRepository;
            }
        }

        public GenericRepository<TourSegment> TourSegmentRepository
        {
            get
            {
                if (this._tourSegmentRepository == null)
                {
                    this._tourSegmentRepository = new GenericRepository<TourSegment>(_context);
                }
                return _tourSegmentRepository;
            }
        }

        public GenericRepository<TransactionsHistory> TransactionsHistoryRepository
        {
            get
            {
                if (this._transactionsHistoryRepository == null)
                {
                    this._transactionsHistoryRepository = new GenericRepository<TransactionsHistory>(_context);
                }
                return _transactionsHistoryRepository;
            }
        }
        public int Save() 
        {
           return _context.SaveChanges();
        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing) 
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
    //This method dynamically combines two LINQ expressions with a logical AND (gộp 2 thằng linq = method AND)
    public static class ExpressionExtensions
    {
        public static Expression<Func<T, bool>> And<T>(
            this Expression<Func<T, bool>> left,
            Expression<Func<T, bool>> right)
        {
            if (left == null)
                return right;
            var invokedExpr = Expression.Invoke(right, left.Parameters);
            return Expression.Lambda<Func<T, bool>>(
                Expression.AndAlso(left.Body, invokedExpr), left.Parameters);
        }
    }
}
