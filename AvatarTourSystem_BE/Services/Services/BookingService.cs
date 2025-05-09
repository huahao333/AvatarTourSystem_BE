﻿using AutoMapper;
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
                var bookingInfor = await _unitOfWork.BookingRepository.GetAllAsyncs(query =>
            query.Include(b => b.Tickets)
                 .Include(a => a.Accounts)
                 .ThenInclude(cs => cs.CustomerSupports)
                 .Include(p => p.Payments)
                 .Include(d=>d.DailyTours));


                var utcNowDate = DateTime.Now.Date;
                foreach (var booking in bookingInfor)
                {
                    if (booking.ExpirationDate.HasValue && booking.ExpirationDate.Value.Date < utcNowDate)
                    {
                        var ticketsToUpdate = await _unitOfWork.TicketRepository.GetAllAsyncs(query => query
                            .Where(t => t.BookingId == booking.BookingId && t.Status == 1));
                        foreach (var ticket in ticketsToUpdate)
                        {
                            ticket.Status = 0;
                            _unitOfWork.TicketRepository.UpdateAsync(ticket);
                        }

                        var servicesToUpdate = await _unitOfWork.ServiceUsedByTicketRepository.GetAllAsyncs(query => query
                            .Where(s => ticketsToUpdate.Select(t => t.TicketId).Contains(s.TicketId) && s.Status == 1));
                        foreach (var service in servicesToUpdate)
                        {
                            service.Status = 0;
                            _unitOfWork.ServiceUsedByTicketRepository.UpdateAsync(service);
                        }

                        if (booking.Status == 1)
                        {
                            booking.Status = 0;
                            _unitOfWork.BookingRepository.UpdateAsync(booking);
                        }
                    }
                }

                _unitOfWork.Save();


                var result = bookingInfor.Select(booking =>
                {
                    // Lấy DateCreateRequest
                    var dateCreateRequest = booking.Accounts.CustomerSupports
                        .OrderByDescending(t => t.CreateDate)
                        .FirstOrDefault(c =>
                        {
                            var match = System.Text.RegularExpressions.Regex.Match(c.Description, @"ᡣ(.*?)୨ৎ");
                            if (match.Success)
                            {
                                var extractedBookingId = match.Groups[1].Value;
                                return extractedBookingId == booking.BookingId ;
                            }
                            return false;
                        })?.CreateDate;

                    var compareDate = dateCreateRequest.HasValue ? dateCreateRequest.Value : DateTime.Now.Date;

                    var isRefundTerms = false;

                    if (booking.Status is not (9 or -1 or 4 or 5 or 0 or 3))
                    {
                        if ( booking.ExpirationDate.HasValue)
                        {
                            var daysDifference = (booking.ExpirationDate.Value.Date - compareDate.Date).TotalDays;
                            if (daysDifference >= 2 && compareDate.Date < booking.ExpirationDate.Value.Date)
                            {
                                isRefundTerms = true;
                            }
                        }
                    }


                    return new
                    {
                        bookingId = booking.BookingId,
                        UserId = booking.UserId,
                        FullName = booking.Accounts.FullName,
                        DailyTourId = booking.DailyTourId,
                        DaiLyTourName = booking.DailyTours?.DailyTourName,
                        BookingData = booking.BookingDate,
                        ExpirationDate = booking.ExpirationDate,
                        IsRefundTerms = isRefundTerms,
                        TotalPrice = booking.TotalPrice,
                        MerchantId = booking.Payments.FirstOrDefault()?.MerchantTransId,
                        DateCreateRequest = dateCreateRequest,
                        Status = booking.Status,
                        CreateDateOfBoooking = booking.CreateDate,
                        Tickets = booking.Tickets.Where(c => c.BookingId == booking.BookingId).Select(t => new
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
                    };
                }).OrderByDescending(c=>c.CreateDateOfBoooking).ThenByDescending(i=>i.IsRefundTerms);

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

        public async Task<APIResponseModel> GetAllBookingsByDailyTourIdAsync(BookingByDailyTourIdViewModel bookingByDailyTourIdViewModel)
        {
            try
            {
                if (string.IsNullOrEmpty(bookingByDailyTourIdViewModel.DailyTourId))
                {
                    return new APIResponseModel
                    {
                        Message = "DailyTourId cannot be empty.",
                        IsSuccess = false
                    };
                }
                var bookingInfor = await _unitOfWork.BookingRepository.GetAllAsyncs(query =>
            query.Where(b=>b.DailyTourId == bookingByDailyTourIdViewModel.DailyTourId).Include(b => b.Tickets)
                 .Include(a => a.Accounts)
                 .ThenInclude(cs => cs.CustomerSupports)
                 .Include(p => p.Payments)
                 .Include(d => d.DailyTours));
                if (bookingInfor == null)
                {
                    return new APIResponseModel
                    {
                        Message = "This DailyTour has no bookings.",
                        IsSuccess = false,
                    };
                }

                var utcNowDate = DateTime.Now.Date;
                foreach (var booking in bookingInfor)
                {
                    if (booking.ExpirationDate.HasValue && booking.ExpirationDate.Value.Date < utcNowDate)
                    {
                        var ticketsToUpdate = await _unitOfWork.TicketRepository.GetAllAsyncs(query => query
                            .Where(t => t.BookingId == booking.BookingId && t.Status == 1));
                        foreach (var ticket in ticketsToUpdate)
                        {
                            ticket.Status = 0;
                            _unitOfWork.TicketRepository.UpdateAsync(ticket);
                        }

                        var servicesToUpdate = await _unitOfWork.ServiceUsedByTicketRepository.GetAllAsyncs(query => query
                            .Where(s => ticketsToUpdate.Select(t => t.TicketId).Contains(s.TicketId) && s.Status == 1));
                        foreach (var service in servicesToUpdate)
                        {
                            service.Status = 0;
                            _unitOfWork.ServiceUsedByTicketRepository.UpdateAsync(service);
                        }

                        if (booking.Status == 1)
                        {
                            booking.Status = 0;
                            _unitOfWork.BookingRepository.UpdateAsync(booking);
                        }
                    }
                }

                _unitOfWork.Save();


                var result = bookingInfor.Select(booking =>
                {
                    // Lấy DateCreateRequest
                    var dateCreateRequest = booking.Accounts.CustomerSupports
                        .OrderByDescending(t => t.CreateDate)
                        .FirstOrDefault(c =>
                        {
                            var match = System.Text.RegularExpressions.Regex.Match(c.Description, @"ᡣ(.*?)୨ৎ");
                            if (match.Success)
                            {
                                var extractedBookingId = match.Groups[1].Value;
                                return extractedBookingId == booking.BookingId;
                            }
                            return false;
                        })?.CreateDate;

                    var compareDate = dateCreateRequest.HasValue ? dateCreateRequest.Value : DateTime.Now.Date;

                    var isRefundTerms = false;

                    if (booking.Status is not (9 or -1 or 4 or 5 or 0 or 3))
                    {
                        if (booking.ExpirationDate.HasValue)
                        {
                            var daysDifference = (booking.ExpirationDate.Value.Date - compareDate.Date).TotalDays;
                            if (daysDifference >= 2 && compareDate.Date < booking.ExpirationDate.Value.Date)
                            {
                                isRefundTerms = true;
                            }
                        }
                    }


                    return new
                    {
                        bookingId = booking.BookingId,
                        UserId = booking.UserId,
                        FullName = booking.Accounts.FullName,
                        DailyTourId = booking.DailyTourId,
                        DaiLyTourName = booking.DailyTours?.DailyTourName,
                        BookingData = booking.BookingDate,
                        ExpirationDate = booking.ExpirationDate,
                        IsRefundTerms = isRefundTerms,
                        TotalPrice = booking.TotalPrice,
                        MerchantId = booking.Payments.FirstOrDefault()?.MerchantTransId,
                        DateCreateRequest = dateCreateRequest,
                        Status = booking.Status,
                        CreateDateOfBoooking = booking.CreateDate,
                        Tickets = booking.Tickets.Where(c => c.BookingId == booking.BookingId).Select(t => new
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
                    };
                }).OrderByDescending(c => c.CreateDateOfBoooking).ThenByDescending(i => i.IsRefundTerms);

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
