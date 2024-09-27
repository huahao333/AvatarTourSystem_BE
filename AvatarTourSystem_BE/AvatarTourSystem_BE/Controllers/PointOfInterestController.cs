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
        [HttpGet]
        public async Task<IActionResult> GetPointOfInterests()
        {
            var result = await _pointOfInterestService.GetAllPointOfInterests();
            return Ok(result);
        }
        [HttpGet("active")]
        public async Task<IActionResult> GetActivePointOfInterests()
        {
            var result = await _pointOfInterestService.GetPointOfInterestsByStatus();
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPointOfInterestById(string id)
        {
            var result = await _pointOfInterestService.GetPointOfInterestById(id);
            return Ok(result);
        }
        [HttpPost]
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
        [HttpPut]
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
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePointOfInterest(string id)
        {
            var result = await _pointOfInterestService.DeletePointOfInterest(id);
            return Ok(result);
        }
    }
}
