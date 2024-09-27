using AutoMapper;
using BusinessObjects.Enums;
using BusinessObjects.Models;
using BusinessObjects.ViewModels.Booking;
using BusinessObjects.ViewModels.Revenue;
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
    public class RevenueService : IRevenueService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public RevenueService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<APIResponseModel> GetRevenuesAsync()
        {
            var list = await _unitOfWork.RevenueRepository.GetAllAsync();
            var count = list.Count();
            return new APIResponseModel
            {
                Message = $" Found {count} Revenue ",
                IsSuccess = true,
                Data = list,
            };
        }
        public async Task<APIResponseModel> GetActiveRevenuesAsync()
        {
            var list = await _unitOfWork.RevenueRepository.GetByConditionAsync(s => s.Status != -1);
            var count = list.Count();
            return new APIResponseModel
            {
                Message = $" Found {count} Revenue ",
                IsSuccess = true,
                Data = list,
            };
        }
        public async Task<APIResponseModel> GetRevenueByIdAsync(string revenueId)
        {
            var revenue = await _unitOfWork.RevenueRepository.GetByIdStringAsync(revenueId);
            if (revenue == null)
            {
                return new APIResponseModel
                {
                    Message = "Revenue not found",
                    IsSuccess = false
                };
            }
            //  return _mapper.Map<RevenueModel>(revenue);
            return new APIResponseModel
            {
                Message = "Revenue found",
                IsSuccess = true,
                Data = revenue,
            };
        }

        public async Task<APIResponseModel> CreateRevenueAsync(RevenueCreateModel createModel)
        {
            var revenue = _mapper.Map<Revenue>(createModel);
            revenue.RevenueId = Guid.NewGuid().ToString();
            revenue.CreateDate = DateTime.Now;
            revenue.RevenueDate = DateTime.Now;
            var result = await _unitOfWork.RevenueRepository.AddAsync(revenue);
            _unitOfWork.Save();
            return new APIResponseModel
            {
                Message = " Revenue Created Successfully",
                IsSuccess = true,
                Data = revenue,
            };
        }
        public async Task<APIResponseModel> UpdateRevenueAsync(RevenueUpdateModel updateModel)
        {
            var existingRevenue = await _unitOfWork.RevenueRepository.GetByIdGuidAsync(updateModel.RevenueId);

            if (existingRevenue == null)
            {
                return new APIResponseModel
                {
                    Message = "Revenue not found",
                    IsSuccess = false
                };
            }
            var createDate = existingRevenue.CreateDate;
            var revenueDate = existingRevenue.RevenueDate;

            var revenue = _mapper.Map(updateModel, existingRevenue);
            revenue.CreateDate = createDate;
            revenue.UpdateDate = DateTime.Now;
            revenue.RevenueDate = revenueDate;

            var result = await _unitOfWork.RevenueRepository.UpdateAsync(revenue);
            _unitOfWork.Save();

            return new APIResponseModel
            {
                Message = "Revenue Updated Successfully",
                IsSuccess = true,
                Data = revenue,
            };
        }
        public async Task<APIResponseModel> DeleteRevenue(string revenueId)
        {
            var revenue = await _unitOfWork.RevenueRepository.GetByIdStringAsync(revenueId);
            if (revenue == null)
            {
                return new APIResponseModel
                {
                    Message = "Revenue not found",
                    IsSuccess = false
                };
            }
            if (revenue.Status == -1)
            {
                return new APIResponseModel
                {
                    Message = "Revenue has been removed",
                    IsSuccess = false
                };
            }
            var createDate = revenue.CreateDate;
            var revenueDate = revenue.RevenueDate;
            revenue.Status = (int?)EStatus.IsDeleted;
            revenue.CreateDate = createDate;
            revenue.RevenueDate = revenueDate;
            revenue.UpdateDate = DateTime.Now;
            var result = await _unitOfWork.RevenueRepository.UpdateAsync(revenue);
            _unitOfWork.Save();
            return new APIResponseModel
            {
                Message = " Revenue Deleted Successfully",
                IsSuccess = true,
                Data = revenue,
            };
        }
    }
}
