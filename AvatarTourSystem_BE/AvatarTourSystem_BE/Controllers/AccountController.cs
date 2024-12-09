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

        [HttpPost("accounts-role")]
        public async Task<IActionResult> GetAllAccountByRole(AccountInforByRole accountInforByRole)
        {
            var response = await _accountService.GetAllAccountByRole(accountInforByRole);
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
                return StatusCode(StatusCodes.Status404NotFound, response);
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
                return StatusCode(StatusCodes.Status404NotFound, response);
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
                return StatusCode(StatusCodes.Status404NotFound, result);
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
                return StatusCode(StatusCodes.Status404NotFound, result);
            }
        }
        [HttpPut("account-zalo")]
        public async Task<IActionResult> UpdateAccountWithZaloId(AccountUpdateWithZaloIdModel updateModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _accountService.UpdateAccountWithZaloId(updateModel);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            else
            {
                return StatusCode(StatusCodes.Status404NotFound, result);
            }
        }

        [HttpDelete("account/{id}")]
        public async Task<IActionResult> DeleteAccount(string id)
        {
            var response = await _accountService.DeleteAccount(id);
            return Ok(response);
        }

        [HttpPost("account-admin-sign-up")]
        public async Task<IActionResult> SignUp(AccountSignUpModel signUpModel)
        {
            var response = await _accountService.SignUpAccountAsync(signUpModel);
            return Ok(response);
        }
        [HttpPost("account-manager-sign-up")]
        public async Task<IActionResult> SignUpManager(AccountSignUpModel signUpModel)
        {
            var response = await _accountService.SignUpManagerAccountAsync(signUpModel);
            return Ok(response);
        }
        [HttpPost("account-staff-sign-up")]
        public async Task<IActionResult> SignUpStaff(AccountSignUpModel signUpModel)
        {
            var response = await _accountService.SignUpStaffAccountAsync(signUpModel);
            return Ok(response);
        }
        [HttpPost("account-supplier-sign-up")]
        public async Task<IActionResult> SignUpSupplier(AccountSignUpModel signUpModel)
        {
            var response = await _accountService.SignUpSupplierAccountAsync(signUpModel);
            return Ok(response);
        }
        [HttpPost("account-super-admin-sign-up")]
        public async Task<IActionResult> SignUpSuperAdminAccountAsync(AccountSignUpModel signUpModel)
        {
            var response = await _accountService.SignUpSuperAdminAccountAsync(signUpModel);
            return Ok(response);
        }
        [HttpPost("update-role")]
        public async Task<IActionResult> ChangeAccountRoleAsync(AccountChangeRoleViewModel accountChangeRoleViewModel)
        {
            var response = await _accountService.ChangeAccountRoleAsync(accountChangeRoleViewModel);
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

        [HttpPost("phonenumber-token")]
        public async Task<IActionResult> UpdatePhoneNumberByZaloID(AccountZaloCURLModel accountZaloCURLModel)
        {
            var response = await _accountService.GetPhoneInfoAndSaveAsync(accountZaloCURLModel);
            return Ok(response);
        }

        [HttpPost("phonenumber-zalo")]
        public async Task<IActionResult> AccountUpdatePhoneNumberByZaloID(AccountUpdatePhoneWithZaloIdModel updatePhoneWithZaloIdModel)
        {
            var response = await _accountService.UpdatePhoneNumberByZaloId(updatePhoneWithZaloIdModel);
            return Ok(response);
        }

        [HttpPost("change-profile")]
        public async Task<IActionResult> UpdateProfile(UpdateProfile updateProfile)
        {
            var response = await _accountService.UpdateProfile(updateProfile);
            return Ok(response);
        }

        [HttpPost("block-account")]
        public async Task<IActionResult> BlockAndUnblockAccount(UpdateStatusViewModel updateStatusViewModel)
        {
            var response = await _accountService.BlockAndUnblockAccount(updateStatusViewModel);
            return Ok(response);
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPasswordAsync(AccountInforByRole accountInforByRole)
        {
            var response = await _accountService.ResetPasswordAsync(accountInforByRole);
            return Ok(response);
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePasswordAsync(ChangePasswordModel changePasswordModel)
        {
            var response = await _accountService.ChangePasswordAsync(changePasswordModel);
            return Ok(response);
        }
    }
}
