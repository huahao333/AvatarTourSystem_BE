using BusinessObjects.ViewModels.City;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace AvatarTourSystem_BE.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class CityController : Controller
    {
        private readonly ICityService _CityService;

        public CityController(ICityService CityService)
        {
            _CityService = CityService;
        }

        [HttpPost("callback-test")]
        public async Task<IActionResult> GetCallbackCityAsync([FromBody] object data)
        {
            var result = await _CityService.GetCallbackCityAsync(data);
            return Ok(result);
        }

        [HttpGet("cities-active")]
        public async Task<IActionResult> GetListActiveCitiesAsync()
        {
            var result = await _CityService.GetActiveCitiesAsync();
            return Ok(result);
        }


        [HttpGet("cities-zalo-mini-app")]
        public async Task<IActionResult> GetCitiesForUser()
        {
            var result = await _CityService.GetCitiesForUser();
            return Ok(result);
        }

        [HttpGet("cities")]
        public async Task<IActionResult> GetListCitiesAsync()
        {
            var result = await _CityService.GetCitiesAsync();
            return Ok(result);
        }

        [HttpGet("city/{id}")]
        public async Task<IActionResult> GetCityById(string id)
        {
            var result = await _CityService.GetCityByIdAsync(id);
            return Ok(result);
        }

        [HttpPost("city")]
        public async Task<IActionResult> CreateCityAsync( CityCreateModel createModel)
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

        [HttpPut("city")]
        public async Task<IActionResult> UpdateCityAsync(CityUpdateModel updateModel)
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

        [HttpDelete("city/{id}")]
        public async Task<IActionResult> DeleteCity(string id)
        {
            var result = await _CityService.DeleteCity(id);
            return Ok(result);
        }
    }
}
