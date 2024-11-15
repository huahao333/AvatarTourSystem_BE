using BusinessObjects.ViewModels.PackageTour;
using BusinessObjects.ViewModels.PackageTourFlow;
using BusinessObjects.ViewModels.PackageTourFlow.PackageTourGet;
using BusinessObjects.ViewModels.PackageTourFlow.PackageTourUpdate;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
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
                return StatusCode(StatusCodes.Status400BadRequest, result);
            }
        }
        [Authorize]
        [HttpGet("get-all-package-tour")]
        public async Task<IActionResult> GetPackageTourFlowAsync()
        {
            var result = await _packageTourFlow.GetPackageTourFlowAsync();
            return Ok(result);
        }
        [Authorize]
        [HttpGet("get-package-tour/{id}")]
        public async Task<IActionResult> GetPackageTourByIdFlowAsync(string id)
        {
            var result = await _packageTourFlow.GetPackageTourFlowByIdAsync(id);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            else
            {
                return StatusCode(StatusCodes.Status404NotFound, result);
            }
        }
        [Authorize]
        [HttpPut("create-detail-package-tours")]
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
                return StatusCode(StatusCodes.Status400BadRequest, result);
            }
        }
        [Authorize]
        [HttpPut("update-packge-tour")]
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
                return StatusCode(StatusCodes.Status400BadRequest, result);
            }
        }
        //[Authorize]
        [HttpPost("get-destination-by-city")]
        public async Task<IActionResult> GetDestinationByCityIdFlowAsync(GetDestinationByCityModel cityId)
        {
            var result = await _packageTourFlow.GetDestinationByCityIdAsync(cityId);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            else
            {
                return StatusCode(StatusCodes.Status404NotFound, result);
            }
        }
        //[Authorize]
        [HttpPost("get-location-by-destination")]
        public async Task<IActionResult> GetLocationsByDestinationIdFlowAsync(GetLocationByDestinationModel destinationId)
        {
            var result = await _packageTourFlow.GetLocationsByDestinationIdAsync(destinationId);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            else
            {
                return StatusCode(StatusCodes.Status404NotFound, result);
            }
        }
        //[Authorize]
        [HttpPost("get-service-by-location")]
        public async Task<IActionResult> GetServicesByLocationIdFlowAsync(GetServiceByLocationModel locationId)
        {
            var result = await _packageTourFlow.GetServicesByLocationIdAsync(locationId);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            else
            {
                return StatusCode(StatusCodes.Status404NotFound, result);
            }
        }

    }
}
