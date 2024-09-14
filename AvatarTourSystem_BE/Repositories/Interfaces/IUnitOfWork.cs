using BusinessObjects.Models;
using System;
using System.Collections.Generic;
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
        GenericRepository<DailyTicket> DailyTicketRepository { get; }
        GenericRepository<PaymentMethod> PaymentMethodRepository { get; }
        //làm table nào thêm repo của table đó 
        int Save();
    }
}
