using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace AvatarTourSystem_BE.Controllers
{
    [Route("api/dashboard")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;
        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;   
        }
        [HttpGet("active-zalo-users-count")]
        public async Task<IActionResult> GetCountActiveZaloUser()
        {
            var response = await _dashboardService.GetCountActiveZaloUser();
            return Ok(response);
        }

        [HttpGet("active-zalo-users")]
        public async Task<IActionResult> GetActiveZaloUser()
        {
            var response = await _dashboardService.GetActiveZaloUser();
            return Ok(response);
        }
        [HttpGet("monthly-bookings")]
        public async Task<IActionResult> GetMonthlyRevenue(int month, int year)
        {
            var monthlyRevenue = await _dashboardService.GetMonthlyBookings(month, year);
            return Ok(monthlyRevenue);
        }

    }
}
