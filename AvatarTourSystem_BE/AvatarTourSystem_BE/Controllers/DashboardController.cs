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
        [HttpGet("count-account")]
        public async Task<IActionResult> CountAccountRole()
        {
            var monthlyRevenue = await _dashboardService.CountAccountRole();
            return Ok(monthlyRevenue);
        }
        [HttpGet("count-booking")]
        public async Task<IActionResult> CountBooking()
        {
            var monthlyRevenue = await _dashboardService.CountBooking();
            return Ok(monthlyRevenue);
        }
        [HttpGet("count-request")]
        public async Task<IActionResult> CountRequest()
        {
            var monthlyRevenue = await _dashboardService.CountRequest();
            return Ok(monthlyRevenue);
        }
        [HttpGet("count-package-in-day")]
        public async Task<IActionResult> CountPackageInday()
        {
            var monthlyRevenue = await _dashboardService.CountPackageInday();
            return Ok(monthlyRevenue);
        }
    }
}
