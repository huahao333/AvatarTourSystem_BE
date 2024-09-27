using BusinessObjects.ViewModels.PackageTour;
using BusinessObjects.ViewModels.TourSegment;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Services.Services;

namespace AvatarTourSystem_BE.Controllers
{
    [Route("api/packagetours")]
    [ApiController]
    public class PackageTourController : Controller
    {
        private readonly IPackageTourService _packageTourService;

        public PackageTourController(IPackageTourService packageTourService)
        {
            _packageTourService = packageTourService;
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetListActivePackageToursAsync()
        {
            var result = await _packageTourService.GetActivePackageToursAsync();
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetListPackageToursAsync()
        {
            var result = await _packageTourService.GetPackageToursAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPackageTourById(string id)
        {
            var result = await _packageTourService.GetPackageTourByIdAsync(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePackageTourAsync([FromForm] PackageTourCreateModel createModel)
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

        [HttpPut]
        public async Task<IActionResult> UpdatePackageTourAsync([FromForm] PackageTourUpdateModel updateModel)
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
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePackageTour(string id)
        {
            var result = await _packageTourService.DeletePackageTour(id);
            return Ok(result);
        }
    }
}
