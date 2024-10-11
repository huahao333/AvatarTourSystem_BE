using BusinessObjects.ViewModels.PackageTour;
using BusinessObjects.ViewModels.TourSegment;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Services.Services;

namespace AvatarTourSystem_BE.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class PackageTourController : Controller
    {
        private readonly IPackageTourService _packageTourService;

        public PackageTourController(IPackageTourService packageTourService)
        {
            _packageTourService = packageTourService;
        }

        [HttpGet("package-tours-active")]
        public async Task<IActionResult> GetActivePackageToursAsync()
        {
            var result = await _packageTourService.GetActivePackageToursAsync();
            return Ok(result);
        }

        [HttpGet("package-tours")]
        public async Task<IActionResult> GetAllPackageToursAsync()
        {
            var result = await _packageTourService.GetPackageToursAsync();
            return Ok(result);
        }

        [HttpGet("package-tour/{id}")]
        public async Task<IActionResult> GetPackageTourByIdAsync(string id)
        {
            var result = await _packageTourService.GetPackageTourByIdAsync(id);
            return Ok(result);
        }

        [HttpPost("package-tour")]
        public async Task<IActionResult> CreatePackageTourAsync(PackageTourCreateModel createModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _packageTourService.CreatePackageTourAsync(createModel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("package-tour")]
        public async Task<IActionResult> UpdatePackageTourAsync(PackageTourUpdateModel updateModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _packageTourService.UpdatePackageTourAsync(updateModel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("package-tour/{id}")]
        public async Task<IActionResult> DeletePackageTour(string id)
        {
            var result = await _packageTourService.DeletePackageTour(id);
            return Ok(result);
        }
    }
}
