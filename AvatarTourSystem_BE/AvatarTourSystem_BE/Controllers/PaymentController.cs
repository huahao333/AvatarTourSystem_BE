using BusinessObjects.ViewModels.Booking;
using BusinessObjects.ViewModels.City;
using BusinessObjects.ViewModels.Rate;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Services.Common;
using Services.Interfaces;
using Services.Services;
using static Services.Common.ZaloPayHelper.ZaloPayHelper;

namespace AvatarTourSystem_BE.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {

        private readonly IZaloPayService _zaloPayService;
        private readonly ILogger<PaymentController> _logger;
        private readonly ICityService _CityService;

        public PaymentController(
            IZaloPayService zaloPayService,
            ICityService cityService,
            ILogger<PaymentController> logger)
        {
             _zaloPayService = zaloPayService;
            _CityService = cityService;
            _logger = logger;
        }

        [HttpPost("zalo-callback")]
        public IActionResult HandleCallback([FromBody] ZaloPayCallbackRequest callback )
        {
            try
            {
                CityCreateModel createModel = new CityCreateModel { CityName = (string)JsonConvert.DeserializeObject(callback.ToString()), Status = BusinessObjects.Enums.EStatus.Disabled};
                _logger.LogInformation("Received ZaloPay callback: {@Callback}", callback);

                if (callback?.Data == null)
                {
                    return BadRequest(new ZaloPayCallbackResponse
                    {
                        returnCode = 0,
                        returnMessage = "Invalid callback"
                    });
                }

                //var isValid = _zaloPayService.ValidateCallback(callback.Data);
                var isValid = _zaloPayService.ValidateCallback(callback.Data);
                if (!isValid)
                {
                    return BadRequest(new ZaloPayCallbackResponse
                    {
                        returnCode = 0,
                        returnMessage = "Invalid mac or overall mac"
                    });
                }

                if (callback.Data.resultCode == 1)
                {
                    var result = _CityService.CreateCityAsync(createModel);
                    return Ok(new ZaloPayCallbackResponse
                    {
                        returnCode = 1,
                        returnMessage = callback.Data.extradata,
                    });

                }
                return BadRequest(new ZaloPayCallbackResponse
                {
                    returnCode = 0,
                    returnMessage = "Payment failure"
                });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing ZaloPay callback");
                return StatusCode(500, new { returncode = 0, returnmessage = "internal server error" });
            }
        }

        //[HttpPost("create-booking")]
        //public async Task<IActionResult> CreateBookingFlowAsync(BookingFlowCreateModel createModel)
        //{
        //    var result = await _zaloPayService.CreateBookingAsync(createModel);
        //    return Ok(result);
        //}
    }
}
