using BusinessObjects.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels.Account
{
    public class AccountChangeRoleViewModel
    {
        public string UserName { get; set; }
        public string UserId { get; set; }
        public ERole NewRole { get; set; }
    }
    public class AccountInforByRole
    {
        public string UserName { get; set; }
    }
}
