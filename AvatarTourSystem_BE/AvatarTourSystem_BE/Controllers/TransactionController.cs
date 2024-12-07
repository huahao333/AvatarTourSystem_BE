using Microsoft.AspNetCore.Mvc;
using Services.Services;

namespace AvatarTourSystem_BE.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ZaloPaymentService _paymentService;
        public TransactionController(ZaloPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet("get-order-status")]
        public async Task<IActionResult> GetOrderStatus([FromQuery] string orderId, [FromQuery] string appId, [FromQuery] string privateKey)
        {
           var response = await _paymentService.GetOrderStatusAsync(orderId, appId, privateKey);
           return Ok(response);
        }
    }
}
