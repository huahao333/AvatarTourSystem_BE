using BusinessObjects.ViewModels.POIType;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace AvatarTourSystem_BE.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class POITypeController : ControllerBase
    {
        private readonly IPOITypeService _poiTypeService;
        public POITypeController(IPOITypeService poiTypeService)
        {
            _poiTypeService = poiTypeService;
        }
        [HttpGet("poi-types")]
        public async Task<IActionResult> GetAllPOITypes()
        {
            var result = await _poiTypeService.GetAllPOITypes();
            return Ok(result);
        }

        [HttpGet("poi-types-active")]
        public async Task<IActionResult> GetActivePOITypes()
        {
            var result = await _poiTypeService.GetPOITypesByStatus();
            return Ok(result);
        }

        [HttpGet("poi-type/{id}")]
        public async Task<IActionResult> GetPOITypeByIdAsync(string id)
        {
            var result = await _poiTypeService.GetPOITypeById(id);
            return Ok(result);
        }

        [HttpPost("poi-type")]
        public async Task<IActionResult> CreatePOIType([FromForm] POITypeCreateModel createModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _poiTypeService.CreatePOIType(createModel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("poi-type")]
        public async Task<IActionResult> UpdatePOIType([FromForm] POITypeUpdateModel updateModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _poiTypeService.UpdatePOIType(updateModel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("poi-type/{id}")]
        public async Task<IActionResult> DeletePOIType(string id)
        {
            var result = await _poiTypeService.DeletePOIType(id);
            return Ok(result);
        }
    }
}
