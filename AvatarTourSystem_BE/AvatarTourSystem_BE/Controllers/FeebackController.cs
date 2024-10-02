using BusinessObjects.ViewModels.Feedback;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace AvatarTourSystem_BE.Controllers
{
        [Route("api/feedback")]
        [ApiController]
        public class FeebackController : ControllerBase
        {
            private readonly IFeedbackService _feedbakService;

            public FeebackController(IFeedbackService feedbakService)
            {
                _feedbakService = feedbakService;
            }

        [HttpGet("GetAllFeedbacksAsync")]
        public async Task<IActionResult> GetAllFeedbacksAsync()
        {
            var response = await _feedbakService.GetAllFeedbacks();
            return Ok(response);
        }

        [HttpGet("GetFeedbacksByStatusAsync")]
        public async Task<IActionResult> GetFeedbacksByStatusAsync()
        {
            var response = await _feedbakService.GetFeedbackByStatus();
            return Ok(response);
        }

        [HttpGet("GetFeedbackByIdAsync/{id}")]
        public async Task<IActionResult> GetFeedbackByIdAsync(string id)
        {
            var response = await _feedbakService.GetFeedbackById(id);
            return Ok(response);
        }

        [HttpGet("GetFeedbackByUserIdAsync/{userId}")]
        public async Task<IActionResult> GetFeedbackByUserIdAsync(string userId)
        {
            var response = await _feedbakService.GetFeedbackByUserId(userId);
            return Ok(response);
        }

        [HttpGet("GetFeedbackByBookingIdAsync/{bookingId}")]
        public async Task<IActionResult> GetFeedbackByBookingIdAsync(string bookingId)
        {
            var response = await _feedbakService.GetFeedbackByBookingId(bookingId);
            return Ok(response);
        }

        [HttpPost("CreateFeedbackAsync")]
        public async Task<IActionResult> CreateFeedbackAsync([FromBody] FeedbackCreateModel feedbackCreateModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _feedbakService.CreateFeedback(feedbackCreateModel);
            return Ok(response);
        }

        [HttpPut("UpdateFeedbackAsync")]
        public async Task<IActionResult> UpdateFeedbackAsync([FromBody] FeedbackUpdateModel feedbackUpdateModel)
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

        [HttpDelete("DeleteFeedbackAsync/{id}")]
        public async Task<IActionResult> DeleteFeedbackAsync(string id)
        {
            var response = await _feedbakService.DeleteFeedback(id);
            return Ok(response);
        }
    }
}
