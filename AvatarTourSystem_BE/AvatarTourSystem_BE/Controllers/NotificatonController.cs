using BusinessObjects.ViewModels.Notification;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace AvatarTourSystem_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificatonController : ControllerBase
    {
        private readonly INotificatonService _notuService;
        public NotificatonController(INotificatonService notuService)
        {
            _notuService = notuService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllNotificaiton()
        {
            var response = await _notuService.GetAllNotificaiton();
            return Ok(response);
        }
        [HttpGet("GetNotificaitonByStatus")]
        public async Task<IActionResult> GetNotificaitonByStatus()
        {
            var response = await _notuService.GetNotificaitonByStatus();
            return Ok(response);
        }
        [HttpGet("{notificaitonId}")]
        public async Task<IActionResult> GetNotificaitonById(string notificaitonId)
        {
            var response = await _notuService.GetNotificaitonById(notificaitonId);
            return Ok(response);
        }
        [HttpGet("GetNotificaitonByUserId")]
        public async Task<IActionResult> GetNotificaitonByUserId(string userId)
        {
            var response = await _notuService.GetNotificaitonByUserId(userId);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> CreateNotificaiton(NotificationCreateModel createModel)
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
        [HttpPut]
        public async Task<IActionResult> UpdateNotificaiton(NotificationUpdateModel updateModel)
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
        [HttpDelete]
        public async Task<IActionResult> DeleteNotificaiton(string notificaitonId)
        {
            var response = await _notuService.DeleteNotificaiton(notificaitonId);
            return Ok(response);
        }


    }
}
