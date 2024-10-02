using BusinessObjects.ViewModels.DailyTicket;
using BusinessObjects.ViewModels.TicketType;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace AvatarTourSystem_BE.Controllers
{
    [Route("api/daily-tickets")]
    [ApiController]
    public class DailyTicketController : Controller
    {
        private readonly IDailyTicketService _dailyTicketService;

        public DailyTicketController(IDailyTicketService dailyTicketService)
        {
            _dailyTicketService = dailyTicketService;
        }

        [HttpGet("GetActiveDailyTicketsAsync")]
        public async Task<IActionResult> GetListActiveDailyTicketsAsync()
        {
            var result = await _dailyTicketService.GetActiveDailyTicketsAsync();
            return Ok(result);
        }

        [HttpGet("GetAllDailyTicketsAsync")]
        public async Task<IActionResult> GetListDailyTicketsAsync()
        {
            var result = await _dailyTicketService.GetDailyTicketsAsync();
            return Ok(result);
        }

        [HttpGet("GetDailyTicketByIdAsync/{id}")]
        public async Task<IActionResult> GetDailyTicketById(string id)
        {
            var result = await _dailyTicketService.GetDailyTicketByIdAsync(id);
            return Ok(result);
        }

        [HttpPost("CreateDailyTicketAsync")]
        public async Task<IActionResult> CreateDailyTicketAsync([FromForm] DailyTicketCreateModel createModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _dailyTicketService.CreateDailyTicketAsync(createModel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("UpdateDailyTicketAsync")]
        public async Task<IActionResult> UpdateDailyTicketAsync([FromForm] DailyTicketUpdateModel updateModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _dailyTicketService.UpdateDailyTicketAsync(updateModel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeleteDailyTicketAsync/{id}")]
        public async Task<IActionResult> DeleteDailyTicket(string id)
        {
            var result = await _dailyTicketService.DeleteDailyTicket(id);
            return Ok(result);
        }
    }
}
