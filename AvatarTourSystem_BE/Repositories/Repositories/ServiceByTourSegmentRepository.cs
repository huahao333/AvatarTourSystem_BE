using BusinessObjects.Models;
using Repositories.Interfaces;

namespace Repositories.Repositories
{
    public class ServiceByTourSegmentRepository : GenericRepository<ServiceByTourSegment>, IServiceByTourSegmentRepository
    {
        public ServiceByTourSegmentRepository(AvatarTourDBContext context) : base(context)
        {
        }
    }
}
