using BusinessObjects.ViewModels.CustomerSupport;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace AvatarTourSystem_BE.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class CustomerSupportController : Controller
    {
        private readonly ICustomerSupportService _customerSupportService;
        public CustomerSupportController(ICustomerSupportService customerSupportService)
        {
            _customerSupportService = customerSupportService;
        }

        [HttpPost("customer-support")]
        public async Task<IActionResult> CreateCustomerSupport(CustomerSupportCreateModel createModel)
        {
            var response = await _customerSupportService.CreateCustomerSupport(createModel);
            return Ok(response);
        }

        [HttpPut("customer-support")]
        public async Task<IActionResult> UpdateCustomerSupport(CustomerSupportUpdateModel updateModel)
        {
            var response = await _customerSupportService.UpdateCustomerSupport(updateModel);
            return Ok(response);
        }

        [HttpDelete("customer-support/{id}")]
        public async Task<IActionResult> DeleteCustomerSupport(string id)
        {
            var response = await _customerSupportService.DeleteCustomerSupport(id);
            return Ok(response);
        }

        [HttpGet("customer-support")]
        public async Task<IActionResult> GetAllCustomerSupport()
        {
            var response = await _customerSupportService.GetAllCustomerSupport();
            return Ok(response);
        }

        [HttpGet("customer-supports-cus/{cusId}")]
        public async Task<IActionResult> GetCustomerSupportById(string cusId)
        {
            var response = await _customerSupportService.GetCustomerSupportById(cusId);
            return Ok(response);
        }

        [HttpGet("customer-supports-user/{userId}")]
        public async Task<IActionResult> GetCustomerSupportByUserId(string userId)
        {
            var response = await _customerSupportService.GetCustomerSupportByUserId(userId);
            return Ok(response);
        }

        [HttpGet("customer-support-active")]
        public async Task<IActionResult> GetCustomerSupportByStatus()
        {
            var response = await _customerSupportService.GetCustomerSupportByStatus();
            return Ok(response);
        }


        [HttpGet("customer-supports")]
        public async Task<IActionResult> GetAllRequest()
        {
            var response = await _customerSupportService.GetAllRequest();
            return Ok(response);
        }

        [HttpPost("customer-supports-status")]
        public async Task<IActionResult> UpdateStatusCustomerSupport(CustomerSupportStatusViewModel customerSupportStatusViewModel)
        {
            var response = await _customerSupportService.UpdateStatusCustomerSupport(customerSupportStatusViewModel);
            return Ok(response);
        }
        [HttpPost("customer-supports-user-request-refund")]
        public async Task<IActionResult> CreateRequestCustomerSupportForRefund(CustomerSupportRequestCreateModel customerSupportRequestCreate)
        {
            var response = await _customerSupportService.CreateRequestCustomerSupportForRefund(customerSupportRequestCreate);
            return Ok(response);
        }
    }
}
