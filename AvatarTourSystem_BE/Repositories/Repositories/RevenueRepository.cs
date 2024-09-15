using BusinessObjects.Models;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories
{
    public class RevenueRepository : GenericRepository<Revenue>, IRevenueRepository
    {
        public RevenueRepository(AvatarTourDBContext context) :base(context)
        {
            
        }
    }
}
