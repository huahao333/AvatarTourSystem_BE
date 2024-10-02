using BusinessObjects.ViewModels.Notification;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace AvatarTourSystem_BE.Controllers
{
    [Route("api/notification")]
    [ApiController]
    public class NotificatonController : ControllerBase
    {
        private readonly INotificatonService _notuService;
        public NotificatonController(INotificatonService notuService)
        {
            _notuService = notuService;
        }
        [HttpGet("GetAllNotificationsAsync")]
        public async Task<IActionResult> GetAllNotifications()
        {
            var response = await _notuService.GetAllNotificaiton();
            return Ok(response);
        }

        [HttpGet("GetNotificationsByStatusAsync")]
        public async Task<IActionResult> GetNotificationsByStatus()
        {
            var response = await _notuService.GetNotificaitonByStatus();
            return Ok(response);
        }

        [HttpGet("GetNotificationByIdAsync/{notificationId}")]
        public async Task<IActionResult> GetNotificationById(string notificationId)
        {
            var response = await _notuService.GetNotificaitonById(notificationId);
            return Ok(response);
        }

        [HttpGet("GetNotificationsByUserIdAsync/{userId}")]
        public async Task<IActionResult> GetNotificationsByUserId(string userId)
        {
            var response = await _notuService.GetNotificaitonByUserId(userId);
            return Ok(response);
        }

        [HttpPost("CreateNotificationAsync")]
        public async Task<IActionResult> CreateNotification(NotificationCreateModel createModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _notuService.CreateNotificaiton(createModel);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result);
            }
        }

        [HttpPut("UpdateNotificationAsync")]
        public async Task<IActionResult> UpdateNotification(NotificationUpdateModel updateModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _notuService.UpdateNotificaiton(updateModel);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result);
            }
        }

        [HttpDelete("DeleteNotificationAsync/{notificationId}")]
        public async Task<IActionResult> DeleteNotification(string notificationId)
        {
            var response = await _notuService.DeleteNotificaiton(notificationId);
            return Ok(response);
        }


    }
}
