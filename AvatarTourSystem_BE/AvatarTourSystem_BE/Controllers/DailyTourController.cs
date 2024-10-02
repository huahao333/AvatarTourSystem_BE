using BusinessObjects.ViewModels.DailyTour;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace AvatarTourSystem_BE.Controllers
{
    [Route("api/daily-tour")]
    [ApiController]
    public class DailyTourController : ControllerBase
    {
        private readonly IDailyTourService _dailyTourService;
        public DailyTourController(IDailyTourService dailyTourService)
        {
            _dailyTourService = dailyTourService;
        }
        [HttpGet("GetAllDailyToursAsync")]
        public async Task<IActionResult> GetAllDailyToursAsync()
        {
            var response = await _dailyTourService.GetAllDailyTour();
            return Ok(response);
        }

        [HttpGet("GetDailyToursByStatusAsync")]
        public async Task<IActionResult> GetDailyToursByStatusAsync()
        {
            var response = await _dailyTourService.GetDailyTourByStatus();
            return Ok(response);
        }

        [HttpGet("GetDailyTourByIdAsync/{dailyTourId}")]
        public async Task<IActionResult> GetDailyTourByIdAsync(string dailyTourId)
        {
            var response = await _dailyTourService.GetDailyTourById(dailyTourId);
            return Ok(response);
        }

        [HttpPost("CreateDailyTourAsync")]
        public async Task<IActionResult> CreateDailyTourAsync([FromForm] DailyTourCreateModel createModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _dailyTourService.CreateDailyTour(createModel);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result);
            }
        }

        [HttpPut("UpdateDailyTourAsync")]
        public async Task<IActionResult> UpdateDailyTourAsync([FromForm] DailyTourUpdateModel updateModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _dailyTourService.UpdateDailyTour(updateModel);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result);
            }
        }

        [HttpDelete("DeleteDailyTourAsync/{dailyTourId}")]
        public async Task<IActionResult> DeleteDailyTourAsync(string dailyTourId)
        {
            var result = await _dailyTourService.DeleteDailyTour(dailyTourId);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result);
            }
        }

        [HttpGet("GetDailyToursByPackageTourIdAsync/{packId}")]
        public async Task<IActionResult> GetDailyToursByPackageTourIdAsync(string packId)
        {
            var response = await _dailyTourService.GetDailyTourByPackageTour(packId);
            return Ok(response);
        }

    }
}
