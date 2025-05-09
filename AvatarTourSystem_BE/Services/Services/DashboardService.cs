﻿using AutoMapper;
using Azure;
using BusinessObjects.Enums;
using BusinessObjects.Models;
using BusinessObjects.ViewModels.Booking;
using BusinessObjects.ViewModels.Dashboard;
using Repositories.Interfaces;
using Services.Common;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public DashboardService(IUnitOfWork unitOfWork, IMapper mapper) 
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<APIGenericResponseModel<decimal>> GetMonthlyBookings(int month, int year)
        {
            var allBookings = await _unitOfWork.BookingRepository.GetAllAsync();
            var bookingFlowLists = _mapper.Map<List<BookingModel>>(allBookings);
            var monthlyBookings = bookingFlowLists.Where(b => b.BookingDate.Value.Month == month
                                            && b.BookingDate.Value.Year == year)
                                    .Sum(b => b.TotalPrice);
            if (monthlyBookings > 0)
            {
                return new APIGenericResponseModel<decimal>
                {
                    Message = "Found bookings!",
                    IsSuccess = true,
                    Data = (decimal)monthlyBookings
                };
            }
            else
            {
                return new APIGenericResponseModel<decimal>
                {
                    Message = "No booking revenue in chosen month!",
                    IsSuccess = false,
                };
            }
        }

        public async Task<APIGenericResponseModel<int>> GetActiveZaloUserCount()
        {
            var users = await _unitOfWork.AccountRepository.GetByConditionAsync(u => u.Status == 1 && !string.IsNullOrEmpty(u.ZaloUser));
            var activeZaloUsers = users.Count();
            if (activeZaloUsers > 0)
            {
                return new APIGenericResponseModel<int>
                {
                    Message = "Found active users!",
                    IsSuccess = true,
                    Data = activeZaloUsers
                };
            }
            else
            {
                return new APIGenericResponseModel<int>
                {
                    Message = "No active users!",
                    IsSuccess = false,
                };
            }
        }
        public async Task<APIResponseModel> GetActiveZaloUser()
        {
            var activeZaloUsers = await _unitOfWork.AccountRepository.GetByConditionAsync(u => u.Status == 1 && !string.IsNullOrEmpty(u.ZaloUser));
            if (activeZaloUsers != null)
            {
                return new APIResponseModel
                {
                    Message = "Found active users!",
                    IsSuccess = true,
                    Data = activeZaloUsers
                };
            }
            else
            {
                return new APIResponseModel
                {
                    Message = "No active users!",
                    IsSuccess = false,
                };
            }
        }

        //public async Task<APIGenericResponseModel<decimal>> GetMonthlyRevenue(int month, int year)
        //{
        //    var revenue = await _unitOfWork.RevenueRepository.GetByConditionAsync(s => s.Status != -1);
        //    var revenueLists = _mapper.Map<List<RevenueModel>>(revenue);
        //    var totalRevenue = revenueLists
        //        .Where(r => r.RevenueDate.Value.Month == month && r.RevenueDate.Value.Year == year)
        //        .Sum(b => b.TotalRevenue);
        //    if (totalRevenue > 0)
        //    {
        //        return new APIGenericResponseModel<decimal>
        //        {
        //            Message = "Found revenue!",
        //            IsSuccess = true,
        //            Data = (decimal)totalRevenue
        //        };
        //    }
        //    else
        //    {
        //        return new APIGenericResponseModel<decimal>
        //        {
        //            Message = "Invalid revenue data!",
        //            IsSuccess = false,
        //        };
        //    }
        //}

        public async Task<APIGenericResponseModel<int>> GetMonthlyTours(int month, int year)
        {
            var tours = await _unitOfWork.DailyTourRepository.GetByConditionAsync(s => s.Status != -1);
            var activeTours = tours.Where(t => t.StartDate.Value.Month == month && t.StartDate.Value.Year == year)
                .Count();
            if (activeTours > 0)
            {
                return new APIGenericResponseModel<int>
                {
                    Message = "Found active daily tours!",
                    IsSuccess = true,
                    Data = activeTours
                };
            }
            else
            {
                return new APIGenericResponseModel<int>
                {
                    Message = "No active daily tours in month!",
                    IsSuccess = false,
                };
            }
        }

        public async Task<APIGenericResponseModel<int>> GetMonthlyTicketsByType(string typeId, int month, int year)
        {
            var tickets = await _unitOfWork.TicketRepository.GetByConditionAsync(s => s.Status != -1);
            var monthlyTickets = tickets.Where(t => t.DailyTicketId == typeId).Sum(t => t.Quantity);
            if (monthlyTickets > 0 && typeId == "1")
            {
                return new APIGenericResponseModel<int>
                {
                    Message = "Found adults tickets!",
                    IsSuccess = true,
                    Data = (int)monthlyTickets
                };
            }
            else if (monthlyTickets > 0 && typeId == "2")
            {
                return new APIGenericResponseModel<int>
                {
                    Message = "Found child tickets!",
                    IsSuccess = true,
                    Data = (int)monthlyTickets
                };
            }
            else
            {
                return new APIGenericResponseModel<int>
                {
                    Message = "No active tickets in month!",
                    IsSuccess = false,
                };
            }
        }

        public async Task<APIResponseModel> CountAccountRole()
        {
            try
            {
                var rolesCount = new AccountCountViewModel
                {
                    SuperAdmin = await _unitOfWork.AccountRepository.CountAsync(a => a.Roles == (int)ERole.SuperAdmin),
                    Admin = await _unitOfWork.AccountRepository.CountAsync(a => a.Roles == (int)ERole.Admin),
                    Staff = await _unitOfWork.AccountRepository.CountAsync(a => a.Roles == (int)ERole.Staff),
                    Supplier = await _unitOfWork.AccountRepository.CountAsync(a => a.Roles == (int)ERole.Supplier),
                    Customer = await _unitOfWork.AccountRepository.CountAsync(a => a.Roles == (int)ERole.Customer)
                };
                rolesCount.Total = rolesCount.SuperAdmin + rolesCount.Admin + rolesCount.Staff + rolesCount.Supplier + rolesCount.Customer;

                return new APIResponseModel
                {
                    Message = "Counted accounts by role successfully.",
                    IsSuccess = true,
                    Data = rolesCount
                };
            }
            catch (Exception ex)
            {
                return new APIResponseModel
                {
                    Message = "Error" +ex,
                    IsSuccess = false,
                };
            }
        }
        public async Task<APIResponseModel> CountBooking()
        {
            try
            {
                var bookingsCount = new BookingCountViewModel
                {
                    BookingActive = await _unitOfWork.BookingRepository.CountAsync(a => a.Status == (int)EStatus.Active),
                    BookingOverdue = await _unitOfWork.BookingRepository.CountAsync(a => a.Status == (int)EStatus.Disabled),
                    BookingCancelled = await _unitOfWork.BookingRepository.CountAsync(a => a.Status == (int)EStatus.IsCancelled),
                    BookingUsed = await _unitOfWork.BookingRepository.CountAsync(a => a.Status == (int)EStatus.IsCompleted),
                    BookingRefund = await _unitOfWork.BookingRepository.CountAsync(a => a.Status == (int)EStatus.IsRefund),
                    BookingInProgress = await _unitOfWork.BookingRepository.CountAsync(a => a.Status == (int)EStatus.InProgress)
                };
                bookingsCount.Total = bookingsCount.BookingActive + bookingsCount.BookingOverdue + bookingsCount.BookingCancelled + bookingsCount.BookingUsed + bookingsCount.BookingRefund + bookingsCount.BookingInProgress;

                return new APIResponseModel
                {
                    Message = "Counted booking successfully.",
                    IsSuccess = true,
                    Data = bookingsCount
                };
            }
            catch (Exception ex)
            {
                return new APIResponseModel
                {
                    Message = "Error" + ex,
                    IsSuccess = false,
                };
            }
        }
        public async Task<APIResponseModel> CountRequest()
        {
            try
            {
                var requestsCount = new RequestCountViewModel
                {
                    RequestInProgress = await _unitOfWork.CustomerSupportRepository.CountAsync(a => a.Status == (int)EStatus.InProgress),
                    RequestCompleted = await _unitOfWork.CustomerSupportRepository.CountAsync(a => a.Status == (int)EStatus.IsCompleted),
                    RequestDeleted = await _unitOfWork.CustomerSupportRepository.CountAsync(a => a.Status == (int)EStatus.IsDeleted)
                };
                requestsCount.Total = requestsCount.RequestInProgress + requestsCount.RequestCompleted + requestsCount.RequestDeleted;

                return new APIResponseModel
                {
                    Message = "Counted request successfully.",
                    IsSuccess = true,
                    Data = requestsCount
                };
            }
            catch (Exception ex)
            {
                return new APIResponseModel
                {
                    Message = "Error" + ex,
                    IsSuccess = false,
                };
            }
        }


        public async Task<APIResponseModel> CountPackageInday()
        {
            try
            {
                var packageIndaysCount = new PackageIndayCountViewModel
                {
                    PackageIndayActive = await _unitOfWork.PackageTourRepository.CountAsync(a => a.Status == (int)EStatus.Active),
                    PackageIndayDelete = await _unitOfWork.PackageTourRepository.CountAsync(a => a.Status == (int)EStatus.IsDeleted),
                };
                packageIndaysCount.Total = packageIndaysCount.PackageIndayActive + packageIndaysCount.PackageIndayDelete ;

                return new APIResponseModel
                {
                    Message = "Counted PackageInday successfully.",
                    IsSuccess = true,
                    Data = packageIndaysCount
                };
            }
            catch (Exception ex)
            {
                return new APIResponseModel
                {
                    Message = "Error" + ex,
                    IsSuccess = false,
                };
            }
        }
    }
}
