using AutoMapper;
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
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public PaymentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<APIResponseModel> GetAllPayments()
        {
            var paymentList = await _unitOfWork.PaymentRepository.GetAllAsync();
            var count = paymentList.Count();
            return new APIResponseModel
            {
                Message = $" Found {count} Payment ",
                IsSuccess = true,
                Data = paymentList,
            };
        }

        public async Task<APIResponseModel> GetPaymentById(string id)
        {
            var payment = await _unitOfWork.PaymentRepository.GetByIdStringAsync(id);
            return new APIResponseModel
            {
                Message = "Get Payment Successfully",
                IsSuccess = true,
                Data = payment,
            };

        }

        public async Task<APIResponseModel> GetPaymentsByStatus()
        {
            var payment = await _unitOfWork.PaymentRepository.GetByConditionAsync(s => s.Status != -1);
            var count = payment.Count();
            return new APIResponseModel
            {
                Message = $" Found {count} Payment ",
                IsSuccess = true,
                Data = payment,
            };
        }
    }
}
