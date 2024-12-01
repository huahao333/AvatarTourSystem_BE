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
using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;

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
        private readonly HttpClient _httpClient;
        private readonly ZaloServices _zaloServices; 
        public AccountService(IUnitOfWork unitOfWork,
                              IMapper mapper,
                              UserManager<Account> userManager,
                              RoleManager<IdentityRole> roleManager,
                              SignInManager<Account> signInManager,
                              IConfiguration configuration,
                              HttpClient httpClient,
                              ZaloServices zaloServices)

        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _httpClient = httpClient;
            _zaloServices = zaloServices;
        }

        public async Task<APIResponseModel> CreateAccount(AccountCreateModel createModel)
        {
            var account = new Account
            {
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
                Status = (int)createModel.Status 
            };

            var result = await _unitOfWork.AccountRepository.AddAsync(account);
            _unitOfWork.Save();
            var accountModel = _mapper.Map<AccountModel>(result);

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

            var accountViewModel = _mapper.Map<AccountViewModel>(account);

            return new APIResponseModel
                {
                    Message = "Account found",
                    IsSuccess = true,
                    Data = accountViewModel
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
            var accounts = await _unitOfWork.AccountRepository.GetAllAsync();

            if (accounts == null || !accounts.Any())
            {
                return new APIResponseModel
                {
                    Message = "No accounts found.",
                    IsSuccess = false,
                    Data = null
                };
            }

            var accountViewModels = _mapper.Map<List<AccountViewModel>>(accounts);

            return new APIResponseModel
            {
                Message = "Get All Account Successfully",
                IsSuccess = true,
                Data = accountViewModels 
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
                    return new APIResponseModel { IsSuccess = false, Message = "Username cannot be empty or whitespace." };
                }

                var existAccount = await _unitOfWork.AccountRepository.GetByConditionAsync(a => a.Email == signUpModel.AccountEmail);
                if (existAccount.Any())
                {
                    return new APIResponseModel { IsSuccess = false, Message = "Account already exists" };
                }
                
                if (signUpModel.FullName.Length < 3 || signUpModel.FullName.Length > 50)
                {
                    return new APIResponseModel
                    {
                        Message = "FullName must be between 3 and 50 characters.",
                        IsSuccess = false
                    };
                }

                if (signUpModel.BirthDate == default || signUpModel.BirthDate.Date >= DateTime.Now.Date)
                {
                    return new APIResponseModel
                    {
                        Message = "Date of Birth is invalid.",
                        IsSuccess = false
                    };
                }


                if (signUpModel.Address.Length < 5 || signUpModel.Address.Length > 120)
                {
                    return new APIResponseModel
                    {
                        Message = "Address must be between 5 and 120 characters.",
                        IsSuccess = false
                    };
                }

                var user = new Account
                {
                    FullName = signUpModel.FullName,
                    Dob = signUpModel.BirthDate,
                    Gender = signUpModel.Gender,
                    Address = signUpModel.Address,
                    UserName = signUpModel.AccountEmail,
                    Email = signUpModel.AccountEmail,
                    PhoneNumber = signUpModel.AccountPhone,
                    AvatarUrl = "http://res.cloudinary.com/dhovknfgb/image/upload/v1732960399/uzewxkbhnv0xyvahpuj0.jpg",
                    CreateDate = DateTime.Now,
                    Status = 1,
                    Roles= (int)ERole.Admin,
                //    PasswordHash = signUpModel.AccountPassword 
                };

                var passwordHasher = new PasswordHasher<Account>();
                user.PasswordHash = passwordHasher.HashPassword(user, signUpModel.AccountPassword);

                await _unitOfWork.AccountRepository.AddAsync(user);
                _unitOfWork.Save(); 


                return new APIResponseModel { IsSuccess = true, Message = "Registration successful." };
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
            var account = (await _unitOfWork.AccountRepository.GetByConditionAsync(a => a.Email == signInModel.AccountEmail)).FirstOrDefault();

            if (account == null)
            {
                return new APIAuthenticationResponseModel
                {
                    Status = false,
                    Message = "Invalid login attempt. Please check your email and password."
                };
            }

            if (account.Status == -1)
            {
                return new APIAuthenticationResponseModel
                {
                    Status = false,
                    Message = "Account is disabled. Please contact support for assistance."
                };
            }

            var passwordHasher = new PasswordHasher<Account>();
            var passwordVerificationResult = passwordHasher.VerifyHashedPassword(account, account.PasswordHash, signInModel.AccountPassword);

            if (passwordVerificationResult != PasswordVerificationResult.Success) 
            {
                return new APIAuthenticationResponseModel
                {
                    Status = false,
                    Message = "Invalid login attempt. Please check your email and password."
                };
            }

             var authClaims = new List<Claim>
               {
                    new Claim(ClaimTypes.Name, account.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("UserId",account.Id.ToString())
                };

            if (!string.IsNullOrEmpty(account.Roles.ToString()))
            {
                var roleName = ((ERole)account.Roles.Value).ToString();
                authClaims.Add(new Claim(ClaimTypes.Role, roleName));
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
            await _unitOfWork.AccountRepository.UpdateAsync(account); 

            return new APIAuthenticationResponseModel
            {
                Status = true,
                Message = "Login successfully!",
                JwtToken = new JwtSecurityTokenHandler().WriteToken(token),
                // Expired = token.ValidTo, 
                // JwtRefreshToken = refreshToken, 
            };
        }

        public async Task<APIResponseModel> SignUpAccountZaloAsync(AccountZaloIdModel accountZaloIdModel)
        {
            try
            {
                var accountZaloExisting = (await _unitOfWork.AccountRepository.GetByConditionAsync(s => s.ZaloUser.ToString() 
                                                                                             == accountZaloIdModel.ZaloUser))
                                          .FirstOrDefault();
                if (accountZaloExisting != null)
                {
                    var accessToken = await GenerateAccessTokenForAccount(accountZaloExisting);

                    return new APIResponseModel
                    {
                        IsSuccess = true,
                        Message = "Zalo account already exists. Returning access token.",
                        Data = new 
                        { 
                            AccessToken = accessToken ,
                            isFirstLogin = false
                        }
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
                        Email =  "",
                        PhoneNumber = "",
                        AvatarUrl ="",
                        CreateDate = DateTime.Now,
                        Status = 1,
                        ZaloUser = accountZaloIdModel.ZaloUser,
                        Roles = (int) ERole.Customer
                    };

                  //  var result = await _userManager.CreateAsync(user);

                 //   string errorMessage = "Failed to register with Zalo. Please check your input and try again.";

                    await _unitOfWork.AccountRepository.AddAsync(user);
                     _unitOfWork.Save(); 

                    var accessToken = await GenerateAccessTokenForAccount(user);

                    return new APIResponseModel
                    {
                        IsSuccess = true,
                        Message = "Registration successful with Zalo.",
                        Data = new
                        {
                            AccessToken = accessToken,
                            isFirstLogin = true
                        }
                    };
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
                    new Claim(JwtRegisteredClaimNames.Iss, _configuration["JWT:ValidIssuer"]),
                    new Claim(ClaimTypes.Role, ERole.Customer.ToString())
                  };

            //var roles = await _userManager.GetRolesAsync(account);
            //foreach (var role in roles)
            //{
            //    claims.Add(new Claim(ClaimTypes.Role, role));
            //}

            //var tokenDescriptor = new SecurityTokenDescriptor
            //{
            //    Subject = new ClaimsIdentity(claims),
            //    Expires = DateTime.UtcNow.AddHours(1),
            //    SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature) // Đặt khóa và thuật toán mã hóa
            //};

            //var tokenHandler = new JwtSecurityTokenHandler();
            //var token = tokenHandler.CreateToken(tokenDescriptor);

            //return tokenHandler.WriteToken(token);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddHours(1),
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public async Task<APIResponseModel> GetAccountByZaloID(string zaloId)
        {
            var account = await _unitOfWork.AccountRepository.GetByConditionAsync(z => z.ZaloUser == zaloId);
            var accountZalo = account.FirstOrDefault();
            if (account == null || !account.Any())
            {
                return new APIResponseModel
                {
                    Message = "Account not found.",
                    IsSuccess = false,
                    Data = null
                };
            }

            var accountViewModel = new AccountZaloIdViewModel
            {
                ZaloUser = !string.IsNullOrWhiteSpace(accountZalo.ZaloUser) ? accountZalo.ZaloUser : "",
                UserName = !string.IsNullOrWhiteSpace(accountZalo.UserName) ? accountZalo.UserName : "",
                FullName = !string.IsNullOrWhiteSpace(accountZalo.FullName) ? accountZalo.FullName : "",
                AvatarUrl = !string.IsNullOrWhiteSpace(accountZalo.AvatarUrl) ? accountZalo.AvatarUrl : "",
                isHasPhoneNumber = !string.IsNullOrWhiteSpace(accountZalo.PhoneNumber)
            };

            return new APIResponseModel
            {
                Message = "Account found",
                IsSuccess = true,
                Data = accountViewModel
            };

        }

        public async Task<APIResponseModel> GetPhoneInfoAndSaveAsync(AccountZaloCURLModel accountZaloCURLModel)
        {
            var (phoneInfo, accessToken, phoneToken) = await _zaloServices.CallZaloApiAsync(accountZaloCURLModel.AccessToken, 
                                                                                              accountZaloCURLModel.PhoneToken);

            if (phoneInfo == null)
            {
                return new APIResponseModel
                {
                    Message = "Failed to retrieve phone info from Zalo API.",
                    IsSuccess = false,
                    Data = new
                    {
                        AccessToken = accessToken, 
                        PhoneToken = phoneToken    
                    }
                };
            }

            var accountZaloID = (await _unitOfWork.AccountRepository.GetByConditionAsync(z => z.ZaloUser == 
                                                                            accountZaloCURLModel.ZaloId)).FirstOrDefault();
            if (accountZaloID == null || !string.IsNullOrEmpty(accountZaloID.PhoneNumber))
            {
                return new APIResponseModel
                {
                    Message = "Account not found or Account phone existed.",
                    IsSuccess = false,
                    Data = null
                };
            }

            var createdDate = accountZaloID.CreateDate;
            accountZaloID.PhoneNumber = phoneInfo;
            accountZaloID.UpdateDate = DateTime.Now;
            accountZaloID.CreateDate = createdDate;
            await _unitOfWork.AccountRepository.UpdateAsync(accountZaloID);
            _unitOfWork.Save();

            return new APIResponseModel
            {
                Message = "Phone info retrieved and saved successfully.",
                IsSuccess = true,
                Data = phoneInfo
            };
        }
        //private async Task<string> CallZaloApiAsync(string accessToken, string phoneToken)
        //{

        //    string url = "https://graph.zalo.me/v2.0/me/info";

        //    _httpClient.DefaultRequestHeaders.Add("access_token", accessToken);
        //    _httpClient.DefaultRequestHeaders.Add("code", phoneToken);
        //    _httpClient.DefaultRequestHeaders.Add("secret_key", _secretKey);

        //    var response = await _httpClient.GetAsync(url);

        //    if (!response.IsSuccessStatusCode)
        //    {
        //        return null;
        //    }
        //    var responseBody = await response.Content.ReadAsStringAsync();

        //    Console.WriteLine("Response body: " + responseBody);

        //    try
        //    {
        //        var jsonDocument = JsonDocument.Parse(responseBody);
        //        if (jsonDocument.RootElement.TryGetProperty("data", out var dataElement) && dataElement.TryGetProperty("number", out var numberElement))
        //        {
        //            string phoneNumber = numberElement.GetString();
        //            return phoneNumber;
        //        }
        //        else
        //        {
        //            Console.WriteLine("Không tìm thấy các thuộc tính mong đợi trong phản hồi.");
        //            return null; 
        //        }
        //    }
        //    catch (JsonException jsonEx)
        //    {
        //        Console.WriteLine("Lỗi phân tích JSON: " + jsonEx.Message);
        //        return null; 
        //    }
        //}

        public async Task<APIResponseModel> UpdateAccountWithZaloId(AccountUpdateWithZaloIdModel updateModel)
        {
            var existingAccount = await _unitOfWork.AccountRepository.GetByConditionAsync(s => s.ZaloUser == updateModel.ZaloUser);

            if (existingAccount == null || !existingAccount.Any())
            {
                return new APIResponseModel
                {
                    Message = "Account not found.",
                    IsSuccess = false,
                    Data = null
                };
            }

            var accountToUpdate = existingAccount.FirstOrDefault();


            _mapper.Map(updateModel, accountToUpdate);
            accountToUpdate.UpdateDate = DateTime.Now;

            _unitOfWork.Save();

            var accountModel = _mapper.Map<AccountModel>(accountToUpdate);

            return new APIResponseModel
            {
                Message = "Account Updated Successfully",
                IsSuccess = true,
                Data = accountModel
            };

        }

        public async Task<APIResponseModel> UpdatePhoneNumberByZaloId(AccountUpdatePhoneWithZaloIdModel updatePhoneWithZaloIdModel)
        {
            try
            {
                var accountZaloExisting = (await _unitOfWork.AccountRepository.GetByConditionAsync(s => s.ZaloUser.ToString() == updatePhoneWithZaloIdModel.ZaloUser))
                                          .FirstOrDefault();
                if (accountZaloExisting == null)
                {
                    return new APIResponseModel
                    {
                        Message = "Account not found.",
                        IsSuccess = false
                    };
                }

                var createDate = accountZaloExisting.CreateDate;
                accountZaloExisting.PhoneNumber = updatePhoneWithZaloIdModel.PhoneNumber;
                accountZaloExisting.UpdateDate = DateTime.Now;
                accountZaloExisting.CreateDate = createDate;
                var result = await _unitOfWork.AccountRepository.UpdateAsync(accountZaloExisting);
                _unitOfWork.Save();

                return new APIResponseModel
                {
                    Message = "Phone Updated Successfully",
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return new APIResponseModel { IsSuccess = false, Message = "An error occurred while checking if the account exists." };
            }
        }

        public async Task<APIResponseModel> SignUpStaffAccountAsync(AccountSignUpModel signUpModel)
        {
            try
            {
                if (string.IsNullOrEmpty(signUpModel.AccountEmail) || string.IsNullOrWhiteSpace(signUpModel.AccountEmail))
                {
                    return new APIResponseModel { IsSuccess = false, Message = "Username cannot be empty or whitespace." };
                }

                var existAccount = await _unitOfWork.AccountRepository.GetByConditionAsync(a => a.Email == signUpModel.AccountEmail);
                if (existAccount.Any())
                {
                    return new APIResponseModel { IsSuccess = false, Message = "Account already exists" };
                }
                if (signUpModel.FullName.Length < 3 || signUpModel.FullName.Length > 50)
                {
                    return new APIResponseModel
                    {
                        Message = "FullName must be between 3 and 50 characters.",
                        IsSuccess = false
                    };
                }

                if (signUpModel.BirthDate == default || signUpModel.BirthDate.Date >= DateTime.Now.Date)
                {
                    return new APIResponseModel
                    {
                        Message = "Date of Birth is invalid.",
                        IsSuccess = false
                    };
                }


                if (signUpModel.Address.Length < 5 || signUpModel.Address.Length > 120)
                {
                    return new APIResponseModel
                    {
                        Message = "Address must be between 5 and 120 characters.",
                        IsSuccess = false
                    };
                }
                var user = new Account
                {
                    FullName = signUpModel.FullName,
                    Dob = signUpModel.BirthDate,
                    Gender = signUpModel.Gender,
                    Address = signUpModel.Address,
                    UserName = signUpModel.AccountEmail,
                    Email = signUpModel.AccountEmail,
                    PhoneNumber = signUpModel.AccountPhone,
                    AvatarUrl = "http://res.cloudinary.com/dhovknfgb/image/upload/v1732960399/uzewxkbhnv0xyvahpuj0.jpg",
                    CreateDate = DateTime.Now,
                    Status = 1,
                    Roles = (int)ERole.Staff,
                    
                };

                var passwordHasher = new PasswordHasher<Account>();
                user.PasswordHash = passwordHasher.HashPassword(user, signUpModel.AccountPassword);

                await _unitOfWork.AccountRepository.AddAsync(user);
                _unitOfWork.Save();


                return new APIResponseModel { IsSuccess = true, Message = "Registration successful." };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return new APIResponseModel { IsSuccess = false, Message = "An error occurred while checking if the account exists." };
            }
        }

        public async Task<APIResponseModel> SignUpSupplierAccountAsync(AccountSignUpModel signUpModel)
        {
            try
            {
                if (string.IsNullOrEmpty(signUpModel.AccountEmail) || string.IsNullOrWhiteSpace(signUpModel.AccountEmail))
                {
                    return new APIResponseModel { IsSuccess = false, Message = "Username cannot be empty or whitespace." };
                }

                var existAccount = await _unitOfWork.AccountRepository.GetByConditionAsync(a => a.Email == signUpModel.AccountEmail);
                if (existAccount.Any())
                {
                    return new APIResponseModel { IsSuccess = false, Message = "Account already exists" };
                }
                if (signUpModel.FullName.Length < 3 || signUpModel.FullName.Length > 50)
                {
                    return new APIResponseModel
                    {
                        Message = "FullName must be between 3 and 50 characters.",
                        IsSuccess = false
                    };
                }

                if (signUpModel.BirthDate == default || signUpModel.BirthDate.Date >= DateTime.Now.Date)
                {
                    return new APIResponseModel
                    {
                        Message = "Date of Birth is invalid.",
                        IsSuccess = false
                    };
                }


                if (signUpModel.Address.Length < 5 || signUpModel.Address.Length > 120)
                {
                    return new APIResponseModel
                    {
                        Message = "Address must be between 5 and 120 characters.",
                        IsSuccess = false
                    };
                }
                var user = new Account
                {
                    FullName = signUpModel.FullName,
                    Dob = signUpModel.BirthDate,
                    Gender = signUpModel.Gender,
                    Address = signUpModel.Address,
                    UserName = signUpModel.AccountEmail,
                    Email = signUpModel.AccountEmail,
                    PhoneNumber = signUpModel.AccountPhone,
                    AvatarUrl = "http://res.cloudinary.com/dhovknfgb/image/upload/v1732960399/uzewxkbhnv0xyvahpuj0.jpg",
                    CreateDate = DateTime.Now,
                    Status = 1,
                    Roles = (int)ERole.Supplier,

                };

                var passwordHasher = new PasswordHasher<Account>();
                user.PasswordHash = passwordHasher.HashPassword(user, signUpModel.AccountPassword);

                await _unitOfWork.AccountRepository.AddAsync(user);
                _unitOfWork.Save();


                return new APIResponseModel { IsSuccess = true, Message = "Registration successful." };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return new APIResponseModel { IsSuccess = false, Message = "An error occurred while checking if the account exists." };
            }
        }

        public async Task<APIResponseModel> SignUpSuperAdminAccountAsync(AccountSignUpModel signUpModel)
        {
            try
            {
                if (string.IsNullOrEmpty(signUpModel.AccountEmail) || string.IsNullOrWhiteSpace(signUpModel.AccountEmail))
                {
                    return new APIResponseModel { IsSuccess = false, Message = "Username cannot be empty or whitespace." };
                }

                var existAccount = await _unitOfWork.AccountRepository.GetByConditionAsync(a => a.Email == signUpModel.AccountEmail);
                if (existAccount.Any())
                {
                    return new APIResponseModel { IsSuccess = false, Message = "Account already exists" };
                }
                if (signUpModel.FullName.Length < 3 || signUpModel.FullName.Length > 50)
                {
                    return new APIResponseModel
                    {
                        Message = "FullName must be between 3 and 50 characters.",
                        IsSuccess = false
                    };
                }

                if (signUpModel.BirthDate == default || signUpModel.BirthDate.Date >= DateTime.Now.Date)
                {
                    return new APIResponseModel
                    {
                        Message = "Date of Birth is invalid.",
                        IsSuccess = false
                    };
                }


                if (signUpModel.Address.Length < 5 || signUpModel.Address.Length > 120)
                {
                    return new APIResponseModel
                    {
                        Message = "Address must be between 5 and 120 characters.",
                        IsSuccess = false
                    };
                }
                var user = new Account
                {
                    FullName = signUpModel.FullName,
                    Dob = signUpModel.BirthDate,
                    Gender = signUpModel.Gender,
                    Address = signUpModel.Address,
                    UserName = signUpModel.AccountEmail,
                    Email = signUpModel.AccountEmail,
                    PhoneNumber = signUpModel.AccountPhone,
                    AvatarUrl = "http://res.cloudinary.com/dhovknfgb/image/upload/v1732960399/uzewxkbhnv0xyvahpuj0.jpg",
                    CreateDate = DateTime.Now,
                    Status = 1,
                    Roles = (int)ERole.SuperAdmin,

                };

                var passwordHasher = new PasswordHasher<Account>();
                user.PasswordHash = passwordHasher.HashPassword(user, signUpModel.AccountPassword);

                await _unitOfWork.AccountRepository.AddAsync(user);
                _unitOfWork.Save();


                return new APIResponseModel { IsSuccess = true, Message = "Registration successful." };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return new APIResponseModel { IsSuccess = false, Message = "An error occurred while checking if the account exists." };
            }
        }

        public async Task<APIResponseModel> ChangeAccountRoleAsync(AccountChangeRoleViewModel accountChangeRoleViewModel)
        {
            try
            {
                var requesterAccount = await _unitOfWork.AccountRepository.GetByConditionAsync(a => a.UserName == accountChangeRoleViewModel.UserName);
                var requester = requesterAccount.FirstOrDefault();

                if (requester == null || (ERole)requester.Roles != ERole.SuperAdmin)
                {
                    return new APIResponseModel { IsSuccess = false, Message = "No permission to update role." };
                }

                var targetAccountResult = await _unitOfWork.AccountRepository.GetByConditionAsync(a => a.Id == accountChangeRoleViewModel.UserId);
                var targetAccount = targetAccountResult.FirstOrDefault();

                if (targetAccount == null)
                {
                    return new APIResponseModel { IsSuccess = false, Message = "Account does not exist." };
                }

                if ((ERole)targetAccount.Roles == ERole.Customer || accountChangeRoleViewModel.NewRole == ERole.Customer)
                {
                    return new APIResponseModel { IsSuccess = false, Message = "Cannot change role from Customer or to customer." };
                }

                // Kiểm tra role hợp lệ
                if (!Enum.IsDefined(typeof(ERole), accountChangeRoleViewModel.NewRole))
                {
                    return new APIResponseModel { IsSuccess = false, Message = "Invalid role." };
                }

                // Cập nhật role cho tài khoản
                targetAccount.Roles = (int)accountChangeRoleViewModel.NewRole;

                // Lưu thay đổi vào database
                _unitOfWork.AccountRepository.UpdateAsync(targetAccount);
                _unitOfWork.Save();

                return new APIResponseModel { IsSuccess = true, Message = "Role update successful." };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return new APIResponseModel { IsSuccess = false, Message = "An error occurred while changing the account role." };
            }
        }

        public async Task<APIResponseModel> GetAllAccountByRole(AccountInforByRole accountInforByRole)
        {
            try
            {
                var requesterAccount = await _unitOfWork.AccountRepository.GetByConditionAsync(a => a.UserName == accountInforByRole.UserName);
                var requester = requesterAccount.FirstOrDefault();

                if (requester == null)
                {
                    return new APIResponseModel
                    {
                        Message = "Account does not exist.",
                        IsSuccess = false,
                        Data = null
                    };
                }

                var requesterRole = (ERole)requester.Roles;
                IEnumerable<Account> accountsQuery = null;

                if (requesterRole == ERole.Admin)
                {
                    accountsQuery = await _unitOfWork.AccountRepository.GetAllAsyncs(query =>
                                               query.Where(a => a.Roles != (int)ERole.SuperAdmin && a.Roles != requester.Roles));
                }
                else if (requesterRole == ERole.SuperAdmin)
                {
                    accountsQuery = await _unitOfWork.AccountRepository.GetAllAsyncs(query =>
                                              query.Where(a => a.Roles != requester.Roles));
                }
                else if (requesterRole == ERole.Staff)
                {
                    return new APIResponseModel
                    {
                        Message = "No permission to view.",
                        IsSuccess = false,
                        Data = null
                    };
                }
                else if (requesterRole == ERole.Supplier)
                {
                    return new APIResponseModel
                    {
                        Message = "No permission to view.",
                        IsSuccess = false,
                        Data = null
                    };
                }
                else if (requesterRole == ERole.Customer)
                {
                    return new APIResponseModel
                    {
                        Message = "No permission to view.",
                        IsSuccess = false,
                        Data = null
                    };
                }

                if (accountsQuery == null || !accountsQuery.Any())
                {
                    return new APIResponseModel
                    {
                        Message = "No accounts found.",
                        IsSuccess = false,
                        Data = null
                    };
                }
                return new APIResponseModel
                {
                    Message = "Get All Account Successfully",
                    IsSuccess = true,
                    Data = accountsQuery
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return new APIResponseModel
                {
                    Message = "An error occurred while retrieving accounts.",
                    IsSuccess = false,
                    Data = null
                };
            }
        }

        public async Task<APIResponseModel> UpdateProfile(UpdateProfile updateProfile)
        {
            using var transaction = _unitOfWork.BeginTransaction();
            try
            {
                var userId = await _unitOfWork.AccountRepository.GetByIdStringAsync(updateProfile.UserId);
                if (userId == null)
                {
                    return new APIResponseModel
                    {
                        Message = "Account not found",
                        IsSuccess = false
                    };
                }
                if (string.IsNullOrEmpty(updateProfile.FullName))
                {
                    return new APIResponseModel
                    {
                        Message = "FullName cannot be empty.",
                        IsSuccess = false
                    };
                }

                if (updateProfile.FullName.Length < 3 || updateProfile.FullName.Length > 50)
                {
                    return new APIResponseModel
                    {
                        Message = "FullName must be between 3 and 50 characters.",
                        IsSuccess = false
                    };
                }

                if (string.IsNullOrEmpty(updateProfile.PhoneNumber))
                {
                    return new APIResponseModel
                    {
                        Message = "PhoneNumber cannot be empty.",
                        IsSuccess = false
                    };
                }

                if (!Regex.IsMatch(updateProfile.PhoneNumber, @"^0\d{9,14}$"))
                {
                    return new APIResponseModel
                    {
                        Message = "PhoneNumber must start with '0' and be a valid number with 10 to 15 digits.",
                        IsSuccess = false
                    };
                }

                if (updateProfile.Dob == default || updateProfile.Dob.Value >= DateTime.Now.Date)
                {
                    return new APIResponseModel
                    {
                        Message = "Date of Birth is invalid.",
                        IsSuccess = false
                    };
                }

                if (string.IsNullOrEmpty(updateProfile.Address))
                {
                    return new APIResponseModel
                    {
                        Message = "Address cannot be empty.",
                        IsSuccess = false
                    };
                }

                if (updateProfile.Address.Length < 5 || updateProfile.Address.Length > 120)
                {
                    return new APIResponseModel
                    {
                        Message = "Address must be between 5 and 120 characters.",
                        IsSuccess = false
                    };
                }

                if (string.IsNullOrEmpty(updateProfile.AvatarUrl))
                {
                    return new APIResponseModel
                    {
                        Message = "AvatarUrl is not a valid.",
                        IsSuccess = false
                    };
                }

                userId.Dob = updateProfile.Dob;
                userId.Address = updateProfile.Address;
                userId.FullName = updateProfile.FullName;
                userId.PhoneNumber = updateProfile.PhoneNumber;
                userId.AvatarUrl = updateProfile.AvatarUrl;
                userId.UpdateDate = DateTime.Now;

                _unitOfWork.AccountRepository.UpdateAsync(userId);
                _unitOfWork.Save();
                transaction.Commit();

                return new APIResponseModel
                {
                    Message = "Get All Account Successfully",
                    IsSuccess = true,
                };
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return new APIResponseModel
                {
                    Message = "An error occurred while retrieving accounts.",
                    IsSuccess = false,
                    Data = null
                };
            }
        }
    }
}
