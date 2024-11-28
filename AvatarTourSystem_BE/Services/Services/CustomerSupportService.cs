using AutoMapper;
using BusinessObjects.Enums;
using BusinessObjects.Models;
using BusinessObjects.ViewModels.Account;
using BusinessObjects.ViewModels.CustomerSupport;
using Microsoft.AspNetCore.Mvc;
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
    public class CustomerSupportService : ICustomerSupportService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CustomerSupportService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<APIResponseModel> CreateCustomerSupport(CustomerSupportCreateModel createModel)
        {

            var customerSupport = _mapper.Map<CustomerSupport>(createModel);
            customerSupport.CusSupportId = Guid.NewGuid().ToString();
            customerSupport.CreateDate = DateTime.Now;
            var result = await _unitOfWork.CustomerSupportRepository.AddAsync(customerSupport);
            _unitOfWork.Save();
            var cusModel = _mapper.Map<CustomerSupportModel>(result);
            return new APIResponseModel
            {
                Message = "Create Customer Support Successfully",
                IsSuccess = true,
                Data = cusModel,
            };
        }

        public async Task<APIResponseModel> DeleteCustomerSupport(string cusId)
        {
            var customerSupport = await _unitOfWork.CustomerSupportRepository.GetByIdStringAsync(cusId);
            if (customerSupport == null)
            {
                return new APIResponseModel
                {
                    Message = "Customer Support not found.",
                    IsSuccess = false,
                    Data = null
                };
            }
            if (customerSupport.Status == (int?)EStatus.IsDeleted)
            {
                return new APIResponseModel
                {
                    Message = "Customer Support has already been deleted.",
                    IsSuccess = false,
                    Data = null
                };
            }
            var createDate = customerSupport.CreateDate;
            customerSupport.Status = (int?)EStatus.IsDeleted;
            customerSupport.UpdateDate = DateTime.Now;
            customerSupport.CreateDate = createDate;
            var result = await _unitOfWork.CustomerSupportRepository.UpdateAsync(customerSupport);
            _unitOfWork.Save();
            return new APIResponseModel
            {
                Message = "Delete Customer Support Successfully",
                IsSuccess = true,
                Data = result,
            };
        }

        public async Task<APIResponseModel> GetAllCustomerSupport()
        {
            var customerSupport = await _unitOfWork.CustomerSupportRepository.GetAllAsync();
            return new APIResponseModel
            {
                Message = "Get all customer support successfully",
                IsSuccess = true,
                Data = customerSupport,
            };
        }

        public async Task<APIResponseModel> GetCustomerSupportById(string cusId)
        {
            var customerSupport = await _unitOfWork.CustomerSupportRepository.GetByConditionAsync(x => x.CusSupportId == cusId);
            if (customerSupport == null)
            {
                return new APIResponseModel
                {
                    Message = "Customer Support not found.",
                    IsSuccess = false,
                    Data = null
                };
            }
            else
            {
                var cusModel = _mapper.Map<List<CustomerSupportModel>>(customerSupport);

                return new APIResponseModel
                {
                    Message = "Customer Support found",
                    IsSuccess = true,
                    Data = cusModel
                };

            }
        }

        public async Task<APIResponseModel> GetCustomerSupportByStatus()
        {
            var customerSupport = await _unitOfWork.CustomerSupportRepository.GetByConditionAsync(s => s.Status != -1);
            if (customerSupport == null || !customerSupport.Any())
            {
                return new APIResponseModel
                {
                    Message = "Customer Support not found.",
                    IsSuccess = false,
                    Data = null
                };
            }
            var cusModel = _mapper.Map<List<CustomerSupportModel>>(customerSupport);
            return new APIResponseModel
            {
                Message = "Get Customer Support by Status Successfully",
                IsSuccess = true,
                Data = cusModel
            };
        }

        public async Task<APIResponseModel> GetCustomerSupportByUserId(string userId)
        {
            var customerSupport = await _unitOfWork.CustomerSupportRepository.GetByConditionAsync(x => x.UserId == userId);
            if (customerSupport == null)
            {
                return new APIResponseModel
                {
                    Message = "Customer Support not found.",
                    IsSuccess = false,
                    Data = null
                };
            }
            var cusModel = _mapper.Map<List<CustomerSupportModel>>(customerSupport);
            return new APIResponseModel
            {
                Message = "Found Customer Support successfully.",
                IsSuccess = true,
                Data = cusModel,
            };
        }

        public async Task<APIResponseModel> UpdateCustomerSupport(CustomerSupportUpdateModel updateModel)
        {
            var existingCustomerSupport = await _unitOfWork.CustomerSupportRepository.GetByIdGuidAsync(updateModel.CusSupportId);
            if (existingCustomerSupport == null)
            {
                return new APIResponseModel
                {
                    Message = "Customer Support not found",
                    IsSuccess = false
                };
            }
            var createDate = existingCustomerSupport.CreateDate;
            var customerSupport = _mapper.Map(updateModel, existingCustomerSupport);
            customerSupport.CreateDate = createDate;
            customerSupport.UpdateDate = DateTime.Now;
            var result = await _unitOfWork.CustomerSupportRepository.UpdateAsync(customerSupport);
            _unitOfWork.Save();
            return new APIResponseModel
            {
                Message = "Customer Support Updated Successfully",
                IsSuccess = true,
                Data = result,
            };

        }



        public async Task<APIResponseModel> GetAllRequest()
        {
            try
            {
               var requests = await _unitOfWork.CustomerSupportRepository.GetAllAsyncs(query => query.Include(dt=>dt.RequestTypes).Include(ac=>ac.Accounts));
               var result = requests.Select(request => new
                {
                    CusSupportId = request.CusSupportId,
                    UserId = request.UserId,
                    FullName = request.Accounts.FullName,
                    RequestId = request.RequestTypeId,
                    Type = request.RequestTypes.Type,
                    Priority = request.RequestTypes.Priority,
                    Description = request.Description,
                    CreateDate = request.CreateDate,
                    DateResolved = request.DateResolved,
                    Status = request.Status
                });

                return new APIResponseModel
                {
                    Message = "Successfully retrieved requests",
                    IsSuccess = true,
                    Data = result
                };
            }
            catch (Exception ex)
            {
                return new APIResponseModel
                {
                    Message = "Error get request",
                    IsSuccess = false
                };
            }
        }

        public async Task<APIResponseModel> UpdateStatusCustomerSupport(CustomerSupportStatusViewModel customerSupportStatusViewModel)
        {
            try
            {
                var updateRequests = await _unitOfWork.CustomerSupportRepository.GetByIdStringAsync(customerSupportStatusViewModel.CusSupportId);

                var createDate = updateRequests.CreateDate;
                updateRequests.Status= customerSupportStatusViewModel.Status;
                updateRequests.DateResolved = DateTime.Now;
                updateRequests.CreateDate = createDate;
                updateRequests.UpdateDate = DateTime.Now;

                _unitOfWork.CustomerSupportRepository.UpdateAsync(updateRequests);
                _unitOfWork.Save();

                return new APIResponseModel
                {
                    Message = "Successfully retrieved requests",
                    IsSuccess = true,
                };
            }
            catch (Exception ex)
            {
                return new APIResponseModel
                {
                    Message = "Error updated request",
                    IsSuccess = false
                };
            }
        }
    }
}
