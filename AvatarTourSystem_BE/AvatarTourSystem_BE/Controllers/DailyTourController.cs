using BusinessObjects.ViewModels.DailyTour;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace AvatarTourSystem_BE.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class DailyTourController : ControllerBase
    {
        private readonly IDailyTourService _dailyTourService;
        public DailyTourController(IDailyTourService dailyTourService)
        {
            _dailyTourService = dailyTourService;
        }
        [HttpGet("dailies-tour")]
        public async Task<IActionResult> GetAllDailyToursAsync()
        {
            var response = await _dailyTourService.GetAllDailyTour();
            return Ok(response);
        }

        [HttpGet("dailies-tour-active")]
        public async Task<IActionResult> GetDailyToursByStatusAsync()
        {
            var response = await _dailyTourService.GetDailyTourByStatus();
            return Ok(response);
        }

        [HttpGet("dailies-tour/{id}")]
        public async Task<IActionResult> GetDailyTourByIdAsync(string id)
        {
            var response = await _dailyTourService.GetDailyTourById(id);
            return Ok(response);
        }

        [HttpPost("dailies-tour")]
        public async Task<IActionResult> CreateDailyTourAsync(DailyTourCreateModel createModel)
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

        [HttpPut("dailies-tour")]
        public async Task<IActionResult> UpdateDailyTourAsync(DailyTourUpdateModel updateModel)
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

        [HttpDelete("dailies-tour/{id}")]
        public async Task<IActionResult> DeleteDailyTourAsync(string id)
        {
            var result = await _dailyTourService.DeleteDailyTour(id);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result);
            }
        }

        [HttpGet("dailies-tour-package/{packId}")]
        public async Task<IActionResult> GetDailyToursByPackageTourIdAsync(string packId)
        {
            var response = await _dailyTourService.GetDailyTourByPackageTour(packId);
            return Ok(response);
        }

    }
}
