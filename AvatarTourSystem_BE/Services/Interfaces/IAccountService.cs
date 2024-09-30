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
        Task<APIResponseModel> CreateAccount(AccountCreateModel createModel);
        Task<APIResponseModel> UpdateAccount(AccountUpdateModel updateModel);
        Task<APIResponseModel> DeleteAccount(string accountId);
        Task<APIResponseModel> SignUpAccountAsync(AccountSignUpModel signUpModel);
        Task<APIResponseModel> SignUpAccountZaloAsync(AccountZaloIdModel accountZaloIdModel);
        Task<APIAuthenticationResponseModel> SignInAccountAsync(AccountSignInModel signInModel);
    }
}
