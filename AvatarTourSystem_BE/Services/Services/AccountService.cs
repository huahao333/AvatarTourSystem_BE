using AutoMapper;
using BusinessObjects.Enums;
using BusinessObjects.Models;
using BusinessObjects.ViewModels.Account;
using BusinessObjects.ViewModels.Feedback;
using BusinessObjects.ViewModels.Rate;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Repositories.Interfaces;
using Services.Common;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<Account> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AccountService(IUnitOfWork unitOfWork, 
                              IMapper mapper,
                              UserManager<Account> userManager,
                              RoleManager<IdentityRole> roleManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
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
            var account = await _unitOfWork.AccountRepository.GetByConditionAsync(x => x.Id == accountId);
            if (account == null)
            {
                return new APIResponseModel
                {
                    Message = "Account not found.",
                    IsSuccess = false,
                    Data = null
                };
            }
            else
            {
                var accountViewModels = _mapper.Map<List<AccountViewModel>>(account);

                return new APIResponseModel
                {
                    Message = "Account found",
                    IsSuccess = true,
                    Data = accountViewModels
                };

            }
            

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
    }
}
