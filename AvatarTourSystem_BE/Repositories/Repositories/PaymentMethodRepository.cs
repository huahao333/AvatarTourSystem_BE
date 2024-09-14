using BusinessObjects.Models;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories
{
    public class PaymentMethodRepository : GenericRepository<PaymentMethod>, IPaymentMethodRepository
    {
        public PaymentMethodRepository(AvatarTourDBContext context) : base(context)
        {

        }
    }
}
