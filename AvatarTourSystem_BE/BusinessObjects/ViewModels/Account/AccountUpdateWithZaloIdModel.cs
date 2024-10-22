using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels.Account
{
    public class AccountUpdateWithZaloIdModel
    {
    
        public string? FullName { get; set; }
        [JsonIgnore]
        public DateTime? UpdateDate { get; set; }
        public string? AvatarUrl { get; set; }
        public string? ZaloUser { get; set; }
      
    }
    public class AccountUpdatePhoneWithZaloIdModel
    {
        public string? ZaloUser { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
