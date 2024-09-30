using BusinessObjects.ViewModels.Destination;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace AvatarTourSystem_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DestinationController : Controller
    {
        private readonly IDestinationService _DestinationService;

        public DestinationController(IDestinationService DestinationService)
        {
            _DestinationService = DestinationService;
        }
        [HttpGet("GetActiveDestinationsAsync")]
        public async Task<IActionResult> GetActiveDestinationsAsync()
        {
            var result = await _DestinationService.GetActiveDestinationsAsync();
            return Ok(result);
        }

        [HttpGet("GetAllDestinationsAsync")]
        public async Task<IActionResult> GetAllDestinationsAsync()
        {
            var result = await _DestinationService.GetDestinationsAsync();
            return Ok(result);
        }

        [HttpGet("GetDestinationByIdAsync/{id}")]
        public async Task<IActionResult> GetDestinationByIdAsync(string id)
        {
            var result = await _DestinationService.GetDestinationByIdAsync(id);
            return Ok(result);
        }

        [HttpPost("CreateDestinationAsync")]
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

        [HttpPut("UpdateDestinationAsync")]
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

        [HttpDelete("DeleteDestinationAsync/{id}")]
        public async Task<IActionResult> DeleteDestinationAsync(string id)
        {
            var result = await _DestinationService.DeleteDestination(id);
            return Ok(result);
        }
    }
}
