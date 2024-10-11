using BusinessObjects.Models;
using BusinessObjects.ViewModels.DailyTicket;
using BusinessObjects.ViewModels.ServiceUsedByTicket;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace AvatarTourSystem_BE.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class ServiceUsedByTicketController : Controller
    {
        private readonly IServiceUsedByTicketService _serviceUsedByTicketService;

        public ServiceUsedByTicketController(IServiceUsedByTicketService serviceUsedByTicketService)
        {
            _serviceUsedByTicketService = serviceUsedByTicketService;
        }

        [HttpGet("service-used-tickets-active")]
        public async Task<IActionResult> GetListActiveServiceUsedByTicketsAsync()
        {
            var result = await _serviceUsedByTicketService.GetActiveServiceUsedByTicketsAsync();
            return Ok(result);
        }

        [HttpGet("service-used-tickets")]
        public async Task<IActionResult> GetListServiceUsedByTicketsAsync()
        {
            var result = await _serviceUsedByTicketService.GetServiceUsedByTicketsAsync();
            return Ok(result);
        }

        [HttpGet("service-used-ticket/{id}")]
        public async Task<IActionResult> GetServiceUsedByTicketByIdAsync(string id)
        {
            var result = await _serviceUsedByTicketService.GetServiceUsedByTicketByIdAsync(id);
            return Ok(result);
        }

        [HttpPost("service-used-ticket")]
        public async Task<IActionResult> CreateServiceUsedByTicketAsync( ServiceUsedByTicketCreateModel createModel)
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

        [HttpPut("service-used-ticket")]
        public async Task<IActionResult> UpdateServiceUsedByTicketAsync( ServiceUsedByTicketUpdateModel updateModel)
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

        [HttpDelete("service-used-ticket/{id}")]
        public async Task<IActionResult> DeleteServiceUsedByTicketAsync(string id)
        {
            var result = await _serviceUsedByTicketService.DeleteServiceUsedByTicket(id);
            return Ok(result);
        }
    }
}
