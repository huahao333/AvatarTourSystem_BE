using BusinessObjects.ViewModels.Service;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace AvatarTourSystem_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly IServiceService _serviceService;

        public ServiceController(IServiceService serviceService)
        {
            _serviceService = serviceService;
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetListActiveServicesAsync()
        {
            var result = await _serviceService.GetActiveServicesAsync();
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetListServicesAsync()
        {
            var result = await _serviceService.GetServicesAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetServiceById(string id)
        {
            var result = await _serviceService.GetServiceByIdAsync(id);
            return Ok(result);
        }

        [HttpPost]
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

        [HttpPut]
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
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteService(string id)
        {
            var result = await _serviceService.DeleteService(id);
            return Ok(result);
        }
    }
}
