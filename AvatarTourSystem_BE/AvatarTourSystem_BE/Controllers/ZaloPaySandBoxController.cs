using BusinessObjects.ViewModels.Payment;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Services.Services;

namespace AvatarTourSystem_BE.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class ZaloPaySandBoxController : ControllerBase
    {
        private readonly IZaloPaySandBoxService _zaloPaySandBoxService;
        public ZaloPaySandBoxController(IZaloPaySandBoxService zaloPaySandBoxService)
        {
            _zaloPaySandBoxService = zaloPaySandBoxService;
        }

        [HttpPost("callback-zalopay-sandbox")]
        public async Task<IActionResult> HandleCallback(object callbackData)
        {
            var result = await _zaloPaySandBoxService.HandleCallback(callbackData);
            return Ok(result);
        }

        [HttpPost("refund-amount")]
        public async Task<IActionResult> ProcessRefund(RefundModel refundModel)
        {
            var result = await _zaloPaySandBoxService.ProcessRefund(refundModel);
            return Ok(result);
        }


        [HttpGet("payment-infor")]
        public async Task<IActionResult> GetAllPayment()
        {
            var result = await _zaloPaySandBoxService.GetAllPayment();
            return Ok(result);
        }
    }
}
