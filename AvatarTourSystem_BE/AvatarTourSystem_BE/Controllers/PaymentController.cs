using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Services.Interfaces;

namespace AvatarTourSystem_BE.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase

    {
        private readonly IZaloPayService _zaloPayService;

        public PaymentController(IZaloPayService zaloPayService)
        {
            _zaloPayService = zaloPayService;
        }

        [HttpPost("zalo-callback")]
        public async Task<IActionResult> PostZaloPay([FromBody] dynamic cbdata)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var dataStr = Convert.ToString(cbdata["data"]);
                var reqMac = Convert.ToString(cbdata["mac"]);

                // Verify the callback
                if (!_zaloPayService.VerifyCallback(dataStr, reqMac))
                {
                    // Callback is not valid
                    result["return_code"] = -1;
                    result["return_message"] = "MAC not equal";
                    return Ok(result);
                }

                // Deserialize the data
                var dataJson = JsonConvert.DeserializeObject<Dictionary<string, object>>(dataStr);

                // Update the order's status
                await _zaloPayService.HandleCallbackAsync(dataJson);

                result["return_code"] = 1;
                result["return_message"] = "success";
            }
            catch (Exception ex)
            {
                result["return_code"] = 0; // ZaloPay server will callback again (maximum 3 times)
                result["return_message"] = ex.Message;
            }

            // Notify the result to the ZaloPay server
            return Ok(result);
        }
    }
}
