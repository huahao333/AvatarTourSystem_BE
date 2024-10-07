using BusinessObjects.ViewModels.Destination;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace AvatarTourSystem_BE.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class DestinationController : Controller
    {
        private readonly IDestinationService _DestinationService;

        public DestinationController(IDestinationService DestinationService)
        {
            _DestinationService = DestinationService;
        }
        [HttpGet("destinations-active")]
        public async Task<IActionResult> GetActiveDestinationsAsync()
        {
            var result = await _DestinationService.GetActiveDestinationsAsync();
            return Ok(result);
        }

        [HttpGet("destinations")]
        public async Task<IActionResult> GetAllDestinationsAsync()
        {
            var result = await _DestinationService.GetDestinationsAsync();
            return Ok(result);
        }

        [HttpGet("destination/{id}")]
        public async Task<IActionResult> GetDestinationByIdAsync(string id)
        {
            var result = await _DestinationService.GetDestinationByIdAsync(id);
            return Ok(result);
        }

        [HttpPost("destination")]
        public async Task<IActionResult> CreateDestinationAsync([FromForm] DestinationCreateModel createModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _DestinationService.CreateDestinationAsync(createModel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("destination")]
        public async Task<IActionResult> UpdateDestinationAsync([FromForm] DestinationUpdateModel updateModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _DestinationService.UpdateDestinationAsync(updateModel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("destinations/{id}")]
        public async Task<IActionResult> DeleteDestinationAsync(string id)
        {
            var result = await _DestinationService.DeleteDestination(id);
            return Ok(result);
        }
    }
}
