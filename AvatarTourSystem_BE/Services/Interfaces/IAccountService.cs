using BusinessObjects.ViewModels.Account;
using Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IAccountService
    {
        Task<APIResponseModel> GetAllAccount();
        Task<APIResponseModel> GetAccountByStatus();
        Task<APIResponseModel> GetAccountById(string accountId);
        Task<APIResponseModel> GetAccountByZaloID(string zaloId);
        Task<APIResponseModel> CreateAccount(AccountCreateModel createModel);
        Task<APIResponseModel> UpdateAccount(AccountUpdateModel updateModel);
        Task<APIResponseModel> UpdateAccountWithZaloId(AccountUpdateWithZaloIdModel updateModel);
        Task<APIResponseModel> DeleteAccount(string accountId);
        Task<APIResponseModel> SignUpAccountAsync(AccountSignUpModel signUpModel);
        Task<APIResponseModel> SignUpStaffAccountAsync(AccountSignUpModel signUpModel);
        Task<APIResponseModel> SignUpSupplierAccountAsync(AccountSignUpModel signUpModel);
        Task<APIResponseModel> SignUpSuperAdminAccountAsync(AccountSignUpModel signUpModel);
        Task<APIResponseModel> SignUpAccountZaloAsync(AccountZaloIdModel accountZaloIdModel);
        Task<APIAuthenticationResponseModel> SignInAccountAsync(AccountSignInModel signInModel);
        Task<APIResponseModel> GetPhoneInfoAndSaveAsync(AccountZaloCURLModel accountZaloCURLModel);
        Task<APIResponseModel> UpdatePhoneNumberByZaloId(AccountUpdatePhoneWithZaloIdModel updatePhoneWithZaloIdModel);
        Task<APIResponseModel> ChangeAccountRoleAsync(AccountChangeRoleViewModel accountChangeRoleViewModel);
        Task<APIResponseModel> GetAllAccountByRole(AccountInforByRole accountInforByRole);
    }
}
