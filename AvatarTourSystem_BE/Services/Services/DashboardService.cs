using AutoMapper;
using Azure;
using BusinessObjects.Models;
using BusinessObjects.ViewModels.Booking;
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
            var bookingFlowLists = _mapper.Map<List<BookingFlowModel>>(allBookings);
            var monthlyBookings = bookingFlowLists.Where(b => b.BookingDate.Month == month
                                            && b.BookingDate.Year == year)
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
        public async Task<decimal> GetMonthlytAdultTickets(int month, int year)
        {
            throw new NotImplementedException();
        }

        public async Task<decimal> GetMonthlytChildTickets(int month, int year)
        {
            throw new NotImplementedException();
        }

        public async Task<APIGenericResponseModel<int>> GetCountActiveZaloUser()
        {
            var users = await _unitOfWork.AccountRepository.GetAllAsync();
            var activeZaloUsers = users.Where(u => u.Status == 1 && !string.IsNullOrEmpty(u.ZaloUser)).Count();
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
            var users = await _unitOfWork.AccountRepository.GetAllAsync();
            var activeZaloUsers = users.Where(u => (u.Status == 1));
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

        public async Task<APIGenericResponseModel<decimal>> GetTotalRevenue()
        {
            var revenue = await _unitOfWork.RevenueRepository.GetAllAsync();
            var totalRevenue = revenue.Sum(b => b.TotalRevenue);
            if (totalRevenue > 0)
            {
                return new APIGenericResponseModel<decimal>
                {
                    Message = "Found revenue!",
                    IsSuccess = true,
                    Data = (decimal)totalRevenue
                };
            }
            else
            {
                return new APIGenericResponseModel<decimal>
                {
                    Message = "Invalid revenue data!",
                    IsSuccess = false,
                };
            }
        }

        public async Task<APIGenericResponseModel<int>> GetMonthlyTour(int month, int year)
        {
            throw new NotImplementedException();
        }

        
    }
}
