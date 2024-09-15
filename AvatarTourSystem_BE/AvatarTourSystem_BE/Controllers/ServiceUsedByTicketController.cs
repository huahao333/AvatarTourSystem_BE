using BusinessObjects.Models;
using BusinessObjects.ViewModels.DailyTicket;
using BusinessObjects.ViewModels.ServiceUsedByTicket;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace AvatarTourSystem_BE.Controllers
{
    [Route("api/serviceusedbyticket")]
    [ApiController]
    public class ServiceUsedByTicketController : Controller
    {
        private readonly IServiceUsedByTicketService _serviceUsedByTicketService;

        public ServiceUsedByTicketController(IServiceUsedByTicketService serviceUsedByTicketService)
        {
            _serviceUsedByTicketService = serviceUsedByTicketService;
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetListActiveServiceUsedByTicketsAsync()
        {
            var result = await _serviceUsedByTicketService.GetActiveServiceUsedByTicketsAsync();
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetListServiceUsedByTicketsAsync()
        {
            var result = await _serviceUsedByTicketService.GetServiceUsedByTicketsAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetServiceUsedByTicketById(string id)
        {
            var result = await _serviceUsedByTicketService.GetServiceUsedByTicketByIdAsync(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateServiceUsedByTicketAsync([FromForm] ServiceUsedByTicketCreateModel createModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _serviceUsedByTicketService.CreateServiceUsedByTicketAsync(createModel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateServiceUsedByTicketAsync([FromForm] ServiceUsedByTicketUpdateModel updateModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {

                var result = await _serviceUsedByTicketService.UpdateServiceUsedByTicketAsync(updateModel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteServiceUsedByTicket(string id)
        {
            var result = await _serviceUsedByTicketService.DeleteServiceUsedByTicket(id);
            return Ok(result);
        }
    }
}
