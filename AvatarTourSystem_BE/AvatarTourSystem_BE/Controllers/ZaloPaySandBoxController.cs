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
    }
}
