using BusinessObjects.ViewModels.Supplier;
using BusinessObjects.ViewModels.TourSegment;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Services.Services;

namespace AvatarTourSystem_BE.Controllers
{
    [Route("api/tour-segments")]
    [ApiController]
    public class TourSegmentController : Controller
    {
        private readonly ITourSegmentService _ourSegmentService;

        public TourSegmentController(ITourSegmentService ourSegmentService)
        {
            _ourSegmentService = ourSegmentService;
        }

        [HttpGet("GetActiveTourSegmentsAsync")]
        public async Task<IActionResult> GetListActiveTourSegmentsAsync()
        {
            var result = await _ourSegmentService.GetActiveTourSegmentsAsync();
            return Ok(result);
        }

        [HttpGet("GetAllTourSegmentsAsync")]
        public async Task<IActionResult> GetListTourSegmentsAsync()
        {
            var result = await _ourSegmentService.GetTourSegmentsAsync();
            return Ok(result);
        }

        [HttpGet("GetTourSegmentByIdAsync/{id}")]
        public async Task<IActionResult> GetTourSegmentByIdAsync(string id)
        {
            var result = await _ourSegmentService.GetTourSegmentByIdAsync(id);
            return Ok(result);
        }

        [HttpPost("CreateTourSegmentAsync")]
        public async Task<IActionResult> CreateTourSegmentAsync([FromForm] TourSegmentCreateModel createModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _ourSegmentService.CreateTourSegmentAsync(createModel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("UpdateTourSegmentAsync")]
        public async Task<IActionResult> UpdateTourSegmentAsync([FromForm] TourSegmentUpdateModel updateModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _ourSegmentService.UpdateTourSegmentAsync(updateModel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeleteTourSegmentAsync/{id}")]
        public async Task<IActionResult> DeleteTourSegmentAsync(string id)
        {
            var result = await _ourSegmentService.DeleteTourSegment(id);
            return Ok(result);
        }
    }
}
