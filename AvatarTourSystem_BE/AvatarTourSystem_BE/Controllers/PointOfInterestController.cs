using BusinessObjects.ViewModels.POI;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace AvatarTourSystem_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PointOfInterestController : ControllerBase 
    {
        private readonly IPointOfInterestService _pointOfInterestService;
        public PointOfInterestController(IPointOfInterestService pointOfInterestService)
        {
            _pointOfInterestService = pointOfInterestService;
        }
        [HttpGet("GetAllPointOfInterestsAsync")]
        public async Task<IActionResult> GetAllPointOfInterests()
        {
            var result = await _pointOfInterestService.GetAllPointOfInterests();
            return Ok(result);
        }

        [HttpGet("GetActivePointOfInterestsAsync")]
        public async Task<IActionResult> GetActivePointOfInterests()
        {
            var result = await _pointOfInterestService.GetPointOfInterestsByStatus();
            return Ok(result);
        }

        [HttpGet("GetPointOfInterestByIdAsync/{id}")]
        public async Task<IActionResult> GetPointOfInterestByIdAsync(string id)
        {
            var result = await _pointOfInterestService.GetPointOfInterestById(id);
            return Ok(result);
        }

        [HttpPost("CreatePointOfInterestAsync")]
        public async Task<IActionResult> CreatePointOfInterest([FromForm] POICreateModel createModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _pointOfInterestService.CreatePointOfInterest(createModel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("UpdatePointOfInterestAsync")]
        public async Task<IActionResult> UpdatePointOfInterest([FromForm] POIUpdateModel updateModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _pointOfInterestService.UpdatePointOfInterest(updateModel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeletePointOfInterestAsync/{id}")]
        public async Task<IActionResult> DeletePointOfInterest(string id)
        {
            var result = await _pointOfInterestService.DeletePointOfInterest(id);
            return Ok(result);
        }
    }
}
