using BusinessObjects.ViewModels.TransactionHistory;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace AvatarTourSystem_BE.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class TransactionsHistoryController : ControllerBase
    {
        private readonly ITransactionsHistoryService _transactionsHistoryService;   

        public TransactionsHistoryController(ITransactionsHistoryService transactionsHistoryService)
        {
            _transactionsHistoryService = transactionsHistoryService;  
        }

        [HttpGet("transactions-history")]
        public async Task<IActionResult> GetAllTransactionsHistory()
        {
            var result = await _transactionsHistoryService.GetAllTransactionsHistory();
            return Ok(result);
        }
        [HttpGet("transactions-history-active")]
        public async Task<IActionResult> GetTransactionsHistoryByStatusAsync()
        {
            var result = await _transactionsHistoryService.GetTransactionsHistoryByStatus();
            return Ok(result);
        }

        [HttpGet("transaction-history/{id}")]
        public async Task<IActionResult> GetTransactionsHistoryByIdAsync(string id)
        {
            var result = await _transactionsHistoryService.GetTransactionsHistoryById(id);
            return Ok(result);
        }

        [HttpGet("transaction-history-user/{userId}")]
        public async Task<IActionResult> GetTransactionsHistoryByUserIdAsync(string userId)
        {
            var result = await _transactionsHistoryService.GetTransactionsHistoryByUserId(userId);
            return Ok(result);
        }

        [HttpGet("transaction-history-booking/{bookingId}")]
        public async Task<IActionResult> GetTransactionsHistoryByBookingIdAsync(string bookingId)
        {
            var result = await _transactionsHistoryService.GetTransactionsHistoryByBookingId(bookingId);
            return Ok(result);
        }

        [HttpPost("transaction-history")]
        public async Task<IActionResult> CreateTransactionsHistoryAsync([FromForm] TransactionHistoryCreateModel createModel)
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

        [HttpPut("transaction-history")]
        public async Task<IActionResult> UpdateTransactionsHistoryAsync([FromForm] TransactionHistoryUpdateModel updateModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _transactionsHistoryService.UpdateTransactionsHistory(updateModel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("transaction-history/{id}")]
        public async Task<IActionResult> DeleteTransactionsHistoryAsync(string id)
        {
            var result = await _transactionsHistoryService.DeleteTransactionsHistory(id);
            return Ok(result);
        }
    }
}
