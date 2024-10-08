using BusinessObjects.ViewModels.ResquestType;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace AvatarTourSystem_BE.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class RequestTypeController : ControllerBase
    {
        private readonly IRequestTypeService _requestTypeService;
        public RequestTypeController(IRequestTypeService requestTypeService)
        {
            _requestTypeService = requestTypeService;
        }

        [HttpPost("request-type")]
        public async Task<IActionResult> CreateRequestType( RequestTypeCreateModel requestTypeCreateModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _requestTypeService.CreateRequestType(requestTypeCreateModel);
            return Ok(result);
        }

        [HttpDelete("request-type/{id}")]
        public async Task<IActionResult> DeleteRequestTypeAsync(string id)
        {
            var result = await _requestTypeService.DeleteRequestType(id);
            return Ok(result);
        }

        [HttpGet("requests-type")]
        public async Task<IActionResult> GetAllRequestTypes()
        {
            var response = await _requestTypeService.GetAllRequestType();
            return Ok(response);
        }

        [HttpGet("request-type/{id}")]
        public async Task<IActionResult> GetRequestTypeById(string id)
        {
            var response = await _requestTypeService.GetRequestTypeById(id);
            return Ok(response);
        }

        [HttpGet("requests-type-active")]
        public async Task<IActionResult> GetRequestTypesByStatus()
        {
            var response = await _requestTypeService.GetRequestTypeByStatus();
            return Ok(response);
        }

        [HttpPut("request-type")]
        public async Task<IActionResult> UpdateRequestType( RequestTypeUpdateModel updateModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _requestTypeService.UpdateRequestType(updateModel);
            return Ok(result);
        }
    }
}
