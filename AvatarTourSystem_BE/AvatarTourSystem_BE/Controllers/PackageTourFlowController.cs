using BusinessObjects.ViewModels.PackageTour;
using BusinessObjects.ViewModels.PackageTourFlow;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace AvatarTourSystem_BE.Controllers
{
    [Route("api/package-tour-flow")]
    [ApiController]
    public class PackageTourFlowController : ControllerBase
    {
        private readonly IPackageTourFlowService _packageTourFlow;
        public PackageTourFlowController(IPackageTourFlowService packageTourFlow)
        {
            _packageTourFlow = packageTourFlow;
        }
        [HttpPost("CreatePackageTourFlowAsync")]
        public async Task<IActionResult> CreatePackageTourFlowAsync([FromBody] FPackageTourCreateModel packageTourFlowModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _packageTourFlow.CreatePackageTourFlowAsync(packageTourFlowModel);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result);
            }
        }
    }
}
