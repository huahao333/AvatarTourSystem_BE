using BusinessObjects.ViewModels.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Services.Interfaces;

namespace AvatarTourSystem_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }


       // [Authorize(Roles = "Admin")]
        [HttpGet("GetAllAccountAsync")]
        public async Task<IActionResult> GetAllAccount()
        {
            var response = await _accountService.GetAllAccount();
            return Ok(response);
        }

        [HttpGet("GetAccountByStatusAsync")]
        public async Task<IActionResult> GetAccountByStatus()
        {
            var response = await _accountService.GetAccountByStatus();
            return Ok(response);
        }

        [HttpGet("GetAccountById/{accountId}")]
        public async Task<IActionResult> GetAccountById(string accountId)
        {
            var response = await _accountService.GetAccountById(accountId);
            return Ok(response);
        }

        [HttpPost("CreateAccountAsync")]
        public async Task<IActionResult> CreateAccount(AccountCreateModel createModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _accountService.CreateAccount(createModel);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result);
            }
        }

        [HttpPut("UpdateAccoutAsync")]
        public async Task<IActionResult> UpdateAccount(AccountUpdateModel updateModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _accountService.UpdateAccount(updateModel);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result);
            }
        }

        [HttpDelete("DeleteAccount/{accountId}")]
        public async Task<IActionResult> DeleteAccount(string accountId)
        {
            var response = await _accountService.DeleteAccount(accountId);
            return Ok(response);
        }

        [HttpPost("SignUpAccountAsync")]
        public async Task<IActionResult> SignUp(AccountSignUpModel signUpModel)
        {
            var response = await _accountService.SignUpAccountAsync(signUpModel);
            return Ok(response);
        }

        [HttpPost("SignInAccountAsync")]
        public async Task<IActionResult> SignIn(AccountSignInModel signInModel)
        {
            var response = await _accountService.SignInAccountAsync(signInModel);
            return Ok(response);
        }

        [HttpPost("SignUpAccountZaloAsync")]
        public async Task<IActionResult> SignUpZalo(string zaloId )
        {
            var response = await _accountService.SignUpAccountZaloAsync(zaloId);
            return Ok(response);
        }
    }
}
