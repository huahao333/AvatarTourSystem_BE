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

        [HttpGet("active")]
        public async Task<IActionResult> GetListActiveLocationsAsync()
        {
            var result = await _locationService.GetActiveLocationsAsync();
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetListLocationsAsync()
        {
            var result = await _locationService.GetLocationsAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetLocationById(string id)
        {
            var result = await _locationService.GetLocationByIdAsync(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreatelocationAsync([FromForm] LocationCreateModel createModel)
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

        [HttpPut]
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
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletelocation(string id)
        {
            var result = await _locationService.DeleteLocation(id);
            return Ok(result);
        }
    }
}
