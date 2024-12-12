using AutoMapper;
using BusinessObjects.Data;
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
                var requests = await _unitOfWork.CustomerSupportRepository.GetAllAsyncs(query =>
                    query.Include(dt => dt.RequestTypes).Include(ac => ac.Accounts));

                var result = requests.Select(request =>
                {
                    var match = System.Text.RegularExpressions.Regex.Match(request.Description, @"ᡣ(.*?)୨ৎ");
                    var bookingId = match.Success ? match.Groups[1].Value : null;
                    var sanitizedDescription = System.Text.RegularExpressions.Regex.Replace(request.Description, @"ᡣ(.*?)୨ৎ", string.Empty).Trim(); 
                    return new
                    {
                        CusSupportId = request.CusSupportId,
                        UserId = request.UserId,
                        FullName = request.Accounts?.FullName,
                        RequestId = request.RequestTypeId,
                        Type = request.RequestTypes?.Type,
                        Priority = request.RequestTypes?.Priority,
                        Description = sanitizedDescription,
                        Booking = bookingId, 
                        CreateDate = request.CreateDate,
                        DateResolved = request.DateResolved,
                        Status = request.Status
                    };
                }).OrderByDescending(r=>r.CreateDate);

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
                if (string.IsNullOrEmpty(customerSupportStatusViewModel.NotificationDescription))
                {
                    return new APIResponseModel
                    {
                        Message = "UserId not found.",
                        IsSuccess = false
                    };
                }
                var userId = await _unitOfWork.AccountRepository.GetByIdStringAsync(customerSupportStatusViewModel.UserId);
                if (string.IsNullOrEmpty(customerSupportStatusViewModel.UserId) || userId ==null)
                {
                    return new APIResponseModel
                    {
                        Message = "UserId not found.",
                        IsSuccess = false
                    };
                }
                string notificationType = "";
                string notificationTitle = "";
                var requestType = await _unitOfWork.RequestTypeRepository.GetByIdStringAsync(updateRequests.RequestTypeId);
                if (customerSupportStatusViewModel.Status == 4)
                {
                    if(requestType.Type == "Yêu cầu hoàn tiền")
                    {
                        notificationType = "Success";
                        notificationTitle = "Hoàn tiền thành công";
                    }
                    else
                    {
                        notificationType = "Success";
                        notificationTitle = "Xử lý yêu cầu thành công";
                    }
                }
                else if (customerSupportStatusViewModel.Status == -1)
                {
                    if (requestType.Type == "Yêu cầu hoàn tiền")
                    {
                        notificationType = "Default";
                        notificationTitle = "Hoàn tiền thất bại";
                    }
                    else
                    {
                        notificationType = "Default";
                        notificationTitle = "Xử lý yêu cầu thất bại";
                    }
                }

                var newNotification = new Notification
                {
                    NotifyId = Guid.NewGuid().ToString(),
                    UserId = customerSupportStatusViewModel.UserId,
                    SendDate = DateTime.Now,
                    Message= customerSupportStatusViewModel.NotificationDescription,
                    Type = notificationType,
                    Title = notificationTitle,
                    Status = 1,
                    CreateDate = DateTime.Now
                };
                await _unitOfWork.CustomerSupportRepository.UpdateAsync(updateRequests);
                await _unitOfWork.NotificationRepository.AddAsync(newNotification);
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

        public async Task<APIResponseModel> CreateRequestCustomerSupportForRefund(CustomerSupportRequestCreateModel customerSupportRequestCreate)
        {
            try
            {
                var zaloUser = await _unitOfWork.AccountRepository.GetFirstOrDefaultAsync(query => query.Where(c=>c.ZaloUser== customerSupportRequestCreate.ZaloUser));
                if (string.IsNullOrEmpty(customerSupportRequestCreate.ZaloUser) || zaloUser == null)
                {
                    return new APIResponseModel
                    {
                        Message = "UserId not found.",
                        IsSuccess = false
                    };
                }
                if (string.IsNullOrEmpty(customerSupportRequestCreate.Description))
                {
                    return new APIResponseModel
                    {
                        Message = "Description are required.",
                        IsSuccess = false
                    };
                }
                string type = "Yêu cầu hoàn tiền";
                var requestTypeId = await _unitOfWork.RequestTypeRepository.GetFirstOrDefaultAsync(query => query.Where(c=>c.Type== type.ToString()));
                if (requestTypeId==null)
                {
                    return new APIResponseModel
                    {
                        Message = "RequestId not found.",
                        IsSuccess = false
                    };
                }
                var updateBooking = await UpdateStatusBookingAsync(customerSupportRequestCreate.BookingId, 2);
                var descriptionWithBooking = customerSupportRequestCreate.BookingId != null
                    ? $"{customerSupportRequestCreate.Description} ᡣ{customerSupportRequestCreate.BookingId}୨ৎ"
                    : customerSupportRequestCreate.Description;

                var newRequest = new CustomerSupport
                {
                    CusSupportId= Guid.NewGuid().ToString(),
                    UserId = zaloUser.Id,
                    RequestTypeId = requestTypeId.RequestTypeId,
                    Description = descriptionWithBooking,
                    Status = 9, 
                    CreateDate = DateTime.Now 
                };

                await _unitOfWork.CustomerSupportRepository.AddAsync(newRequest);

                var notification = new BusinessObjects.Models.Notification
                {
                    NotifyId = Guid.NewGuid().ToString(),
                    UserId = zaloUser.Id,
                    SendDate = DateTime.Now,
                    Message = $"Bạn đã gửi yêu cầu hoàn tiền thành công chúng tôi sẽ cố gắng phản hồi cho quý khách sớm nhất",
                    Title = "Gửi yêu cầu thành công",
                    Type = "Success",
                    Status = 1,
                    CreateDate = DateTime.Now,
                };
                await _unitOfWork.NotificationRepository.AddAsync(notification);
                _unitOfWork.Save();

                return new APIResponseModel
                {
                    Message = "Successfully created request.",
                    IsSuccess = true,
                };
            }
            catch (Exception ex)
            {
                return new APIResponseModel
                {
                    Message = "Error create request",
                    IsSuccess = false
                };
            }
        }

        public async Task<APIResponseModel> UpdateStatusBookingAsync(string bookingId, int status)
        {
            try
            {
                var booking = await _unitOfWork.BookingRepository.GetFirstsOrDefaultAsync(b => b.BookingId == bookingId);
                if (booking == null)
                {
                    return new APIResponseModel
                    {
                        Message = "Booking not found.",
                        IsSuccess = false,
                    };
                }

                booking.Status = status;
                booking.UpdateDate = DateTime.Now;
                await _unitOfWork.BookingRepository.UpdateAsync(booking);


                var tickets = await _unitOfWork.TicketRepository.GetAllAsyncs(query => query
                                                            .Where(t => t.BookingId == bookingId));
                foreach (var ticket in tickets)
                {
                    ticket.Status = status;
                    ticket.UpdateDate = DateTime.Now;
                    await _unitOfWork.TicketRepository.UpdateAsync(ticket);

                    var dailyTicketType = await _unitOfWork.DailyTicketRepository.GetAllAsyncs(query => query.Where(d => d.DailyTicketId == ticket.DailyTicketId));
                    var dailyTicketTypeCapa = dailyTicketType.FirstOrDefault();
                    if (dailyTicketTypeCapa != null)
                    {
                        dailyTicketTypeCapa.Capacity += ticket.Quantity;
                        dailyTicketTypeCapa.UpdateDate = DateTime.Now;
                        await _unitOfWork.DailyTicketRepository.UpdateAsync(dailyTicketTypeCapa);
                    }
                }

                foreach (var ticket in tickets)
                {
                    var servicesUsedByTicket = await _unitOfWork.ServiceUsedByTicketRepository.GetAllAsyncs(query => query
                                                                                             .Where(s => s.TicketId == ticket.TicketId));
                    foreach (var serviceUsed in servicesUsedByTicket)
                    {
                        serviceUsed.Status = status;
                        serviceUsed.UpdateDate = DateTime.Now;
                        await _unitOfWork.ServiceUsedByTicketRepository.UpdateAsync(serviceUsed);
                    }
                }

                _unitOfWork.Save();

                return new APIResponseModel
                {
                    Message = "Refund successfully and Status updated successfully for booking and related records.",
                    IsSuccess = true,
                };
            }
            catch (Exception ex)
            {
                return new APIResponseModel
                {
                    Message = ex.Message,
                    IsSuccess = false,
                };
            }
        }

        public async Task<APIResponseModel> CreateRequestCustomerSupportByZaloId(SupportRequestByZaloIdViewModel supportRequestByZaloIdViewModel)
        {
            try
            {
                var zaloUser = await _unitOfWork.AccountRepository.GetFirstOrDefaultAsync(query => query.Where(c => c.ZaloUser == supportRequestByZaloIdViewModel.ZaloUser));
                if ( zaloUser == null)
                {
                    return new APIResponseModel
                    {
                        Message = "UserId not found.",
                        IsSuccess = false
                    };
                }

                var newRequest = new CustomerSupport
                {
                    CusSupportId = Guid.NewGuid().ToString(),
                    UserId = zaloUser.Id,
                    RequestTypeId = supportRequestByZaloIdViewModel.RequestTypeId,
                    Description = supportRequestByZaloIdViewModel.Description,
                    Status = 9,
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now
                };

                await _unitOfWork.CustomerSupportRepository.AddAsync(newRequest);

                var notification = new BusinessObjects.Models.Notification
                {
                    NotifyId = Guid.NewGuid().ToString(),
                    UserId = zaloUser.Id,
                    SendDate = DateTime.Now,
                    Message = $"Bạn đã gửi yêu cầu thành công chúng tôi sẽ cố gắng phản hồi cho quý khách sớm nhất",
                    Title = "Gửi yêu cầu thành công",
                    Type = "Success",
                    Status = 1,
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                };
                await _unitOfWork.NotificationRepository.AddAsync(notification);
                _unitOfWork.Save();


                return new APIResponseModel
                {
                    Message = "Create successfully",
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                return new APIResponseModel
                {
                    Message = ex.Message,
                    IsSuccess = false,
                };
            }
        }
        public async Task<APIResponseModel> GetListRequestByZaloId(GetListRequestViewModel getListRequestViewModel)
        {
            try
            {
                var zaloUser = await _unitOfWork.AccountRepository.GetFirstOrDefaultAsync(query => query.Where(c => c.ZaloUser == getListRequestViewModel.ZaloUser));
                if (zaloUser == null)
                {
                    return new APIResponseModel
                    {
                        Message = "UserId not found.",
                        IsSuccess = false
                    };
                }

                var requests = await _unitOfWork.CustomerSupportRepository.GetAllAsyncs(query => query.Where(c=>c.UserId == zaloUser.Id && c.RequestTypes.Type != "Yêu cầu hoàn tiền")
                                                                                                      .Include(r=>r.RequestTypes));
                if (requests == null)
                {
                    return new APIResponseModel
                    {
                        Message = "Request not found.",
                        IsSuccess = false
                    };
                }
                var result = requests.Select(b => new
                {
                    CustomerSupportID = b.CusSupportId,
                    UserId = b.UserId,
                    FullName = zaloUser.FullName,
                    RequestTypeId = b.RequestTypeId,
                    Priority = b.RequestTypes?.Priority,
                    Type = b.RequestTypes?.Type,
                    Description = b.Description,
                    CreateDate = b.CreateDate,
                    DateResolved = b.DateResolved,
                    Status = b.Status
                }).ToList();


                return new APIResponseModel
                {
                    Message = "Found Request.",
                    IsSuccess = true,
                    Data = result
                };

            }
            catch (Exception ex)
            {
                return new APIResponseModel
                {
                    Message = ex.Message,
                    IsSuccess = false,
                };
            }
        }
    }
}
