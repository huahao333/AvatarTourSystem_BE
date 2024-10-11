using BusinessObjects.ViewModels.ServiceByTourSegment;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace AvatarTourSystem_BE.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class ServiceByTourSegmentController : ControllerBase
    {
        private readonly IServiceByTourSegmentService _serviceByTourSegmentService;

        public ServiceByTourSegmentController(IServiceByTourSegmentService serviceByTourSegmentService)
        {
            _serviceByTourSegmentService = serviceByTourSegmentService;
        }
        [HttpGet("service-toursegments-active")]
        public async Task<IActionResult> GetListActiveServiceByTourSegmentsAsync()
        {
            var result = await _serviceByTourSegmentService.GetActiveServiceByTourSegmentsAsync();
            return Ok(result);
        }

        [HttpGet("service-toursegments")]
        public async Task<IActionResult> GetListServiceByTourSegmentsAsync()
        {
            var result = await _serviceByTourSegmentService.GetServiceByTourSegmentsAsync();
            return Ok(result);
        }

        [HttpGet("service-toursegment/{id}")]
        public async Task<IActionResult> GetServiceByTourSegmentByIdAsync(string id)
        {
            var result = await _serviceByTourSegmentService.GetServiceByTourSegmentByIdAsync(id);
            return Ok(result);
        }

        [HttpPost("service-toursegment")]
        public async Task<IActionResult> CreateServiceByTourSegmentAsync(ServiceByTourSegmentCreateModel createModel)
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

        [HttpPut("service-toursegment")]
        public async Task<IActionResult> UpdateServiceByTourSegmentAsync(ServiceByTourSegmentUpdateModel updateModel)
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

        [HttpDelete("service-toursegment/{id}")]
        public async Task<IActionResult> DeleteServiceByTourSegmentAsync(string id)
        {
            var result = await _serviceByTourSegmentService.DeleteServiceByTourSegment(id);
            return Ok(result);
        }
    }
}
