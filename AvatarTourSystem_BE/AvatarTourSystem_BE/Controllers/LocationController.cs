using BusinessObjects.ViewModels.Location;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
namespace AvatarTourSystem_BE.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly ILocationService _locationService;

        public LocationController(ILocationService locationService)
        {
            _locationService = locationService;
        }

        [HttpGet("locations-active")]
        public async Task<IActionResult> GetActiveLocationsAsync()
        {
            var result = await _locationService.GetActiveLocationsAsync();
            return Ok(result);
        }

        [HttpGet("locations")]
        public async Task<IActionResult> GetAllLocationsAsync()
        {
            var result = await _locationService.GetLocationsAsync();
            return Ok(result);
        }

        [HttpGet("location/{id}")]
        public async Task<IActionResult> GetLocationByIdAsync(string id)
        {
            var result = await _locationService.GetLocationByIdAsync(id);
            return Ok(result);
        }

        [HttpPost("location")]
        public async Task<IActionResult> CreateLocationAsync(LocationCreateModel createModel)
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

        [HttpPut("location")]
        public async Task<IActionResult> UpdateLocationAsync(LocationUpdateModel updateModel)
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

        [HttpDelete("location/{id}")]
        public async Task<IActionResult> DeleteLocationAsync(string id)
        {
            var result = await _locationService.DeleteLocation(id);
            return Ok(result);
        }


        [HttpPost("locations")]
        public async Task<IActionResult> UpdateLocation(LocationUpdateViewModel locationUpdateViewModel)
        {
            var result = await _locationService.UpdateLocation(locationUpdateViewModel);
            return Ok(result);
        }
    }
}
