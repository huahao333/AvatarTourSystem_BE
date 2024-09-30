using BusinessObjects.ViewModels.Location;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
namespace AvatarTourSystem_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly ILocationService _locationService;

        public LocationController(ILocationService locationService)
        {
            _locationService = locationService;
        }

        [HttpGet("GetActiveLocationsAsync")]
        public async Task<IActionResult> GetActiveLocationsAsync()
        {
            var result = await _locationService.GetActiveLocationsAsync();
            return Ok(result);
        }

        [HttpGet("GetAllLocationsAsync")]
        public async Task<IActionResult> GetAllLocationsAsync()
        {
            var result = await _locationService.GetLocationsAsync();
            return Ok(result);
        }

        [HttpGet("GetLocationByIdAsync/{id}")]
        public async Task<IActionResult> GetLocationByIdAsync(string id)
        {
            var result = await _locationService.GetLocationByIdAsync(id);
            return Ok(result);
        }

        [HttpPost("CreateLocationAsync")]
        public async Task<IActionResult> CreateLocationAsync([FromForm] LocationCreateModel createModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _locationService.CreateLocationAsync(createModel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("UpdateLocationAsync")]
        public async Task<IActionResult> UpdateLocationAsync([FromForm] LocationUpdateModel updateModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _locationService.UpdateLocationAsync(updateModel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeleteLocationAsync/{id}")]
        public async Task<IActionResult> DeleteLocationAsync(string id)
        {
            var result = await _locationService.DeleteLocation(id);
            return Ok(result);
        }
    }
}
