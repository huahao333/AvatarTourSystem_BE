using BusinessObjects.ViewModels.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Services.Interfaces;

namespace AvatarTourSystem_BE.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }


       // [Authorize(Roles = "Admin")]
        [HttpGet("accounts")]
        public async Task<IActionResult> GetAllAccount()
        {
            var response = await _accountService.GetAllAccount();
            return Ok(response);
        }

        [HttpGet("accounts-active")]
        public async Task<IActionResult> GetAccountByStatus()
        {
            var response = await _accountService.GetAccountByStatus();
            return Ok(response);
        }

        [HttpGet("account/{id}")]
        public async Task<IActionResult> GetAccountById(string id)
        {
            var response = await _accountService.GetAccountById(id);
            if(response.IsSuccess)
            {
                return Ok(response);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
           
        }
        [HttpGet("account-zalo/{zaloId}")]
        public async Task<IActionResult> GetAccountByZaloID(string zaloId)
        {
            var response = await _accountService.GetAccountByZaloID(zaloId);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }

        }

        [HttpPost("account")]
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

        [HttpPut("account")]
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

        [HttpDelete("account/{id}")]
        public async Task<IActionResult> DeleteAccount(string id)
        {
            var response = await _accountService.DeleteAccount(id);
            return Ok(response);
        }

        [HttpPost("account-sign-up")]
        public async Task<IActionResult> SignUp(AccountSignUpModel signUpModel)
        {
            var response = await _accountService.SignUpAccountAsync(signUpModel);
            return Ok(response);
        }

        [HttpPost("account-sign-in")]
        public async Task<IActionResult> SignIn(AccountSignInModel signInModel)
        {
            var response = await _accountService.SignInAccountAsync(signInModel);
            return Ok(response);
        }

        [HttpPost("login-zalo")]
        public async Task<IActionResult> SignUpZalo(AccountZaloIdModel accountZaloIdModel )
        {
            var response = await _accountService.SignUpAccountZaloAsync(accountZaloIdModel);
            return Ok(response);
        }

        [HttpPost("phonenumber-zalo")]
        public async Task<IActionResult> UpdatePhoneNumberByZaloID(AccountZaloCURLModel accountZaloCURLModel)
        {
            var response = await _accountService.GetPhoneInfoAndSaveAsync(accountZaloCURLModel);
            return Ok(response);
        }
    }
}
