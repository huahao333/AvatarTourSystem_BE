using BusinessObjects.ViewModels.Rate;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace AvatarTourSystem_BE.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class RateController : ControllerBase
    {
        private readonly IRateService _rateService;
        public RateController(IRateService rateService)
        {
            _rateService = rateService;
        }
        [HttpGet("rates")]
        public async Task<IActionResult> GetAllRates()
        {
            var result = await _rateService.GetAllRate();
            return Ok(result);
        }

        [HttpGet("rate/{id}")]
        public async Task<IActionResult> GetRateByIdAsync(string id)
        {
            var result = await _rateService.GetRateById(id);
            return Ok(result);
        }

        [HttpGet("rates-booking/{bookingId}")]
        public async Task<IActionResult> GetRateByBookingId(string bookingId)
        {
            var result = await _rateService.GetRateByBookingId(bookingId);
            return Ok(result);
        }

        [HttpGet("rates-user/{userId}")]
        public async Task<IActionResult> GetRateByUserId(string userId)
        {
            var result = await _rateService.GetRateByUserId(userId);
            return Ok(result);
        }

        [HttpGet("rates-active")]
        public async Task<IActionResult> GetRatesByStatus()
        {
            var result = await _rateService.GetRateByStatus();
            return Ok(result);
        }

        [HttpPost("rate")]
        public async Task<IActionResult> CreateRate(RateCreateModel rate)
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
                return StatusCode(StatusCodes.Status404NotFound, ex.Message);
            }
        }
        [HttpPost("create-rate-zalouser-booking")]
        public async Task<IActionResult> CreateRateWithZaloAndBooking(RateCreateWithZaloModel rate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _rateService.CreaateRateWithZaloAndBooking(rate);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status404NotFound, ex.Message);
            }
        }

        [HttpPut("rate")]
        public async Task<IActionResult> UpdateRate(RateUpdateModel rate)
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

        [HttpDelete("rate/{id}")]
        public async Task<IActionResult> DeleteRate(string id)
        {
            var result = await _rateService.DeleteRate(id);
            return Ok(result);
        }
    }
}
