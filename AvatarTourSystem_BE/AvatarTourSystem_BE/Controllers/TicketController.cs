using BusinessObjects.ViewModels.Ticket;
using BusinessObjects.ViewModels.TicketType;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace AvatarTourSystem_BE.Controllers
{
    [Route("api/tickets")]
    [ApiController]
    public class TicketController : Controller
    {
        private readonly ITicketService _ticketService;

        public TicketController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }


        [HttpGet("GetActiveTicketsAsync")]
        public async Task<IActionResult> GetListActiveTicketsAsync()
        {
            var result = await _ticketService.GetActiveTicketsAsync();
            return Ok(result);
        }

        [HttpGet("GetAllTicketsAsync")]
        public async Task<IActionResult> GetListTicketsAsync()
        {
            var result = await _ticketService.GetTicketsAsync();
            return Ok(result);
        }

        [HttpGet("GetTicketByIdAsync/{id}")]
        public async Task<IActionResult> GetTicketByIdAsync(string id)
        {
            var result = await _ticketService.GetTicketByIdAsync(id);
            return Ok(result);
        }

        [HttpPost("CreateTicketAsync")]
        public async Task<IActionResult> CreateTicketAsync([FromForm] TicketCreateModel createModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _ticketService.CreateTicketAsync(createModel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("UpdateTicketAsync")]
        public async Task<IActionResult> UpdateTicketAsync([FromForm] TicketUpdateModel updateModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _ticketService.UpdateTicketAsync(updateModel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeleteTicketAsync/{id}")]
        public async Task<IActionResult> DeleteTicketAsync(string id)
        {
            var result = await _ticketService.DeleteTicket(id);
            return Ok(result);
        }
    }
}
