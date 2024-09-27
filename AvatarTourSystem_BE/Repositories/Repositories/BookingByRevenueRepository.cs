using BusinessObjects.Models;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories
{
    public class BookingByRevenueRepository : GenericRepository<BookingByRevenue>, IBookingByRevenueRepository
    {
        public BookingByRevenueRepository(AvatarTourDBContext context) : base(context)
        {
            
        }
    }
}
