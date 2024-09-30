using BusinessObjects.ViewModels.Booking;
using BusinessObjects.ViewModels.BookingByRevenue;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace AvatarTourSystem_BE.Controllers
{
    [Route("api/bookingbyrevenues")]
    [ApiController]
    public class BookingByRevenueController : Controller
    {
        private readonly IBookingByRevenueService _bookingByRevenueService;

        public BookingByRevenueController(IBookingByRevenueService bookingByRevenueService)
        {
            _bookingByRevenueService = bookingByRevenueService;
        }
        [HttpGet("GetActiveBookingByRevenuesAsync")]
        public async Task<IActionResult> GetListActiveBookingByRevenuesAsync()
        {
            var result = await _bookingByRevenueService.GetActiveBookingByRevenuesAsync();
            return Ok(result);
        }

        [HttpGet("GetAllBookingByRevenuesAsync")]
        public async Task<IActionResult> GetListBookingByRevenuesAsync()
        {
            var result = await _bookingByRevenueService.GetBookingByRevenuesAsync();
            return Ok(result);
        }

        [HttpGet("GetBookingByRevenueByIdAsync/{id}")]
        public async Task<IActionResult> GetBookingByRevenueById(string id)
        {
            var result = await _bookingByRevenueService.GetBookingByRevenueByIdAsync(id);
            return Ok(result);
        }

        [HttpPost("CreateBookingByRevenueAsync")]
        public async Task<IActionResult> CreateBookingByRevenueAsync([FromForm] BookingByRevenueCreateModel createModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _bookingByRevenueService.CreateBookingByRevenueAsync(createModel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("UpdateBookingByRevenueAsync")]
        public async Task<IActionResult> UpdateBookingByRevenueAsync([FromForm] BookingByRevenueUpdateModel updateModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _bookingByRevenueService.UpdateBookingByRevenueAsync(updateModel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeleteBookingByRevenueAsync/{id}")]
        public async Task<IActionResult> DeleteBookingByRevenue(string id)
        {
            var result = await _bookingByRevenueService.DeleteBookingByRevenue(id);
            return Ok(result);
        }
    }
}
