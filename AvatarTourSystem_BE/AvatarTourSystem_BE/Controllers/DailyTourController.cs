using BusinessObjects.ViewModels.DailyTour;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace AvatarTourSystem_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DailyTourController : ControllerBase
    {
        private readonly IDailyTourService _dailyTourService;
        public DailyTourController(IDailyTourService dailyTourService)
        {
            _dailyTourService = dailyTourService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllDailyTour()
        {
            var response = await _dailyTourService.GetAllDailyTour();
            return Ok(response);
        }
        [HttpGet("GetDailyTourByStatus")]
        public async Task<IActionResult> GetDailyTourByStatus()
        {
            var response = await _dailyTourService.GetDailyTourByStatus();
            return Ok(response);
        }
        [HttpGet("GetDailyTourById")]
        public async Task<IActionResult> GetDailyTourById(string dailyTourId)
        {
            var response = await _dailyTourService.GetDailyTourById(dailyTourId);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> CreateDailyTour(DailyTourCreateModel createModel)
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
        [HttpPut]
        public async Task<IActionResult> UpdateDailyTour(DailyTourUpdateModel updateModel)
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
        [HttpDelete]
        public async Task<IActionResult> DeleteDailyTour(string dailyTourId)
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
        [HttpGet("GetDailyTourByPackageTourID")]
        public async Task<IActionResult> GetDailyTourByPackageTour(string packId)
        {
            var response = await _dailyTourService.GetDailyTourByPackageTour(packId);
            return Ok(response);
        }

    }
}
