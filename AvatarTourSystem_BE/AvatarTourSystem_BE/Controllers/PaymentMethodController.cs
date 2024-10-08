using BusinessObjects.ViewModels.PaymentMethod;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace AvatarTourSystem_BE.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class PaymentMethodController : ControllerBase
    {
        private readonly IPaymentMethodService _paymentMethodService;
        public PaymentMethodController(IPaymentMethodService paymentMethodService)
        {
            _paymentMethodService = paymentMethodService;
        }
        [HttpGet("payment-methods")]
        public async Task<IActionResult> GetAllPaymentMethods()
        {
            var result = await _paymentMethodService.GetAllPaymentMethods();
            return Ok(result);
        }

        [HttpGet("payment-methods-active")]
        public async Task<IActionResult> GetActivePaymentMethods()
        {
            var result = await _paymentMethodService.GetPaymentMethodsByStatus();
            return Ok(result);
        }

        [HttpGet("payment-method/{id}")]
        public async Task<IActionResult> GetPaymentMethodByIdAsync(string id)
        {
            var result = await _paymentMethodService.GetPaymentMethodById(id);
            return Ok(result);
        }

        [HttpPost("payment-method")]
        public async Task<IActionResult> CreatePaymentMethod( PaymentMethodCreateModel createModel)
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

        [HttpPut("payment-method")]
        public async Task<IActionResult> UpdatePaymentMethod( PaymentMethodUpdateModel updateModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _paymentMethodService.UpdatePaymentMethod(updateModel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("payment-method/{id}")]
        public async Task<IActionResult> DeletePaymentMethod(string id)
        {
            var result = await _paymentMethodService.DeletePaymentMethod(id);
            return Ok(result);
        }

    }
}
