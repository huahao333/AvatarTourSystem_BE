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

        [HttpGet("active")]
        public async Task<IActionResult> GetListActiveServiceByTourSegmentsAsync()
        {
            var result = await _serviceByTourSegmentService.GetActiveServiceByTourSegmentsAsync();
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetListServiceByTourSegmentsAsync()
        {
            var result = await _serviceByTourSegmentService.GetServiceByTourSegmentsAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetServiceByTourSegmentById(string id)
        {
            var result = await _serviceByTourSegmentService.GetServiceByTourSegmentByIdAsync(id);
            return Ok(result);
        }

        [HttpPost]
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

        [HttpPut]
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
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteServiceByTourSegment(string id)
        {
            var result = await _serviceByTourSegmentService.DeleteServiceByTourSegment(id);
            return Ok(result);
        }
    }
}
