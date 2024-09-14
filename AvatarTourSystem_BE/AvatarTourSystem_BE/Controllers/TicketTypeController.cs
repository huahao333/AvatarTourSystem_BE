using BusinessObjects.ViewModels.PackageTour;
using BusinessObjects.ViewModels.TicketType;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Services.Services;

namespace AvatarTourSystem_BE.Controllers
{
    [Route("api/tickettypes")]
    [ApiController]
    public class TicketTypeController : Controller
    {
        public readonly ITicketTypeService _ticketTypeService;

        public TicketTypeController(ITicketTypeService ticketTypeService)
        {
            _ticketTypeService = ticketTypeService;
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetListActiveTicketTypesAsync()
        {
            var result = await _ticketTypeService.GetActiveTicketTypesAsync();
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetListTicketTypesAsync()
        {
            var result = await _ticketTypeService.GetTicketTypesAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTicketTypeById(string id)
        {
            var result = await _ticketTypeService.GetTicketTypeByIdAsync(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTicketTypeAsync([FromForm] TicketTypeCreateModel createModel)
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

        [HttpPut]
        public async Task<IActionResult> UpdateTicketTypeAsync([FromForm] TicketTypeUpdateModel updateModel)
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
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTicketType(string id)
        {
            var result = await _ticketTypeService.DeleteTicketType(id);
            return Ok(result);
        }
    }
}
