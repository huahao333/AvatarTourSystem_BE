using BusinessObjects.Models;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories
{
    public class ServiceTypeRepository : GenericRepository<ServiceType>, IServiceTypeRepository
    {
        public ServiceTypeRepository(AvatarTourDBContext context) : base(context)
        {
        }
    }
}
