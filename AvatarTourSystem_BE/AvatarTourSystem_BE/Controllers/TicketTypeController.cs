using BusinessObjects.ViewModels.PackageTour;
using BusinessObjects.ViewModels.TicketType;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Services.Services;

namespace AvatarTourSystem_BE.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class TicketTypeController : Controller
    {
        private readonly ITicketTypeService _ticketTypeService;

        public TicketTypeController(ITicketTypeService ticketTypeService)
        {
            _ticketTypeService = ticketTypeService;
        }
        [HttpGet("tickets-type-active")]
        public async Task<IActionResult> GetListActiveTicketTypesAsync()
        {
            var result = await _ticketTypeService.GetActiveTicketTypesAsync();
            return Ok(result);
        }

        [HttpGet("tickets-type")]
        public async Task<IActionResult> GetListTicketTypesAsync()
        {
            var result = await _ticketTypeService.GetTicketTypesAsync();
            return Ok(result);
        }

        [HttpGet("ticket-type/{id}")]
        public async Task<IActionResult> GetTicketTypeByIdAsync(string id)
        {
            var result = await _ticketTypeService.GetTicketTypeByIdAsync(id);
            return Ok(result);
        }

        [HttpPost("ticket-type")]
        public async Task<IActionResult> CreateTicketTypeAsync(TicketTypeCreateModel createModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _ticketTypeService.CreateTicketTypeAsync(createModel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("ticket-type")]
        public async Task<IActionResult> UpdateTicketTypeAsync(TicketTypeUpdateModel updateModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _ticketTypeService.UpdateTicketTypeAsync(updateModel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("ticket-type/{id}")]
        public async Task<IActionResult> DeleteTicketTypeAsync(string id)
        {
            var result = await _ticketTypeService.DeleteTicketType(id);
            return Ok(result);
        }
    }
}
