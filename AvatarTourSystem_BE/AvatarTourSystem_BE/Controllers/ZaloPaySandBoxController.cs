using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Services.Services;

namespace AvatarTourSystem_BE.Controllers
{
    public class ZaloPaySandBoxController : Controller
    {
        private readonly IZaloPaySandBoxService _zaloPaySandBoxService;
        public ZaloPaySandBoxController(IZaloPaySandBoxService zaloPaySandBoxService)
        {
            _zaloPaySandBoxService = zaloPaySandBoxService;
        }

        [HttpPost("callback-zalopay-sandbox")]
        public async Task<IActionResult> HandleCallback([FromBody] object callbackData)
        {
            var result = await _zaloPaySandBoxService.HandleCallback(callbackData);
            return Ok(result);
        }

        [HttpPost("refund-amount")]
        public async Task<IActionResult> ProcessRefund(string zptransid, long amount, string description)
        {
            var result = await _zaloPaySandBoxService.ProcessRefund( zptransid,  amount,  description);
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
