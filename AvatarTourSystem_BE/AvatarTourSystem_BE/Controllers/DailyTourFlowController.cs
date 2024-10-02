using BusinessObjects.ViewModels.DailyTour;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Services.Services;

namespace AvatarTourSystem_BE.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class DailyTourFlowController : ControllerBase
    {
        private readonly IDailyTourFlowService _dailyTourFlowService;
        public DailyTourFlowController(IDailyTourFlowService dailyTourFlowService)
        {
            _dailyTourFlowService = dailyTourFlowService;
        }

        [HttpPost("daily-tours")]
        public async Task<IActionResult> CreateDailyTourFlowAsync( DailyTourFlowModel dailyTourFlowModel)
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

        [HttpGet("daily-tours/{id}")]
        public async Task<IActionResult> GetDailyTourFlowByIdAsync(string dailyTourId)
        {
            var response = await _dailyTourFlowService.GetDailyTourDetails(dailyTourId);
            return Ok(response);
        }

        //[HttpGet("GetAllDailyToursAsync")]
        //public async Task<IActionResult> GetAllDailyToursAsync()
        //{
        //    var response = await _dailyTourFlowService.GetAllDailyTours();
        //    return Ok(response);
        //}

        [HttpGet("daily-tours")]
        public async Task<IActionResult> GetAllDailysToursAsync()
        {
            var response = await _dailyTourFlowService.GetAllDailysTours();
            return Ok(response);
        }

        [HttpGet("daily-tours-discount")]
        public async Task<IActionResult> GetDailyToursHaveDiscount()
        {
            var response = await _dailyTourFlowService.GetDailyToursHaveDiscount();
            return Ok(response);
        }


        [HttpGet("daily-tours-poi")]
        public async Task<IActionResult> GetDailyToursHavePOI()
        {
            var response = await _dailyTourFlowService.GetDailyToursHavePOI();
            return Ok(response);
        }
    }
}
