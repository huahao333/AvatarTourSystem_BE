using BusinessObjects.ViewModels.City;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace AvatarTourSystem_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : Controller
    {
        private readonly ICityService _CityService;

        public CityController(ICityService CityService)
        {
            _CityService = CityService;
        }

        [HttpGet("GetActiveCitiesAsync")]
        public async Task<IActionResult> GetListActiveCitiesAsync()
        {
            var result = await _CityService.GetActiveCitiesAsync();
            return Ok(result);
        }

        [HttpGet("GetAllCitiesAsync")]
        public async Task<IActionResult> GetListCitiesAsync()
        {
            var result = await _CityService.GetCitiesAsync();
            return Ok(result);
        }

        [HttpGet("GetCityByIdAsync/{id}")]
        public async Task<IActionResult> GetCityById(string id)
        {
            var result = await _CityService.GetCityByIdAsync(id);
            return Ok(result);
        }

        [HttpPost("CreateCityAsync")]
        public async Task<IActionResult> CreateCityAsync([FromForm] CityCreateModel createModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _CityService.CreateCityAsync(createModel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("UpdateCityAsync")]
        public async Task<IActionResult> UpdateCityAsync([FromForm] CityUpdateModel updateModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _CityService.UpdateCityAsync(updateModel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeleteCityAsync/{id}")]
        public async Task<IActionResult> DeleteCity(string id)
        {
            var result = await _CityService.DeleteCity(id);
            return Ok(result);
        }
    }
}
