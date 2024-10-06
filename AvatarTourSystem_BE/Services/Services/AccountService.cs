using AutoMapper;
using BusinessObjects.Enums;
using BusinessObjects.Models;
using BusinessObjects.ViewModels.Account;
using BusinessObjects.ViewModels.Feedback;
using BusinessObjects.ViewModels.Rate;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Mvc;
using Repositories.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using Services.Common;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;

namespace Services.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<Account> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<Account> _signInManager;
        private readonly IConfiguration _configuration;
        public AccountService(IUnitOfWork unitOfWork,
                              IMapper mapper,
                              UserManager<Account> userManager,
                              RoleManager<IdentityRole> roleManager,
                              SignInManager<Account> signInManager,
                              IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        public async Task<APIResponseModel> CreateAccount(AccountCreateModel createModel)
        {
            var account = new Account
            {
                Id = Guid.NewGuid().ToString(),
                UserName = createModel.UserName,
                Email = createModel.Email,
                FullName = createModel.FullName,
                Gender = createModel.Gender,
                Dob = createModel.Dob,
                Address = createModel.Address,
                PhoneNumber = createModel.PhoneNumber,
                AvatarUrl = createModel.AvatarUrl,
                ZaloUser = createModel.ZaloUser,
                CreateDate = DateTime.Now,
                Status = (int)createModel.Status // Example status (Active)
            };

            // Save to the database
            var result = await _unitOfWork.AccountRepository.AddAsync(account);
            _unitOfWork.Save();
            var accountModel = _mapper.Map<AccountModel>(result);

            // Return response
            return new APIResponseModel
            {
                Message = "Account Created Successfully",
                IsSuccess = true,
                Data = accountModel
            };
        }

        public async Task<APIResponseModel> DeleteAccount(string accountId)
        {
            var account = await _unitOfWork.AccountRepository.GetByIdStringAsync(accountId);
            if (account == null)
            {
                return new APIResponseModel
                {
                    Message = "Account not found.",
                    IsSuccess = false,
                    Data = null
                };
            }
            if (account.Status == (int?)EStatus.IsDeleted)
            {
                return new APIResponseModel
                {
                    Message = "Account has already been deleted.",
                    IsSuccess = false,
                    Data = null
                };
            }
            var createDate = account.CreateDate;
            account.Status = (int?)EStatus.IsDeleted;
            account.UpdateDate = DateTime.Now;
            account.CreateDate = createDate;
            await _unitOfWork.AccountRepository.UpdateAsync(account);
            _unitOfWork.Save();
            return new APIResponseModel
            {
                Message = "Account Deleted Successfully",
                IsSuccess = true,
                Data = account
            };
        }

        public async Task<APIResponseModel> GetAccountById(string accountId)
        {
            var account = await _unitOfWork.AccountRepository.GetByIdStringAsync(accountId);
            if (account == null)
            {
                return new APIResponseModel
                {
                    Message = "Account not found.",
                    IsSuccess = false,
                    Data = null
                };
            }
           
                var accountViewModels = _mapper.Map<List<AccountViewModel>>(account);

                return new APIResponseModel
                {
                    Message = "Account found",
                    IsSuccess = true,
                    Data = accountViewModels
                };

            


        }

        public async Task<APIResponseModel> GetAccountByStatus()
        {
            var accounts = await _unitOfWork.AccountRepository.GetByConditionAsync(s => s.Status != -1);
            if (accounts == null || !accounts.Any())
            {
                return new APIResponseModel
                {
                    Message = "Account not found.",
                    IsSuccess = false,
                    Data = null
                };
            }
            var accountViewModels = _mapper.Map<List<AccountViewModel>>(accounts);

            return new APIResponseModel
            {
                Message = "Get Account by Status Successfully",
                IsSuccess = true,
                Data = accountViewModels
            };

        }

        public async Task<APIResponseModel> GetAllAccount()
        {
            // Fetch all accounts from the repository
            var accounts = await _unitOfWork.AccountRepository.GetAllAsync();

            // Check if accounts exist (optional)
            if (accounts == null || !accounts.Any())
            {
                return new APIResponseModel
                {
                    Message = "No accounts found.",
                    IsSuccess = false,
                    Data = null
                };
            }

            // Map accounts to AccountViewModel
            var accountViewModels = _mapper.Map<List<AccountViewModel>>(accounts);

            // Return success response with mapped view models
            return new APIResponseModel
            {
                Message = "Get All Account Successfully",
                IsSuccess = true,
                Data = accountViewModels // Use the mapped view models here
            };
        }

        public async Task<APIResponseModel> UpdateAccount(AccountUpdateModel updateModel)
        {
            var existingAccount = await _unitOfWork.AccountRepository.GetByIdGuidAsync(updateModel.Id);
            if (existingAccount == null)
            {
                return new APIResponseModel
                {
                    Message = "Account not found.",
                    IsSuccess = false,
                    Data = null
                };
            }
            var createdDate = existingAccount.CreateDate;

            _mapper.Map(updateModel, existingAccount);

            existingAccount.UpdateDate = DateTime.Now;
            existingAccount.CreateDate = createdDate;
            var result = await _unitOfWork.AccountRepository.UpdateAsync(existingAccount);
            _unitOfWork.Save();
            var accountModel = _mapper.Map<AccountModel>(result);

            return new APIResponseModel
            {
                Message = "Account Updated Successfully",
                IsSuccess = true,
                Data = accountModel
            };

        }

        public async Task<APIResponseModel> SignUpAccountAsync(AccountSignUpModel signUpModel)
        {
            try
            {
                if (string.IsNullOrEmpty(signUpModel.AccountEmail) || string.IsNullOrWhiteSpace(signUpModel.AccountEmail))
                {
                    return new APIResponseModel { IsSuccess = false, Message = "Email cannot be empty or whitespace." };
                }

                var exsistAccount = await _userManager.FindByNameAsync(signUpModel.AccountEmail);
                if (exsistAccount == null)
                {
                    var user = new Account
                    {
                        FullName = signUpModel.FullName,
                        Dob = signUpModel.BirthDate,
                        Gender = signUpModel.Gender,
                        Address = signUpModel.Address,
                        UserName = signUpModel.AccountEmail,
                        Email = signUpModel.AccountEmail,
                        PhoneNumber = signUpModel.AccountPhone,
                        CreateDate = DateTime.Now,
                        Status = 1,
                    };

                    var result = await _userManager.CreateAsync(user, signUpModel.AccountPassword);

                    string errorMessage = "Failed to register. Please check your input and try again.";

                    if (result.Succeeded)
                    {
                        if (!await _roleManager.RoleExistsAsync(ERole.Admin.ToString()))
                        {
                            await _roleManager.CreateAsync(new IdentityRole(ERole.Admin.ToString()));
                        }
                        if (await _roleManager.RoleExistsAsync(ERole.Admin.ToString()))
                        {
                            await _userManager.AddToRoleAsync(user, ERole.Admin.ToString());
                        }

                        return new APIResponseModel { IsSuccess = true, Message = "Registration successful." };
                    }
                    foreach (var ex in result.Errors)
                    {
                        errorMessage = ex.Description;
                    }
                    return new APIResponseModel { IsSuccess = false, Message = errorMessage };
                }
                return new APIResponseModel { IsSuccess = false, Message = "Account already exists" };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return new APIResponseModel { IsSuccess = false, Message = "An error occurred while checking if the account exists." };
            }
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }


        public async Task<APIAuthenticationResponseModel> SignInAccountAsync(AccountSignInModel signInModel)
        {
            var account = await _userManager.FindByNameAsync(signInModel.AccountEmail);

            if (account == null)
            {
                return new APIAuthenticationResponseModel
                {
                    Status = false,
                    Message = "Invalid login attempt. Please check your email and password."
                };
            }
            var result = await _signInManager.PasswordSignInAsync(signInModel.AccountEmail, signInModel.AccountPassword, false, false);

            if (result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(signInModel.AccountEmail);
                if (user != null)
                {
                    var authClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, signInModel.AccountEmail),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                    };

                    var userRole = await _userManager.GetRolesAsync(user);
                    foreach (var role in userRole)
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role, role.ToString()));
                    }

                    var authenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]));

                    var token = new JwtSecurityToken(
                        issuer: _configuration["JWT:ValidIssuer"],
                        audience: _configuration["JWT:ValidAudience"],
                        expires: DateTime.Now.AddHours(2),
                        claims: authClaims,
                        signingCredentials: new SigningCredentials(authenKey, SecurityAlgorithms.HmacSha256)
                    );

                    var refreshToken = GenerateRefreshToken();

                    _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInDays"], out int refreshTokenValidityInDays);

                    account.RefreshToken = refreshToken;
                    account.RefreshTokenExpiryTime = DateTime.Now.AddDays(refreshTokenValidityInDays);

                    await _userManager.UpdateAsync(account);

                    return new APIAuthenticationResponseModel
                    {
                        Status = true,
                        Message = "Login successfully!",
                        JwtToken = new JwtSecurityTokenHandler().WriteToken(token),
                        //  Expired = token.ValidTo,
                        //  JwtRefreshToken = refreshToken,
                    };
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return new APIAuthenticationResponseModel
                {
                    Status = false,
                    Message = "Invalid login attempt. Please check your email and password."
                };
            }
        }

        public async Task<APIResponseModel> SignUpAccountZaloAsync(AccountZaloIdModel accountZaloIdModel)
        {
            try
            {
                var accountZaloExisting = (await _unitOfWork.AccountRepository.GetByConditionAsync(s => s.ZaloUser.ToString() == accountZaloIdModel.ZaloUser))
                                          .FirstOrDefault();
                if (accountZaloExisting != null)
                {
                    var accessToken = await GenerateAccessTokenForAccount(accountZaloExisting);

                    return new APIResponseModel
                    {
                        IsSuccess = true,
                        Message = "Zalo account already exists. Returning access token.",
                        Data = new { AccessToken = accessToken }
                    };
                }
                else
                {
                    var user = new Account
                    {
                        FullName = "",
                        Dob = null,
                        Gender = true,
                        Address = "",
                        UserName = accountZaloIdModel.ZaloUser,
                        Email = "",
                        PhoneNumber = "",
                        CreateDate = DateTime.Now,
                        Status = 1,
                        ZaloUser = accountZaloIdModel.ZaloUser,
                    };

                    var result = await _userManager.CreateAsync(user);

                    string errorMessage = "Failed to register with Zalo. Please check your input and try again.";

                    if (result.Succeeded)
                    {

                        if (!await _roleManager.RoleExistsAsync(ERole.Customer.ToString()))
                        {
                            await _roleManager.CreateAsync(new IdentityRole(ERole.Customer.ToString()));
                        }
                        if (await _roleManager.RoleExistsAsync(ERole.Customer.ToString()))
                        {
                            await _userManager.AddToRoleAsync(user, ERole.Customer.ToString());
                        }


                        var accessToken = await GenerateAccessTokenForAccount(user);

                        return new APIResponseModel
                        {
                            IsSuccess = true,
                            Message = "Registration successful with Zalo.",
                            Data = new { AccessToken = accessToken }
                        };
                    }

                    foreach (var ex in result.Errors)
                    {
                        errorMessage = ex.Description;
                    }
                    return new APIResponseModel { IsSuccess = false, Message = errorMessage };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return new APIResponseModel { IsSuccess = false, Message = "An error occurred while checking if the account exists." };
            }
        }

        private async Task<string> GenerateAccessTokenForAccount(Account account)
        {
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JWT:SecretKey"]));

            var claims = new List<Claim>
                   {
                    new Claim(JwtRegisteredClaimNames.Sub, account.UserName),
                    new Claim(JwtRegisteredClaimNames.Email, account.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Aud, _configuration["JWT:ValidAudience"]),
                    new Claim(JwtRegisteredClaimNames.Iss, _configuration["JWT:ValidIssuer"])
                  };

            var roles = await _userManager.GetRolesAsync(account);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature) // Đặt khóa và thuật toán mã hóa
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public async Task<APIResponseModel> GetAccountByZaloID(string zaloId)
        {
            var account = await _unitOfWork.AccountRepository.GetByIdStringAsync(zaloId);
            if (account == null)
            {
                return new APIResponseModel
                {
                    Message = "Account not found.",
                    IsSuccess = false,
                    Data = null
                };
            }
            
                var accountViewModels = _mapper.Map<List<AccountViewModel>>(account);

                return new APIResponseModel
                {
                    Message = "Account found",
                    IsSuccess = true,
                    Data = accountViewModels
                };
            
        }
    }
}
