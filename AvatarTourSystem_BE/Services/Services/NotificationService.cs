using AutoMapper;
using BusinessObjects.Enums;
using BusinessObjects.Models;
using BusinessObjects.ViewModels.Notification;
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
    public class NotificationService : INotificatonService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public NotificationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<APIResponseModel> CreateNotificaiton(NotificationCreateModel createModel)
        {
            var newNotificaiton = _mapper.Map<Notification>(createModel);
            newNotificaiton.NotifyId = Guid.NewGuid().ToString();
            newNotificaiton.CreateDate = DateTime.Now;
            await _unitOfWork.NotificationRepository.AddAsync(newNotificaiton);
            _unitOfWork.Save();
            return new APIResponseModel
            {
                Message = "Create Notificaiton Successfully",
                IsSuccess = true,
                Data = newNotificaiton,
            };
        }

        public async Task<APIResponseModel> DeleteNotificaiton(string notificaitonId)
        {
            var notificaiton = await _unitOfWork.NotificationRepository.GetByIdStringAsync(notificaitonId);
            if (notificaiton == null)
            {
                return new APIResponseModel
                {
                    Message = "Notificaiton not found.",
                    IsSuccess = false,
                    Data = null
                };
            }
            if (notificaiton.Status == (int?)EStatus.IsDeleted)
            {
                return new APIResponseModel
                {
                    Message = "Notificaiton has already been deleted.",
                    IsSuccess = false,
                    Data = null
                };
            }
            var createDate = notificaiton.CreateDate;
            notificaiton.Status = (int)EStatus.IsDeleted;
            notificaiton.UpdateDate = DateTime.Now;
            notificaiton.CreateDate = createDate;
            var result = await _unitOfWork.NotificationRepository.UpdateAsync(notificaiton);
            _unitOfWork.Save();
            return new APIResponseModel
            {
                Message = "Delete Notificaiton Successfully",
                IsSuccess = true,
                Data = result,
            };

        }

        public async Task<APIResponseModel> GetAllNotificaiton()
        {
            var notificaitons = await _unitOfWork.NotificationRepository.GetAllAsync();
            return new APIResponseModel
            {
                Message = "Get All Notificaiton Successfully",
                IsSuccess = true,
                Data = notificaitons,
            };
        }

        public async Task<APIResponseModel> GetNotificaitonById(string notificaitonId)
        {
            var notificaiton = await _unitOfWork.NotificationRepository.GetByIdStringAsync(notificaitonId);
            if (notificaiton == null)
            {
                return new APIResponseModel
                {
                    Message = "Notificaiton not found.",
                    IsSuccess = false,
                    Data = null
                };
            }
            return new APIResponseModel
            {
                Message = "Get Notificaiton Successfully",
                IsSuccess = true,
                Data = notificaiton,
            };
        }

        public async Task<APIResponseModel> GetNotificaitonByStatus()
        {
            var notificaitons = await _unitOfWork.NotificationRepository.GetByConditionAsync(s => s.Status != -1);
            return new APIResponseModel
            {
                Message = "Get Notificaiton Successfully",
                IsSuccess = true,
                Data = notificaitons,
            };

        }

        public async Task<APIResponseModel> GetNotificaitonByUserId(string userId)
        {
            var notificaitons = await _unitOfWork.NotificationRepository.GetByConditionAsync(s => s.UserId == userId);
            return new APIResponseModel
            {
                Message = "Get Notificaiton Successfully",
                IsSuccess = true,
                Data = notificaitons,
            };
        }

        public async Task<APIResponseModel> UpdateNotificaiton(NotificationUpdateModel updateModel)
        {
            var notificaiton = await _unitOfWork.NotificationRepository.GetByIdGuidAsync(updateModel.NotifyId);
            if (notificaiton == null)
            {
                return new APIResponseModel
                {
                    Message = "Notificaiton not found.",
                    IsSuccess = false,
                    Data = null
                };
            }
            var createDate = notificaiton.CreateDate;
            notificaiton = _mapper.Map(updateModel, notificaiton);
            notificaiton.UpdateDate = DateTime.Now;
            notificaiton.CreateDate = createDate;
            var result = await _unitOfWork.NotificationRepository.UpdateAsync(notificaiton);
            _unitOfWork.Save();
            return new APIResponseModel
            {
                Message = "Update Notificaiton Successfully",
                IsSuccess = true,
                Data = result,
            };
        }
    }
}
