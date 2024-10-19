using BusinessObjects.ViewModels.Notification;
using Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface INotificatonService
    {
        Task<APIResponseModel> GetAllNotificaiton();
        Task<APIResponseModel> GetNotificaitonByStatus();
        Task<APIResponseModel> GetNotificaitonById(string notificaitonId);
        Task<APIResponseModel> GetNotificaitonByUserId(string userId);
        Task<APIResponseModel> GetNotificaitonByZaloID(string zaloId);
        Task<APIResponseModel> CreateNotificaiton(NotificationCreateModel createModel);
        Task<APIResponseModel> CreateNotificaitonByZaloId(NotificationCreateByZaloIdModel createModel);
        Task<APIResponseModel> UpdateNotificaiton(NotificationUpdateModel updateModel);
        Task<APIResponseModel> DeleteNotificaiton(string notificaitonId);

    }
}
