using BusinessObjects.ViewModels.Feedback;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace AvatarTourSystem_BE.Controllers
{
        [Route("api/[controller]")]
        [ApiController]
        public class FeebackController : ControllerBase
        {
            private readonly IFeedbackService _feedbakService;

            public FeebackController(IFeedbackService feedbakService)
            {
                _feedbakService = feedbakService;
            }

            [HttpGet]
            public async Task<IActionResult> GetAllFeedbacks()
            {
                var response = await _feedbakService.GetAllFeedbacks();
                return Ok(response);
            }
            [HttpGet("GetByStatus")]
            public async Task<IActionResult> GetFeedbackByStatus()
            {
            var response = await _feedbakService.GetFeedbackByStatus();
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFeedbackById(string id)
        {
            var response = await _feedbakService.GetFeedbackById(id);
            return Ok(response);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetFeedbackByUserId(string userId)
        {
            var response = await _feedbakService.GetFeedbackByUserId(userId);
            return Ok(response);
        }

        [HttpGet("booking/{bookingId}")]
        public async Task<IActionResult> GetFeedbackByBookingId(string bookingId)
        {
            var response = await _feedbakService.GetFeedbackByBookingId(bookingId);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateFeedback(FeedbackCreateModel feedbackCreateModel)
        {
            var response = await _feedbakService.CreateFeedback(feedbackCreateModel);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateFeedback(FeedbackUpdateModel feedbackUpdateModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var response = await _feedbakService.UpdateFeedback(feedbackUpdateModel);
                return Ok(response);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFeedback(string id)
        {
            var response = await _feedbakService.DeleteFeedback(id);
            return Ok(response);
        }
    }
}
