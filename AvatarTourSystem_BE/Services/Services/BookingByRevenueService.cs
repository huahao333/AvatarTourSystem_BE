using AutoMapper;
using BusinessObjects.Enums;
using BusinessObjects.Models;
using BusinessObjects.ViewModels.Booking;
using BusinessObjects.ViewModels.BookingByRevenue;
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
    public class BookingByRevenueService : IBookingByRevenueService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public BookingByRevenueService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<APIResponseModel> GetBookingByRevenuesAsync()
        {
            var list = await _unitOfWork.BookingByRevenueRepository.GetAllAsync();
            var count = list.Count();
            return new APIResponseModel
            {
                Message = $" Found {count} BookingByRevenue ",
                IsSuccess = true,
                Data = list,
            };
        }
        public async Task<APIResponseModel> GetActiveBookingByRevenuesAsync()
        {
            var list = await _unitOfWork.BookingByRevenueRepository.GetByConditionAsync(s => s.Status != -1);
            var count = list.Count();
            return new APIResponseModel
            {
                Message = $" Found {count} BookingByRevenue ",
                IsSuccess = true,
                Data = list,
            };
        }
        public async Task<APIResponseModel> GetBookingByRevenueByIdAsync(string bookingByRevenueId)
        {
            var bookingByRevenue = await _unitOfWork.BookingByRevenueRepository.GetByIdStringAsync(bookingByRevenueId);
            if (bookingByRevenue == null)
            {
                return new APIResponseModel
                {
                    Message = "BookingByRevenue not found",
                    IsSuccess = false
                };
            }
            return new APIResponseModel
            {
                Message = "BookingByRevenue found",
                IsSuccess = true,
                Data = bookingByRevenue,
            };
            // return _mapper.Map<BookingModel>(booking);
        }

        public async Task<APIResponseModel> CreateBookingByRevenueAsync(BookingByRevenueCreateModel createModel)
        {
            var bookingByRevenue = _mapper.Map<BookingByRevenue>(createModel);
            bookingByRevenue.BookingByRevenueId = Guid.NewGuid().ToString();
            bookingByRevenue.CreateDate = DateTime.Now;
            var result = await _unitOfWork.BookingByRevenueRepository.AddAsync(bookingByRevenue);
            _unitOfWork.Save();
            return new APIResponseModel
            {
                Message = " BookingByRevenue Created Successfully",
                IsSuccess = true,
                Data = bookingByRevenue,
            };
        }
        public async Task<APIResponseModel> UpdateBookingByRevenueAsync(BookingByRevenueUpdateModel updateModel)
        {
            var existingBookingByRevenue = await _unitOfWork.BookingByRevenueRepository.GetByIdGuidAsync(updateModel.BookingByRevenueId);

            if (existingBookingByRevenue == null)
            {
                return new APIResponseModel
                {
                    Message = "BookingByRevenue not found",
                    IsSuccess = false
                };
            }
            var createDate = existingBookingByRevenue.CreateDate;

            var bookingByRevenue = _mapper.Map(updateModel, existingBookingByRevenue);
            bookingByRevenue.CreateDate = createDate;
            bookingByRevenue.UpdateDate = DateTime.Now;

            var result = await _unitOfWork.BookingByRevenueRepository.UpdateAsync(bookingByRevenue);
            _unitOfWork.Save();

            return new APIResponseModel
            {
                Message = "BookingByRevenue Updated Successfully",
                IsSuccess = true,
                Data = bookingByRevenue,
            };
        }
        public async Task<APIResponseModel> DeleteBookingByRevenue(string bookingByRevenueId)
        {
            var bookingByRevenue = await _unitOfWork.BookingByRevenueRepository.GetByIdStringAsync(bookingByRevenueId);
            if (bookingByRevenue == null)
            {
                return new APIResponseModel
                {
                    Message = "BookingByRevenue not found",
                    IsSuccess = false
                };
            }
            if (bookingByRevenue.Status == -1)
            {
                return new APIResponseModel
                {
                    Message = "BookingByRevenue has been removed",
                    IsSuccess = false
                };
            }
            var createDate = bookingByRevenue.CreateDate;
            bookingByRevenue.Status = (int?)EStatus.IsDeleted;
            bookingByRevenue.CreateDate = createDate;
            bookingByRevenue.UpdateDate = DateTime.Now;
            var result = await _unitOfWork.BookingByRevenueRepository.UpdateAsync(bookingByRevenue);
            _unitOfWork.Save();
            return new APIResponseModel
            {
                Message = " BookingByRevenue Deleted Successfully",
                IsSuccess = true,
                Data = bookingByRevenue,
            };
        }
    }
}
