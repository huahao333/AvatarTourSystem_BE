using BusinessObjects.ViewModels.POI;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace AvatarTourSystem_BE.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class PointOfInterestController : ControllerBase 
    {
        private readonly IPointOfInterestService _pointOfInterestService;
        public PointOfInterestController(IPointOfInterestService pointOfInterestService)
        {
            _pointOfInterestService = pointOfInterestService;
        }
        [HttpGet("point-of-interests")]
        public async Task<IActionResult> GetAllPointOfInterests()
        {
            var result = await _pointOfInterestService.GetAllPointOfInterests();
            return Ok(result);
        }

        [HttpGet("point-of-interests-active")]
        public async Task<IActionResult> GetActivePointOfInterests()
        {
            var result = await _pointOfInterestService.GetPointOfInterestsByStatus();
            return Ok(result);
        }

        [HttpGet("point-of-interest/{id}")]
        public async Task<IActionResult> GetPointOfInterestByIdAsync(string id)
        {
            var result = await _pointOfInterestService.GetPointOfInterestById(id);
            return Ok(result);
        }

        [HttpPost("point-of-interest")]
        public async Task<IActionResult> CreatePointOfInterest(POICreateModel createModel)
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

        [HttpPut("point-of-interest")]
        public async Task<IActionResult> UpdatePointOfInterest(POIUpdateModel updateModel)
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

        [HttpDelete("point-of-interest/{id}")]
        public async Task<IActionResult> DeletePointOfInterest(string id)
        {
            var result = await _pointOfInterestService.DeletePointOfInterest(id);
            return Ok(result);
        }

        [HttpPost("point-of-interest-location")]
        public async Task<IActionResult> CreatePointOfInterestByLocation(POICreateByLocationViewModel pOICreateByLocation)
        {
            var result = await _pointOfInterestService.CreatePointOfInterestByLocation(pOICreateByLocation);
            return Ok(result);
        }
    }
}
