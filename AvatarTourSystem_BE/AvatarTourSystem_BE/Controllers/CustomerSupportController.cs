using BusinessObjects.ViewModels.CustomerSupport;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace AvatarTourSystem_BE.Controllers
{
    [Route("api/customer-support")]
    [ApiController]
    public class CustomerSupportController : ControllerBase
    {
        private readonly ICustomerSupportService _customerSupportService;

        [HttpPost("CreateCustomerSupportAsync")]
        public async Task<IActionResult> CreateCustomerSupport([FromForm] CustomerSupportCreateModel createModel)
        {
            var response = await _customerSupportService.CreateCustomerSupport(createModel);
            return Ok(response);
        }

        [HttpPut("UpdateCustomerSupportAsync")]
        public async Task<IActionResult> UpdateCustomerSupport([FromForm] CustomerSupportUpdateModel updateModel)
        {
            var response = await _customerSupportService.UpdateCustomerSupport(updateModel);
            return Ok(response);
        }

        [HttpDelete("DeleteCustomerSupportAsync")]
        public async Task<IActionResult> DeleteCustomerSupport([FromForm] string cusId)
        {
            var response = await _customerSupportService.DeleteCustomerSupport(cusId);
            return Ok(response);
        }

        [HttpGet("GetAllCustomerSupportAsync")]
        public async Task<IActionResult> GetAllCustomerSupport()
        {
            var response = await _customerSupportService.GetAllCustomerSupport();
            return Ok(response);
        }

        [HttpGet("GetCustomerSupportByIdAsync/{cusId}")]
        public async Task<IActionResult> GetCustomerSupportById(string cusId)
        {
            var response = await _customerSupportService.GetCustomerSupportById(cusId);
            return Ok(response);
        }

        [HttpGet("GetCustomerSupportByUserIdAsync/{userId}")]
        public async Task<IActionResult> GetCustomerSupportByUserId(string userId)
        {
            var response = await _customerSupportService.GetCustomerSupportByUserId(userId);
            return Ok(response);
        }

        [HttpGet("GetCustomerSupportByStatusAsync")]
        public async Task<IActionResult> GetCustomerSupportByStatus()
        {
            var response = await _customerSupportService.GetCustomerSupportByStatus();
            return Ok(response);
        }
    }
}
