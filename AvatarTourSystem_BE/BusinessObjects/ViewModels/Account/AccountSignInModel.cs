using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels.Account
{
    public class AccountSignInModel
    {
        [Required(ErrorMessage = "Email can not be blank!")]
        public String AccountEmail { get; set; } = string.Empty;
        [Required(ErrorMessage = "Password can not be blank!")]
        [DataType(DataType.Password)]
        [PasswordPropertyText]
        public string AccountPassword { get; set; } = string.Empty;
    }
}
