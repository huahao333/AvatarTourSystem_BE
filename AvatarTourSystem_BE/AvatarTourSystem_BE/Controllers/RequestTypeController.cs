using BusinessObjects.ViewModels.ResquestType;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace AvatarTourSystem_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestTypeController : ControllerBase
    {
        private readonly IRequestTypeService _requestTypeService;
        public RequestTypeController(IRequestTypeService requestTypeService)
        {
            _requestTypeService = requestTypeService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateRequestType(RequestTypeCreateModel requestTypeCreateModel)
        {
            var result = await _requestTypeService.CreateRequestType(requestTypeCreateModel);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRequestType(string id)
        {
            var result = await _requestTypeService.DeleteRequestType(id);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllRequestType()
        {
            var response = await _requestTypeService.GetAllRequestType();
            return Ok(response);
        }
        [HttpGet("GetRequestTypeById")]
        public async Task<IActionResult> GetRequestTypeById(string requestTypeId)
        {
            var response = await _requestTypeService.GetRequestTypeById(requestTypeId);
            return Ok(response);
        }
        [HttpGet("GetRequestTypeByStatus")]
        public async Task<IActionResult> GetRequestTypeByStatus()
        {
            var response = await _requestTypeService.GetRequestTypeByStatus();
            return Ok(response);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateRequestType(RequestTypeUpdateModel updateModel)
        {
            var result = await _requestTypeService.UpdateRequestType(updateModel);
            return Ok(result);
        }


    }
}
