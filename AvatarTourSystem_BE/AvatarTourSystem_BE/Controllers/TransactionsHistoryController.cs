using BusinessObjects.ViewModels.TransactionHistory;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace AvatarTourSystem_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsHistoryController : ControllerBase
    {
        private readonly ITransactionsHistoryService _transactionsHistoryService;   

        public TransactionsHistoryController(ITransactionsHistoryService transactionsHistoryService)
        {
            _transactionsHistoryService = transactionsHistoryService;  
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTransactionsHistory()
        {
            var result = await _transactionsHistoryService.GetAllTransactionsHistory();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTransactionsHistoryById(string id)
        {
            var result = await _transactionsHistoryService.GetTransactionsHistoryById(id);
            return Ok(result);
        }

        [HttpGet("GetTransactionsHistoryByUserId/{userId}")]
        public async Task<IActionResult> GetTransactionsHistoryByUserId(string userId)
        {
            var result = await _transactionsHistoryService.GetTransactionsHistoryByUserId(userId);
            return Ok(result);
        }

        [HttpGet("GetTransactionsHistoryByBookingId/{bookingId}")]
        public async Task<IActionResult> GetTransactionsHistoryByBookingId(string bookingId)
        {
            var result = await _transactionsHistoryService.GetTransactionsHistoryByBookingId(bookingId);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTransactionsHistory([FromForm] TransactionHistoryCreateModel createModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _transactionsHistoryService.CreateTransactionsHistory(createModel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTransactionsHistory(string id, [FromForm] TransactionHistoryUpdateModel updateModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _transactionsHistoryService.UpdateTransactionsHistory(id, updateModel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransactionsHistory(string id)
        {
            var result = await _transactionsHistoryService.DeleteTransactionsHistory(id);
            return Ok(result);
        }
    }
}
