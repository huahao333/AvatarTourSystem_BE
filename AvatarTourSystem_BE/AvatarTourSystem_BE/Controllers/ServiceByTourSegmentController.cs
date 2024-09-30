using BusinessObjects.ViewModels.ServiceByTourSegment;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace AvatarTourSystem_BE.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ServiceByTourSegmentController : ControllerBase
    {
        private readonly IServiceByTourSegmentService _serviceByTourSegmentService;

        public ServiceByTourSegmentController(IServiceByTourSegmentService serviceByTourSegmentService)
        {
            _serviceByTourSegmentService = serviceByTourSegmentService;
        }
        [HttpGet("GetActiveServicesAsync")]
        public async Task<IActionResult> GetListActiveServiceByTourSegmentsAsync()
        {
            var result = await _serviceByTourSegmentService.GetActiveServiceByTourSegmentsAsync();
            return Ok(result);
        }

        [HttpGet("GetAllServicesAsync")]
        public async Task<IActionResult> GetListServiceByTourSegmentsAsync()
        {
            var result = await _serviceByTourSegmentService.GetServiceByTourSegmentsAsync();
            return Ok(result);
        }

        [HttpGet("GetServiceByIdAsync/{id}")]
        public async Task<IActionResult> GetServiceByTourSegmentByIdAsync(string id)
        {
            var result = await _serviceByTourSegmentService.GetServiceByTourSegmentByIdAsync(id);
            return Ok(result);
        }

        [HttpPost("CreateServiceAsync")]
        public async Task<IActionResult> CreateServiceByTourSegmentAsync([FromForm] ServiceByTourSegmentCreateModel createModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _serviceByTourSegmentService.CreateServiceByTourSegmentAsync(createModel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("UpdateServiceAsync")]
        public async Task<IActionResult> UpdateServiceByTourSegmentAsync([FromForm] ServiceByTourSegmentUpdateModel updateModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _serviceByTourSegmentService.UpdateServiceByTourSegmentAsync(updateModel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeleteServiceAsync/{id}")]
        public async Task<IActionResult> DeleteServiceByTourSegmentAsync(string id)
        {
            var result = await _serviceByTourSegmentService.DeleteServiceByTourSegment(id);
            return Ok(result);
        }
    }
}
