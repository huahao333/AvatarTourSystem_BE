using BusinessObjects.Models;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Repositories.Repositories
{
    public class TransactionHistoryRepository : GenericRepository<TransactionsHistory>, ITransactionHistoryRepository
    {
        public TransactionHistoryRepository(AvatarTourDBContext context) : base(context)
        {

        }
    }
    
}
