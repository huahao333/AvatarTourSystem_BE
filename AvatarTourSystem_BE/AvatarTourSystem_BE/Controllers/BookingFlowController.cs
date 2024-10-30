using BusinessObjects.Models;
using BusinessObjects.ViewModels.Account;
using BusinessObjects.ViewModels.Booking;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Services.Services;

namespace AvatarTourSystem_BE.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class BookingFlowController : ControllerBase
    {
        private readonly IBookingFlowService _bookingFlowService;
        private readonly IVNPayService _vnPayService;

        public BookingFlowController(IBookingFlowService bookingFlowService, IVNPayService vnPayService)
        {
            _bookingFlowService = bookingFlowService;
            _vnPayService = vnPayService;
        }

        [HttpPost("booking-zaloId")]
        public async Task<IActionResult> GetBookingFlowByZaloIdAsync(AccountZaloIdModel accountZaloIdModel)
        {
            var result = await _bookingFlowService.GetBookingFlowByZaloIdAsync(accountZaloIdModel);
            return Ok(result);
        }

        [HttpPost("create-booking")]
        public async Task<IActionResult> CreateBookingFlowAsync(BookingFlowCreateModel createModel)
        {
            var result = await _bookingFlowService.CreateBookingFlowAsync(createModel);
            return Ok(result);
        }

        [HttpPost("share-booking-phone")]
        public async Task<IActionResult> ShareTicketByPhoneNumber(BookingPhoneNumberShareTicket updateModel)
        {
            var result = await _bookingFlowService.ShareTicketByPhoneNumber(updateModel);
            return Ok(result);
        }

        [HttpPut("update-booking-by-zalo")]
        public async Task<IActionResult> UpdateBookingByZaloIdFlowAsync(BookingModel updateModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _bookingFlowService.UpdateBookingByZaloIdFlowAsync(updateModel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("vnpay-callback")]
        public async Task<IActionResult> PaymentCallback()
        {
            if (Request.QueryString.HasValue)
            {
                var result = await _vnPayService.ConfirmPaymentAsync(Request.Query);
                return Ok(result);
            }
            return StatusCode(500, "No query data");
        }

        [HttpPost("vnpay")]
        public async Task<IActionResult> VnPaymentRequest([FromBody] BookingFlowModel model)
        {
            try
            {
                var paymentUrl = await _vnPayService.CreatePaymentRequestAsync(model.BookingId);
                return Ok(new { url = paymentUrl });
                //return Redirect(paymentUrl.Data);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, "An error occurred while processing the payment request.");
            }
        }

        
    }
}
