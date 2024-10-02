using BusinessObjects.ViewModels.Service;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace AvatarTourSystem_BE.Controllers
{
    [Route("api/service")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly IServiceService _serviceService;

        public ServiceController(IServiceService serviceService)
        {
            _serviceService = serviceService;
        }
        [HttpGet("GetActiveServicesAsync")]
        public async Task<IActionResult> GetListActiveServicesAsync()
        {
            var result = await _serviceService.GetActiveServicesAsync();
            return Ok(result);
        }

        [HttpGet("GetAllServicesAsync")]
        public async Task<IActionResult> GetListServicesAsync()
        {
            var result = await _serviceService.GetServicesAsync();
            return Ok(result);
        }

        [HttpGet("GetServiceByIdAsync/{id}")]
        public async Task<IActionResult> GetServiceByIdAsync(string id)
        {
            var result = await _serviceService.GetServiceByIdAsync(id);
            return Ok(result);
        }

        [HttpPost("CreateServiceAsync")]
        public async Task<IActionResult> CreateServiceAsync([FromForm] ServiceCreateModel createModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _serviceService.CreateServiceAsync(createModel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("UpdateServiceAsync")]
        public async Task<IActionResult> UpdateServiceAsync([FromForm] ServiceUpdateModel updateModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _serviceService.UpdateServiceAsync(updateModel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeleteServiceAsync/{id}")]
        public async Task<IActionResult> DeleteServiceAsync(string id)
        {
            var result = await _serviceService.DeleteService(id);
            return Ok(result);
        }
    }
}
