using BusinessObjects.ViewModels.CustomerSupport;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace AvatarTourSystem_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerSupportController : ControllerBase
    {
        private readonly ICustomerSupportService _customerSupportService;
       
        public CustomerSupportController(ICustomerSupportService customerSupportService)
        {
            _customerSupportService = customerSupportService;            
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomerSupport([FromForm] CustomerSupportCreateModel createModel)
        {
            var response = await _customerSupportService.CreateCustomerSupport(createModel);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCustomerSupport([FromForm] CustomerSupportUpdateModel updateModel)
        {
            var response = await _customerSupportService.UpdateCustomerSupport(updateModel);
            return Ok(response);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCustomerSupport([FromForm] string cusId)
        {
            var response = await _customerSupportService.DeleteCustomerSupport(cusId);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCustomerSupport()
        {
            var response = await _customerSupportService.GetAllCustomerSupport();
            return Ok(response);
        }

        [HttpGet("GetCustomerSupportById")]
        public async Task<IActionResult> GetCustomerSupportById(string cusId)
        {
            var response = await _customerSupportService.GetCustomerSupportById(cusId);
            return Ok(response);
        }
        [HttpGet("GetCustomerSpportByUserId")]
        public async Task<IActionResult> GetCustomerSupportByUserId(string userId)
        {
            var response = await _customerSupportService.GetCustomerSupportByUserId(userId);
            return Ok(response);
        }
        [HttpGet("GetCustomerSupportByStatus")]
        public async Task<IActionResult> GetCustomerSupportByStatus()
        {
            var response = await _customerSupportService.GetCustomerSupportByStatus();
            return Ok(response);
        }
    }
}
