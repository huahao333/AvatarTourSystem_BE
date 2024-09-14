using BusinessObjects.ViewModels.Destination;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace AvatarTourSystem_BE.Controllers
{
    [Route("api/destinations")]
    [ApiController]
    public class DestinationController : Controller
    {
        private readonly IDestinationService _DestinationService;

        public DestinationController(IDestinationService DestinationService)
        {
            _DestinationService = DestinationService;
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetListActiveDestinationsAsync()
        {
            var result = await _DestinationService.GetActiveDestinationsAsync();
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetListDestinationssAsync()
        {
            var result = await _DestinationService.GetDestinationsAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDestinationById(string id)
        {
            var result = await _DestinationService.GetDestinationByIdAsync(id);
            return Ok(result);
        }

        [HttpPost]
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

        [HttpPut]
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
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDestination(string id)
        {
            var result = await _DestinationService.DeleteDestination(id);
            return Ok(result);
        }
    }
}
