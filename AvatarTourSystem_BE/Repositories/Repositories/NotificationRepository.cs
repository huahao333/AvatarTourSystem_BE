using BusinessObjects.Models;
using Google.Apis.Storage.v1.Data;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories
{
    public class NotificationRepository : GenericRepository<Notifications>, INotificationRepository
    {
        public NotificationRepository(AvatarTourDBContext context) : base(context)
        {

        }
    }
}
