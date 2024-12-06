using AutoMapper;
using BusinessObjects.Enums;
using BusinessObjects.Models;
using BusinessObjects.ViewModels.PaymentMethod;
using BusinessObjects.ViewModels.TransactionHistory;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;
using Services.Common;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZXing;

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
            if (transactionsHistory == null)
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

        public async Task<APIResponseModel> GetTransactionsHistoryByZaloId(GetTransactionHistory getTransactionHistory)
        {
            try
            {
                var user = await _unitOfWork.AccountRepository.GetFirstOrDefaultAsync(query => query.Where(a => a.ZaloUser == getTransactionHistory.ZaloId));
                if (user == null)
                {
                    return new APIResponseModel
                    {
                        Message = "User not found",
                        IsSuccess = false
                    };
                }

                var transactions = await _unitOfWork.TransactionsHistoryRepository.GetAllAsyncs(query => query
                                                                 .Where(t => t.UserId == user.Id)
                                                                     .Include(b => b.Bookings)
                                                                          .ThenInclude(p => p.Payments)
                                                                     .Include(b => b.Bookings)
                                                                           .ThenInclude(d => d.DailyTours));

                var result = transactions.Select(t => new
                {
                    TransactionId = t.TransactionId,
                    UserId = t.UserId,
                    FullName = user.FullName,
                    StatusTransaction = t.Status,
                    OrderId = t.OrderId,
                    UpdateTime = t.UpdateDate,
                    Time = (t.UpdateDate ?? t.CreateDate)?.Date,
                    BookingId = t.BookingId,
                    DailyTourId = t.Bookings.DailyTourId,
                    DailyTourName = t.Bookings.DailyTours.DailyTourName,
                    Bookings = t.Bookings.Payments.Where(c => c.BookingId == t.BookingId && c.Status!=5).Select(c => new
                    {
                        TotalAmount = c.Amount,
                        ResultCode = c.ResultCode,

                    }),
                }).ToList();

                return new APIResponseModel
                {
                    Message = "Find Transaction",
                    IsSuccess = true,
                    Data = result
                };

            }
            catch (Exception ex)
            {
                return new APIResponseModel
                {
                    Message = "Error Transaction",
                    IsSuccess = false
                };
            }
        }

        public async Task<APIResponseModel> GetTransactionsHistoryRefundByZaloId(GetTransactionHistory getTransactionHistory)
        {
            try
            {
                var user = await _unitOfWork.AccountRepository.GetFirstOrDefaultAsync(query => query.Where(a => a.ZaloUser == getTransactionHistory.ZaloId));
                if (user == null)
                {
                    return new APIResponseModel
                    {
                        Message = "User not found",
                        IsSuccess = false
                    };
                }

                var transactions = await _unitOfWork.TransactionsHistoryRepository.GetAllAsyncs(query => query
                                                                 .Where(t => t.UserId == user.Id && t.Status == 5)
                                                                     .Include(b => b.Bookings)
                                                                          .ThenInclude(p => p.Payments)
                                                                     .Include(b => b.Bookings)
                                                                           .ThenInclude(d => d.DailyTours));

                var result = transactions.Select(t => new
                {
                    TransactionId = t.TransactionId,
                    UserId = t.UserId,
                    FullName = user.FullName,
                    StatusTransaction = t.Status,
                    OrderId = t.OrderId,
                    UpdateTime = t.UpdateDate,
                    Time = (t.UpdateDate ?? t.CreateDate)?.Date,
                    BookingId = t.BookingId,
                    DailyTourId = t.Bookings.DailyTourId,
                    DailyTourName = t.Bookings.DailyTours.DailyTourName,
                    Bookings = t.Bookings.Payments.Where(c => c.BookingId == t.BookingId && c.Status == 5).Select(c => new
                    {
                        TotalAmount = c.Amount,
                        ResultCode = c.ResultCode,

                    }),
                }).ToList();

                return new APIResponseModel
                {
                    Message = "Find Transaction",
                    IsSuccess = true,
                    Data = result
                };

            }
            catch (Exception ex)
            {
                return new APIResponseModel
                {
                    Message = "Error Transaction",
                    IsSuccess = false
                };
            }
        }
    }
}
