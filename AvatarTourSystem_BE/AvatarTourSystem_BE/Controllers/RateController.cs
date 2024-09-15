using BusinessObjects.ViewModels.Rate;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace AvatarTourSystem_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RateController : ControllerBase
    {
        private readonly IRateService _rateService;
        public RateController(IRateService rateService)
        {
            _rateService = rateService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllRate()
        {
            var result = await _rateService.GetAllRate();
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRateById(string id)
        {
            var result = await _rateService.GetRateById(id);
            return Ok(result);
        }
        [HttpGet("GetRateByBookingId/{bookingId}")]
        public async Task<IActionResult> GetRateByBookingId(string bookingId)
        {
            var result = await _rateService.GetRateByBookingId(bookingId);
            return Ok(result);
        }
        [HttpGet("GetRateByUserId/{userId}")]
        public async Task<IActionResult> GetRateByUserId(string userId)
        {
            var result = await _rateService.GetRateByUserId(userId);
            return Ok(result);
        }
        [HttpGet("GetRateByStatus")]
        public async Task<IActionResult> GetRateByStatus()
        {
            var result = await _rateService.GetRateByStatus();
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> CreateRate([FromForm] RateCreateModel rate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _rateService.CreateRate(rate);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpPut]
        public async Task<IActionResult> UpdateRate([FromForm] RateUpdateModel rate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _rateService.UpdateRate(rate);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRate(string id)
        {
            var result = await _rateService.DeleteRate(id);
            return Ok(result);
        }


    }
}
