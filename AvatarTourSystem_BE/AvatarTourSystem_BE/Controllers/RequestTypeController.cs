using BusinessObjects.ViewModels.ResquestType;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace AvatarTourSystem_BE.Controllers
{
    [Route("api/request-type")]
    [ApiController]
    public class RequestTypeController : ControllerBase
    {
        private readonly IRequestTypeService _requestTypeService;
        public RequestTypeController(IRequestTypeService requestTypeService)
        {
            _requestTypeService = requestTypeService;
        }

        [HttpPost("CreateRequestTypeAsync")]
        public async Task<IActionResult> CreateRequestType([FromBody] RequestTypeCreateModel requestTypeCreateModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _requestTypeService.CreateRequestType(requestTypeCreateModel);
            return Ok(result);
        }

        [HttpDelete("DeleteRequestTypeAsync/{id}")]
        public async Task<IActionResult> DeleteRequestTypeAsync(string id)
        {
            var result = await _requestTypeService.DeleteRequestType(id);
            return Ok(result);
        }

        [HttpGet("GetAllRequestTypesAsync")]
        public async Task<IActionResult> GetAllRequestTypes()
        {
            var response = await _requestTypeService.GetAllRequestType();
            return Ok(response);
        }

        [HttpGet("GetRequestTypeByIdAsync/{requestTypeId}")]
        public async Task<IActionResult> GetRequestTypeById(string requestTypeId)
        {
            var response = await _requestTypeService.GetRequestTypeById(requestTypeId);
            return Ok(response);
        }

        [HttpGet("GetRequestTypesByStatusAsync")]
        public async Task<IActionResult> GetRequestTypesByStatus()
        {
            var response = await _requestTypeService.GetRequestTypeByStatus();
            return Ok(response);
        }

        [HttpPut("UpdateRequestTypeAsync")]
        public async Task<IActionResult> UpdateRequestType([FromBody] RequestTypeUpdateModel updateModel)
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
