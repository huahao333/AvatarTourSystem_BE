using AutoMapper;
using BusinessObjects.Enums;
using BusinessObjects.Models;
using BusinessObjects.ViewModels.Booking;
using BusinessObjects.ViewModels.ServiceUsedByTicket;
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
    public class BookingService : IBookingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public BookingService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<APIResponseModel> GetBookingsAsync()
        {
            var list = await _unitOfWork.BookingRepository.GetAllAsync();
            var count = list.Count();
            return new APIResponseModel
            {
                Message = $" Found {count} Booking ",
                IsSuccess = true,
                Data = list,
            };
        }
        public async Task<APIResponseModel> GetActiveBookingsAsync()
        {
            var list = await _unitOfWork.BookingRepository.GetByConditionAsync(s => s.Status != -1);
            var count = list.Count();
            return new APIResponseModel
            {
                Message = $" Found {count} Booking ",
                IsSuccess = true,
                Data = list,
            };
        }
        public async Task<APIResponseModel> GetBookingByIdAsync(string bookingId)
        {
            var booking = await _unitOfWork.BookingRepository.GetByIdStringAsync(bookingId);
            if (booking == null)
            {
                return new APIResponseModel
                {
                    Message = "Booking not found",
                    IsSuccess = false
                };
            }
            return new APIResponseModel
            {
                Message = "Booking found",
                IsSuccess = true,
                Data = booking,
            };
            // return _mapper.Map<BookingModel>(booking);
        }

        public async Task<APIResponseModel> CreateBookingAsync(BookingCreateModel createModel)
        {
            var booking = _mapper.Map<Booking>(createModel);
            booking.BookingId = Guid.NewGuid().ToString();
            booking.CreateDate = DateTime.Now;
            booking.BookingDate = DateTime.Now;
            var result = await _unitOfWork.BookingRepository.AddAsync(booking);
            _unitOfWork.Save();
            return new APIResponseModel
            {
                Message = " Booking Created Successfully",
                IsSuccess = true,
                Data = booking,
            };
        }
        public async Task<APIResponseModel> UpdateBookingAsync(BookingUpdateModel updateModel)
        {
            var existingBooking = await _unitOfWork.BookingRepository.GetByIdGuidAsync(updateModel.BookingId);

            if (existingBooking == null)
            {
                return new APIResponseModel
                {
                    Message = "Booking not found",
                    IsSuccess = false
                };
            }
            var createDate = existingBooking.CreateDate;
            var bookingDate = existingBooking.BookingDate;

            var booking = _mapper.Map(updateModel, existingBooking);
            booking.CreateDate = createDate;
            booking.UpdateDate = DateTime.Now;
            booking.BookingDate = bookingDate;

            var result = await _unitOfWork.BookingRepository.UpdateAsync(booking);
            _unitOfWork.Save();

            return new APIResponseModel
            {
                Message = "Booking Updated Successfully",
                IsSuccess = true,
                Data = booking,
            };
        }
        public async Task<APIResponseModel> DeleteBooking(string bookingId)
        {
            var booking = await _unitOfWork.BookingRepository.GetByIdStringAsync(bookingId);
            if (booking == null)
            {
                return new APIResponseModel
                {
                    Message = "Booking not found",
                    IsSuccess = false
                };
            }
            if (booking.Status == -1)
            {
                return new APIResponseModel
                {
                    Message = "Booking has been removed",
                    IsSuccess = false
                };
            }
            var createDate = booking.CreateDate;
            var bookingDate = booking.BookingDate;
            booking.Status = (int?)EStatus.IsDeleted;
            booking.CreateDate = createDate;
            booking.BookingDate = bookingDate;
            booking.UpdateDate = DateTime.Now;
            var result = await _unitOfWork.BookingRepository.UpdateAsync(booking);
            _unitOfWork.Save();
            return new APIResponseModel
            {
                Message = " Booking Deleted Successfully",
                IsSuccess = true,
                Data = booking,
            };
        }
    }
}
