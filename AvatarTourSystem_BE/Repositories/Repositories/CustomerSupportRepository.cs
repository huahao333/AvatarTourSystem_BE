using BusinessObjects.Models;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories
{
    public class CustomerSupportRepository : GenericRepository<CustomerSupport>, ICustomerSupportRepository
    {
        public CustomerSupportRepository(AvatarTourDBContext context) : base(context)
        {
        }
    }
}
