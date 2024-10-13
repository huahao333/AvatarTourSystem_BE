using BusinessObjects.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels.Account
{
    public class AccountZaloIdViewModel
    {
       
        public string? UserName { get; set; }
        public string? FullName { get; set; }
        public string? IsPhoneNumber { get; set; }
        [JsonIgnore]
        public string? PhoneNumber { get; set; }

        public string? AvatarUrl { get; set; }
        public string? ZaloUser { get; set; }
        
    }
}
