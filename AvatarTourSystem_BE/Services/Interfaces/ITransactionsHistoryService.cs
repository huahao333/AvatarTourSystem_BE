using BusinessObjects.ViewModels.TransactionHistory;
using Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface ITransactionsHistoryService
    {
        Task<APIResponseModel> GetAllTransactionsHistory();
        Task<APIResponseModel> GetTransactionsHistoryByUserId(string userId);
        Task<APIResponseModel> GetTransactionsHistoryByBookingId(string bookingId);
        Task<APIResponseModel> GetTransactionsHistoryById(string id);
        Task<APIResponseModel> GetTransactionsHistoryByStatus();
        Task<APIResponseModel> CreateTransactionsHistory(TransactionHistoryCreateModel transactionsHistoryCreateModel);
        Task<APIResponseModel> UpdateTransactionsHistory(TransactionHistoryUpdateModel transactionsHistoryUpdateModel);
        Task<APIResponseModel> DeleteTransactionsHistory(string id);
        Task<APIResponseModel> GetTransactionsHistoryByZaloId(GetTransactionHistory getTransactionHistory);
        Task<APIResponseModel> GetTransactionsHistoryRefundByZaloId(GetTransactionHistory getTransactionHistory);
    }
}
