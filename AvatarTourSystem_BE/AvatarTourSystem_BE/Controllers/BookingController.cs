using BusinessObjects.ViewModels.Booking;
using BusinessObjects.ViewModels.ServiceUsedByTicket;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace AvatarTourSystem_BE.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class BookingController : Controller
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpGet("bookings-active")]
        public async Task<IActionResult> GetListActiveBookingsAsync()
        {
            var result = await _bookingService.GetActiveBookingsAsync();
            return Ok(result);
        }

        [HttpGet("bookings")]
        public async Task<IActionResult> GetListBookingsAsync()
        {
            var result = await _bookingService.GetBookingsAsync();
            return Ok(result);
        }

        [HttpGet("bookings/{id}")]
        public async Task<IActionResult> GetBookingById(string id)
        {
            var result = await _bookingService.GetBookingByIdAsync(id);
            return Ok(result);
        }

        [HttpPost("booking")]
        public async Task<IActionResult> CreateBookingAsync( BookingCreateModel createModel)
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

        [HttpPut("booking")]
        public async Task<IActionResult> UpdateBookingAsync( BookingUpdateModel updateModel)
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

        [HttpDelete("booking/{id}")]
        public async Task<IActionResult> DeleteBooking(string id)
        {
            var result = await _bookingService.DeleteBooking(id);
            return Ok(result);
        }

        [HttpGet("booking-ticket")]
        public async Task<IActionResult> GetAllBookingsAsync()
        {
            var result = await _bookingService.GetAllBookingsAsync();
            return Ok(result);
        }

        [HttpPost("booking-dailytour")]
        public async Task<IActionResult> GetAllBookingsByDailyTourIdAsync(BookingByDailyTourIdViewModel bookingByDailyTourIdViewModel)
        {
            var result = await _bookingService.GetAllBookingsByDailyTourIdAsync(bookingByDailyTourIdViewModel);
            return Ok(result);
        }
    }
}
