using AutoMapper;
using BusinessObjects.Enums;
using BusinessObjects.Models;
using BusinessObjects.ViewModels.PaymentMethod;
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
    public class PaymentMethodService : IPaymentMethodService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public PaymentMethodService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<APIResponseModel> CreatePaymentMethod(PaymentMethodCreateModel paymentMethodCreateModel)
        {
            var paymentMethod = _mapper.Map<PaymentMethod>(paymentMethodCreateModel);
            paymentMethod.PaymentMethodId = Guid.NewGuid().ToString();
            paymentMethod.CreateDate = DateTime.Now;
            await _unitOfWork.PaymentMethodRepository.AddAsync(paymentMethod);
            _unitOfWork.Save();
            return new APIResponseModel
            {
                Message = "Create PaymentMethod Successfully",
                IsSuccess = true,
                Data = paymentMethod,
            };
        }

        public async Task<APIResponseModel> DeletePaymentMethod(string id)
        {

            var paymentMethod = await _unitOfWork.PaymentMethodRepository.GetByIdStringAsync(id);

            if (paymentMethod == null)
            {
                return new APIResponseModel
                {
                    Message = "Payment method not found.",
                    IsSuccess = false,
                    Data = null
                };
            }

            if (paymentMethod.Status == (int?)EStatus.IsDeleted)
            {
                return new APIResponseModel
                {
                    Message = "Payment method has already been deleted.",
                    IsSuccess = false,
                    Data = null
                };
            }
            var createDate = paymentMethod.CreateDate;
            paymentMethod.CreateDate = createDate;
            paymentMethod.Status = (int?)EStatus.IsDeleted;
            paymentMethod.UpdateDate = DateTime.Now;

            var result = await _unitOfWork.PaymentMethodRepository.UpdateAsync(paymentMethod);
            _unitOfWork.Save();

            return new APIResponseModel
            {
                Message = "Delete PaymentMethod Successfully",
                IsSuccess = true,
                Data = result,
            };
        }
          

        public async Task<APIResponseModel> GetAllPaymentMethods()
        {
            var paymentMethodList = await _unitOfWork.PaymentMethodRepository.GetAllAsync();
            var count = paymentMethodList.Count();
            return new APIResponseModel
            {
                Message = $" Found {count} PaymentMethod ",
                IsSuccess = true,
                Data = paymentMethodList,
            };
        }

        public async Task<APIResponseModel> GetPaymentMethodById(string id)
        {
            var paymentMethod = await _unitOfWork.PaymentMethodRepository.GetByIdStringAsync(id);
            return new APIResponseModel
            {
                Message = "Get PaymentMethod Successfully",
                IsSuccess = true,
                Data = paymentMethod,
            };

        }

        public async Task<APIResponseModel> GetPaymentMethodsByStatus()
        {
            var paymentMethodList = await _unitOfWork.PaymentMethodRepository.GetByConditionAsync(s => s.Status != -1);
            var count = paymentMethodList.Count();
            return new APIResponseModel
            {
                Message = $" Found {count} PaymentMethod ",
                IsSuccess = true,
                Data = paymentMethodList,
            };
        }

        public async Task<APIResponseModel> UpdatePaymentMethod(PaymentMethodUpdateModel paymentMethodUpdateModel)
        {
            var existingPaymentMethod =  await _unitOfWork.PaymentMethodRepository.GetByIdGuidAsync(paymentMethodUpdateModel.PaymentMethodId);

            if (existingPaymentMethod == null)
            {
                return new APIResponseModel
                {
                    Message = "PaymentMethod not found",
                    IsSuccess = false
                };
            }
            var createDate = existingPaymentMethod.CreateDate;
            var paymentMethod = _mapper.Map(paymentMethodUpdateModel, existingPaymentMethod);
            paymentMethod.CreateDate = createDate;
            paymentMethod.UpdateDate = DateTime.Now;
            var result = await _unitOfWork.PaymentMethodRepository.UpdateAsync(paymentMethod);
            _unitOfWork.Save();
            return new APIResponseModel
            {
                Message = "Update PaymentMethod Successfully",
                IsSuccess = true,
                Data = result,
            };

        }

     
    }
}
