using BusinessObjects.ViewModels.PackageTour;
using BusinessObjects.ViewModels.PackageTourFlow;
using BusinessObjects.ViewModels.PackageTourFlow.PackageTourUpdate;
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
        [HttpPut("CreatePartsPackageTourFlowAsync")]
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
        [HttpGet("GetPackageTourByIdFlowAsync")]
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
        [HttpPut("AddPartToPackageTour")]
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
        [HttpPut("UpdateTour")]
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
