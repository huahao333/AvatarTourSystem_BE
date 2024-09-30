using BusinessObjects.ViewModels.POIType;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace AvatarTourSystem_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class POITypeController : ControllerBase
    {
        private readonly IPOITypeService _poiTypeService;
        public POITypeController(IPOITypeService poiTypeService)
        {
            _poiTypeService = poiTypeService;
        }
        [HttpGet("GetAllPOITypesAsync")]
        public async Task<IActionResult> GetAllPOITypes()
        {
            var result = await _poiTypeService.GetAllPOITypes();
            return Ok(result);
        }

        [HttpGet("GetActivePOITypesAsync")]
        public async Task<IActionResult> GetActivePOITypes()
        {
            var result = await _poiTypeService.GetPOITypesByStatus();
            return Ok(result);
        }

        [HttpGet("GetPOITypeByIdAsync/{id}")]
        public async Task<IActionResult> GetPOITypeByIdAsync(string id)
        {
            var result = await _poiTypeService.GetPOITypeById(id);
            return Ok(result);
        }

        [HttpPost("CreatePOITypeAsync")]
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

        [HttpPut("UpdatePOITypeAsync")]
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

        [HttpDelete("DeletePOITypeAsync/{id}")]
        public async Task<IActionResult> DeletePOIType(string id)
        {
            var result = await _poiTypeService.DeletePOIType(id);
            return Ok(result);
        }
    }
}
