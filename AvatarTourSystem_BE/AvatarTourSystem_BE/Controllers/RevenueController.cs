using BusinessObjects.Models;
using BusinessObjects.ViewModels.Booking;
using BusinessObjects.ViewModels.Revenue;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace AvatarTourSystem_BE.Controllers
{
    [Route("api/revenues")]
    [ApiController]
    public class RevenueController : Controller
    {
        private readonly IRevenueService _revenueService;

        public RevenueController(IRevenueService revenueService)
        {
            _revenueService = revenueService;
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetListActiveRevenuesAsync()
        {
            var result = await _revenueService.GetActiveRevenuesAsync();
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetListRevenuesAsync()
        {
            var result = await _revenueService.GetRevenuesAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRevenueById(string id)
        {
            var result = await _revenueService.GetRevenueByIdAsync(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRevenueAsync([FromForm] RevenueCreateModel createModel)
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

        [HttpPut]
        public async Task<IActionResult> UpdateRevenueAsync([FromForm] RevenueUpdateModel updateModel)
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
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRevenue(string id)
        {
            var result = await _revenueService.DeleteRevenue(id);
            return Ok(result);
        }
    }
}
