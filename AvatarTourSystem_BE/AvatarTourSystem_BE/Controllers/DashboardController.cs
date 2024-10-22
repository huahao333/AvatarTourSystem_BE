using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace AvatarTourSystem_BE.Controllers
{
    [Route("api/v1")]
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
            var response = await _dashboardService.GetActiveZaloUserCount();
            return Ok(response);
        }
        [HttpGet("active-zalo-users")]
        public async Task<IActionResult> GetActiveZaloUser()
        {
            var response = await _dashboardService.GetActiveZaloUser();
            return Ok(response);
        }
        //[HttpGet("monthly-revenue")]
        //public async Task<IActionResult> GetMonthlyRevenue(int month, int year)
        //{
        //    var monthlyRevenue = await _dashboardService.GetMonthlyRevenue(month, year);
        //    return Ok(monthlyRevenue);
        //}
        [HttpGet("monthly-bookings")]
        public async Task<IActionResult> GetMonthlyBookings(int month, int year)
        {
            var monthlyRevenue = await _dashboardService.GetMonthlyBookings(month, year);
            return Ok(monthlyRevenue);
        }
        [HttpGet("monthly-tickets")]
        public async Task<IActionResult> GetMonthlyTicketsByType(string typeId, int month, int year)
        {
            var monthlyRevenue = await _dashboardService.GetMonthlyTicketsByType(typeId, month, year);
            return Ok(monthlyRevenue);
        }
        [HttpGet("monthly-tours")]
        public async Task<IActionResult> GetMonthlyTours(int month, int year)
        {
            var monthlyRevenue = await _dashboardService.GetMonthlyTours(month, year);
            return Ok(monthlyRevenue);
        }
    }
}
