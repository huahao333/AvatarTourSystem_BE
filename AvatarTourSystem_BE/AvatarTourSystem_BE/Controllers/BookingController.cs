using BusinessObjects.ViewModels.Booking;
using BusinessObjects.ViewModels.ServiceUsedByTicket;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace AvatarTourSystem_BE.Controllers
{
    [Route("api/bookings")]
    [ApiController]
    public class BookingController : Controller
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpGet("GetActiveBookingsAsync")]
        public async Task<IActionResult> GetListActiveBookingsAsync()
        {
            var result = await _bookingService.GetActiveBookingsAsync();
            return Ok(result);
        }

        [HttpGet("GetAllBookingsAsync")]
        public async Task<IActionResult> GetListBookingsAsync()
        {
            var result = await _bookingService.GetBookingsAsync();
            return Ok(result);
        }

        [HttpGet("GetBookingByIdAsync/{id}")]
        public async Task<IActionResult> GetBookingById(string id)
        {
            var result = await _bookingService.GetBookingByIdAsync(id);
            return Ok(result);
        }

        [HttpPost("CreateBookingAsync")]
        public async Task<IActionResult> CreateBookingAsync([FromForm] BookingCreateModel createModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _bookingService.CreateBookingAsync(createModel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("UpdateBookingAsync")]
        public async Task<IActionResult> UpdateBookingAsync([FromForm] BookingUpdateModel updateModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _bookingService.UpdateBookingAsync(updateModel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeleteBookingAsync/{id}")]
        public async Task<IActionResult> DeleteBooking(string id)
        {
            var result = await _bookingService.DeleteBooking(id);
            return Ok(result);
        }
    }
}
