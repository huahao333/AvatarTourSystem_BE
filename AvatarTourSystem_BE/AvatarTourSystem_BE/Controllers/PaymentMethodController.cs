using BusinessObjects.ViewModels.PaymentMethod;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace AvatarTourSystem_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentMethodController : ControllerBase
    {
        private readonly IPaymentMethodService _paymentMethodService;
        public PaymentMethodController(IPaymentMethodService paymentMethodService)
        {
            _paymentMethodService = paymentMethodService;
        }
        [HttpGet]
        public async Task<IActionResult> GetPaymentMethods()
        {
            var result = await _paymentMethodService.GetAllPaymentMethods();
            return Ok(result);
        }
        [HttpGet("active")]
        public async Task<IActionResult> GetActivePaymentMethods()
        {
            var result = await _paymentMethodService.GetPaymentMethodsByStatus();
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPaymentMethodById(string id)
        {
            var result = await _paymentMethodService.GetPaymentMethodById(id);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> CreatePaymentMethod([FromForm] PaymentMethodCreateModel createModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _paymentMethodService.CreatePaymentMethod(createModel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePaymentMethod(string id, [FromForm] PaymentMethodUpdateModel updateModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _paymentMethodService.UpdatePaymentMethod(id, updateModel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePaymentMethod(string id)
        {
            var result = await _paymentMethodService.DeletePaymentMethod(id);
            return Ok(result);
        }

    }
}
