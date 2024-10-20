using AutoMapper;
using BusinessObjects.Enums;
using BusinessObjects.Models;
using BusinessObjects.ViewModels.Booking;
using BusinessObjects.ViewModels.DailyTour;
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
    public class BookingFlowService : IBookingFlowService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public BookingFlowService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<APIResponseModel> CreateBookingFlowAsync(BookingCreateModel createModel)
        {
            try
            {
                var newBookingId = Guid.NewGuid();
                var user = await _unitOfWork.AccountRepository.GetByIdStringAsync(createModel.UserId);
                if (user == null || string.IsNullOrEmpty(createModel.UserId))
                {
                    return new APIResponseModel
                    {
                        Message = "User must exist.",
                        IsSuccess = false,
                    };
                }

                if (createModel.TotalPrice <= 0)
                {
                    return new APIResponseModel
                    {
                        Message = "Price must be greater than 0.",
                        IsSuccess = false,
                    };
                }

                var booking = new Booking
                {
                    BookingId = newBookingId.ToString(),
                    UserId = createModel.UserId,
                    BookingDate = DateTime.Now,
                    DailyTourId = createModel.DailyTourId,
                    PaymentId = createModel.PaymentId,
                    TotalPrice = createModel.TotalPrice,
                    CreateDate = DateTime.Now,
                    Status = (int?)EStatus.IsPending
                };
                //var bookingByRevenue = new BookingByRevenue 
                //{

                //};
                await _unitOfWork.BookingRepository.AddAsync(booking);
                _unitOfWork.Save();
                return new APIResponseModel
                {
                    Message = "Booking created successfully",
                    IsSuccess = true,
                    Data = booking
                };
            }
            catch (Exception ex)
            {
                return new APIResponseModel
                {
                    Message = ex.Message,
                    IsSuccess = false,
                };
            }

        }

        public Task<APIResponseModel> GetBookingFlowAsync()
        {
            throw new NotImplementedException();
        }

        public Task<APIResponseModel> GetBookingFlowByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<APIResponseModel> UpdateBookingByZaloIdFlowAsync(BookingModel updateModel)
        {
            var zaloAccount = await _unitOfWork.AccountRepository.GetFirstOrDefaultAsync(query => query
                    .Where(a => a.Id == updateModel.UserId && (a.Status == 1 && a.ZaloUser != null ) ));
                
            if (zaloAccount == null)
            {
                return new APIResponseModel
                {
                    Message = "Account not found.",
                    IsSuccess = false,
                };
            }
            var booking = await _unitOfWork.BookingRepository.GetFirstOrDefaultAsync(query => query
                    .Where(b => b.UserId == zaloAccount.Id && b.Status == 1));
            if (booking == null)
            {
                return new APIResponseModel
                {
                    Message = "Booking not found for the given account.",
                    IsSuccess = false
                };
            }

            booking.BookingDate = updateModel.BookingDate;
            booking.ExpirationDate = updateModel.ExpirationDate;
            booking.TotalPrice = updateModel.TotalPrice;
            booking.Status = updateModel.Status;
            booking.UpdateDate = DateTime.UtcNow;

            _unitOfWork.BookingRepository.UpdateAsync(booking);
            _unitOfWork.Save();

            return new APIResponseModel
            {
                Message = "Booking updated successfully.",
                IsSuccess = true,
                //Data = booking 
            };
        }

        public Task<APIResponseModel> UpdateBookingFlowAsync(BookingUpdateModel updateModel)
        {
            throw new NotImplementedException();
        }
    }
}
