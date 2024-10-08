using BusinessObjects.Models;
using BusinessObjects.ViewModels.Booking;
using BusinessObjects.ViewModels.Revenue;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace AvatarTourSystem_BE.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class RevenueController : Controller
    {
        private readonly IRevenueService _revenueService;

        public RevenueController(IRevenueService revenueService)
        {
            _revenueService = revenueService;
        }

        [HttpGet("revenues")]
        public async Task<IActionResult> GetAllRevenuesAsync()
        {
            var result = await _revenueService.GetRevenuesAsync();
            return Ok(result);
        }

        [HttpGet("revenues-active")]
        public async Task<IActionResult> GetActiveRevenuesAsync()
        {
            var result = await _revenueService.GetActiveRevenuesAsync();
            return Ok(result);
        }

        [HttpGet("revenue/{id}")]
        public async Task<IActionResult> GetRevenueByIdAsync(string id)
        {
            var result = await _revenueService.GetRevenueByIdAsync(id);
            return Ok(result);
        }

        [HttpPost("revenue")]
        public async Task<IActionResult> CreateRevenueAsync( RevenueCreateModel createModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _revenueService.CreateRevenueAsync(createModel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("revenue")]
        public async Task<IActionResult> UpdateRevenueAsync( RevenueUpdateModel updateModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _revenueService.UpdateRevenueAsync(updateModel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("revenue/{id}")]
        public async Task<IActionResult> DeleteRevenueAsync(string id)
        {
            var result = await _revenueService.DeleteRevenue(id);
            return Ok(result);
        }
    }
}
