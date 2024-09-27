using BusinessObjects.Models;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories
{
    public class DailyTicketRepository : GenericRepository<DailyTicket>, IDailyTicketRepository
    {
        public DailyTicketRepository(AvatarTourDBContext context) : base(context)
        {
            
        }
    }
}
