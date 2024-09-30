using BusinessObjects.ViewModels.DailyTour;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Services.Services;

namespace AvatarTourSystem_BE.Controllers
{
    [Route("api/dailytourflows")]
    [ApiController]
    public class DailyTourFlowController : ControllerBase
    {
        private readonly IDailyTourFlowService _dailyTourFlowService;
        public DailyTourFlowController(IDailyTourFlowService dailyTourFlowService)
        {
            _dailyTourFlowService = dailyTourFlowService;
        }

        [HttpPost("CreateDailyTourFlowAsync")]
        public async Task<IActionResult> CreateDailyTourFlowAsync([FromForm] DailyTourFlowModel dailyTourFlowModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _dailyTourFlowService.CreateDailyTourFlow(dailyTourFlowModel);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result);
            }
        }

        [HttpGet("GetDailyTourFlowByIdAsync/{dailyTourId}")]
        public async Task<IActionResult> GetDailyTourFlowByIdAsync(string dailyTourId)
        {
            var response = await _dailyTourFlowService.GetDailyTourDetails(dailyTourId);
            return Ok(response);
        }

        [HttpGet("GetAllDailyToursAsync")]
        public async Task<IActionResult> GetAllDailyToursAsync()
        {
            var response = await _dailyTourFlowService.GetAllDailyTours();
            return Ok(response);
        }
    }
}
