using BusinessObjects.ViewModels.PackageTour;
using BusinessObjects.ViewModels.PackageTourFlow;
using BusinessObjects.ViewModels.PackageTourFlow.PackageTourUpdate;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace AvatarTourSystem_BE.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class PackageTourFlowController : ControllerBase
    {
        private readonly IPackageTourFlowService _packageTourFlow;
        public PackageTourFlowController(IPackageTourFlowService packageTourFlow)
        {
            _packageTourFlow = packageTourFlow;
        }
        [HttpPost("package-tour-flows")]
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
        [HttpPut("parts-package-tour-flow")]
        public async Task<IActionResult> CreatePartsPackageTourFlowAsync([FromBody] FPackageTourUpdate packageTourFlowModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _packageTourFlow.CreatePartsPackageTourFlowAsync(packageTourFlowModel);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result);
            }
        }
        [HttpGet("package-tour-flow/{id}")]
        public async Task<IActionResult> GetPackageTourByIdFlowAsync(string id)
        {
            var result = await _packageTourFlow.GetPackageTourFlowByIdAsync(id);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result);
            }
        }
        [HttpPut("package-tours")]
        public async Task<IActionResult> AddPartToPackageTour([FromBody] FPackageTourUpdateModel packageTourFlowModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _packageTourFlow.AddPartToPackageTourFlow(packageTourFlowModel);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result);
            }
        }
        [HttpPut("tour-flow")]
        public async Task<IActionResult> UpdateTour([FromBody] FPackageTourUpdateModel packageTourFlowModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _packageTourFlow.UpdatePackageTourFlowAsync(packageTourFlowModel);
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
