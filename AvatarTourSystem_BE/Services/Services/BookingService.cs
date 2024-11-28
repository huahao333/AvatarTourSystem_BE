using AutoMapper;
using BusinessObjects.Enums;
using BusinessObjects.Models;
using BusinessObjects.ViewModels.Booking;
using BusinessObjects.ViewModels.ServiceUsedByTicket;
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

        public async Task<APIResponseModel> GetAllBookingsAsync()
        {
            try
            {
                var bookingInfor = await _unitOfWork.BookingRepository.GetAllAsyncs(query=>
                                                     query.Include(b=>b.Tickets).Include(a=>a.Accounts).Include(p=>p.Payments));
                var result = bookingInfor.Select(booking => new
                {
                    bookingId = booking.BookingId,
                    UserId = booking.UserId,
                    FullName = booking.Accounts.FullName,
                    DailyTourId = booking.DailyTourId,
                    BookingData = booking.BookingDate,
                    ExpirationDate = booking.ExpirationDate,
                    TotalPrice = booking.TotalPrice,
                    MerchantId = booking.Payments.FirstOrDefault()?.MerchantTransId,
                    Status = booking.Status,
                    Tickets = booking.Tickets.Where(c=>c.BookingId == booking.BookingId).Select(t => new
                    {
                        TicketId = t.TicketId,
                        DailyTicketId = t.DailyTicketId,
                        TicketName = t.TicketName,
                        Quantity = t.Quantity,
                        Qr = t.QRImgUrl,
                        Phone = t.PhoneNumberReference,
                        Price = t.Price,
                        CreateDate = t.CreateDate,
                    }).ToList(),
                });


                return new APIResponseModel
                {
                    Message = "Successfully retrieved booking",
                    IsSuccess = true,
                    Data = result
                };
            }
            catch (Exception ex)
            {
                return new APIResponseModel
                {
                    Message = " Error get booking",
                    IsSuccess = false,
                };
            }
        }
    }
}
