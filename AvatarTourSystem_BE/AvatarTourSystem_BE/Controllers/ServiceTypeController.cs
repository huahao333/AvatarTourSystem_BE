using BusinessObjects.ViewModels.Service;
using BusinessObjects.ViewModels.ServiceType;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace AvatarTourSystem_BE.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class ServiceTypeController : ControllerBase
    {
        private readonly IServiceTypeService _serviceTypeService;

        public ServiceTypeController(IServiceTypeService serviceTypeService)
        {
            _serviceTypeService = serviceTypeService;
        }

        [HttpGet("services-type-active")]
        public async Task<IActionResult> GetListActiveServiceTypesAsync()
        {
            var result = await _serviceTypeService.GetActiveServiceTypesAsync();
            return Ok(result);
        }

        [HttpGet("services-type")]
        public async Task<IActionResult> GetListServiceTypesAsync()
        {
            var result = await _serviceTypeService.GetServiceTypesAsync();
            return Ok(result);
        }

        [HttpGet("service-type/{id}")]
        public async Task<IActionResult> GetServiceTypeByIdAsync(string id)
        {
            var result = await _serviceTypeService.GetServiceTypeByIdAsync(id);
            return Ok(result);
        }

        [HttpPost("service-type")]
        public async Task<IActionResult> CreateServiceTypeAsync([FromForm] ServiceTypeCreateModel createModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _serviceTypeService.CreateServiceTypeAsync(createModel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("service-type")]
        public async Task<IActionResult> UpdateServiceTypeAsync([FromForm] ServiceTypeUpdateModel updateModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _serviceTypeService.UpdateServiceTypeAsync(updateModel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("service-type/{id}")]
        public async Task<IActionResult> DeleteServiceTypeAsync(string id)
        {
            var result = await _serviceTypeService.DeleteServiceType(id);
            return Ok(result);
        }
    }
}
