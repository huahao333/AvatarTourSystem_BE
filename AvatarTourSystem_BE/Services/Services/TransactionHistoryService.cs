using AutoMapper;
using BusinessObjects.Enums;
using BusinessObjects.Models;
using BusinessObjects.ViewModels.PaymentMethod;
using BusinessObjects.ViewModels.TransactionHistory;
using Repositories.Interfaces;
using Services.Common;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class TransactionHistoryService : ITransactionsHistoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public TransactionHistoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<APIResponseModel> CreateTransactionsHistory(TransactionHistoryCreateModel transactionsHistoryCreateModel)
        {
            var transactionsHistory = _mapper.Map<TransactionsHistory>(transactionsHistoryCreateModel);
            transactionsHistory.TransactionId = Guid.NewGuid().ToString();
            transactionsHistory.CreateDate = DateTime.Now;
            await _unitOfWork.TransactionsHistoryRepository.AddAsync(transactionsHistory);
            _unitOfWork.Save();
            return new APIResponseModel
            {
                Message = "Create Transactions History Successfully",
                IsSuccess = true,
                Data = transactionsHistory,
            };
        }

        public async Task<APIResponseModel> DeleteTransactionsHistory(string id)
        {
            var transactionsHistory = await _unitOfWork.TransactionsHistoryRepository.GetByIdStringAsync(id);
            if(transactionsHistory == null)
            {
                return new APIResponseModel
                {
                    Message = "Transaction history not found.",
                    IsSuccess = false,
                    Data = null
                };
            }
            if (transactionsHistory.Status == (int?)EStatus.IsDeleted)
            {
                return new APIResponseModel
                {
                    Message = "Transaction history has already been deleted.",
                    IsSuccess = false,
                    Data = null
                };
            }
            transactionsHistory.Status = (int?)EStatus.IsDeleted;
            transactionsHistory.UpdateDate = DateTime.Now;
            var result = await _unitOfWork.TransactionsHistoryRepository.UpdateAsync(transactionsHistory);
            _unitOfWork.Save();
            return new APIResponseModel
            {
                Message = "Delete Transaction history Successfully",
                IsSuccess = true,
                Data = result,
            };
        }

        public async Task<APIResponseModel> GetAllTransactionsHistory()
        {
            var transactionsHistory = await _unitOfWork.TransactionsHistoryRepository.GetAllAsync();
            return new APIResponseModel
            {
                Message = "Get all transactions history successfully",
                IsSuccess = true,
                Data = transactionsHistory,
            };
        }

        public async Task<APIResponseModel> GetTransactionsHistoryByBookingId(string bookingId)
        {
            var transactions = await _unitOfWork.TransactionsHistoryRepository.GetByConditionAsync(x => x.BookingId == bookingId);
            if (transactions == null || !transactions.Any())
            {
                return new APIResponseModel
                {
                    Message = "Transaction history not found.",
                    IsSuccess = false,
                    Data = null
                };
            }
            var transactionHistoryViewModels = _mapper.Map<List<TransactionHistoryModel>>(transactions);
            return new APIResponseModel
            {
                Message = "Found transactions history successfully.",
                IsSuccess = true,
                Data = transactionHistoryViewModels,
            };
        }

        public async Task<APIResponseModel> GetTransactionsHistoryById(string id)
        {
            var transactions = await _unitOfWork.TransactionsHistoryRepository.GetByConditionAsync(x => x.TransactionId == id);
            if (transactions == null || !transactions.Any())
            {
                return new APIResponseModel
                {
                    Message = "Transaction history not found.",
                    IsSuccess = false,
                    Data = null
                };
            }
            var transactionHistoryViewModels = _mapper.Map<List<TransactionHistoryModel>>(transactions);
            return new APIResponseModel
            {
                Message = "Found transactions history successfully.",
                IsSuccess = true,
                Data = transactionHistoryViewModels,
            };

        }

        public async Task<APIResponseModel> GetTransactionsHistoryByStatus()
        {
            var transactions = await _unitOfWork.TransactionsHistoryRepository.GetByConditionAsync(s => s.Status != -1);
            if (transactions == null || !transactions.Any())
            {
                return new APIResponseModel
                {
                    Message = "Transaction history not found.",
                    IsSuccess = false,
                    Data = null
                };
            }
            return new APIResponseModel
            {
                 Message = "Transaction history not found.",
                 IsSuccess = false,
                 Data = null
            };      
        }

        public async Task<APIResponseModel> GetTransactionsHistoryByUserId(string userId)
        {
            var transactions = await _unitOfWork.TransactionsHistoryRepository.GetByConditionAsync(x => x.UserId == userId);
            if (transactions == null || !transactions.Any())
            {
                return new APIResponseModel
                {
                    Message = "Transaction history not found.",
                    IsSuccess = false,
                    Data = null
                };
            }
            var transactionHistoryViewModels = _mapper.Map<List<TransactionHistoryModel>>(transactions);
            return new APIResponseModel
            {
                Message = "Found transactions history successfully.",
                IsSuccess = true,
                Data = transactionHistoryViewModels,  
            };
        }


        public async Task<APIResponseModel> UpdateTransactionsHistory(TransactionHistoryUpdateModel transactionsHistoryUpdateModel)
        {
            var existingTransactionsHistory = await _unitOfWork.TransactionsHistoryRepository.GetByIdGuidAsync(transactionsHistoryUpdateModel.TransactionHistoryId);
            if (existingTransactionsHistory == null)
            {
                return new APIResponseModel
                {
                    Message = "Transaction history not found.",
                    IsSuccess = false,
                    Data = null
                };
            }
            var createdDate = existingTransactionsHistory.CreateDate;
            var transactionsHistory = _mapper.Map(transactionsHistoryUpdateModel, existingTransactionsHistory);
            transactionsHistory.CreateDate = createdDate;
            transactionsHistory.UpdateDate = DateTime.Now;
            var result = await _unitOfWork.TransactionsHistoryRepository.UpdateAsync(transactionsHistory);
            _unitOfWork.Save();
            return new APIResponseModel
            {
                Message = "Update Transaction history Successfully",
                IsSuccess = true,
                Data = result,
            };
        }
    }
}
