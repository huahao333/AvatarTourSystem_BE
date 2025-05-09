﻿using BusinessObjects.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels.Account
{
    public class AccountViewModel
    {
      //  [FromForm(Name = "user-id")]
        public string? Id { get; set; }
      //  [FromForm(Name = "user-name")]
        public string? UserName { get; set; }
     //   [FromForm(Name = "gender")]
        public bool? Gender { get; set; }
      //  [FromForm(Name = "dob")]
        public DateTime? Dob { get; set; }
      //  [FromForm(Name = "address")]
        public string? Address { get; set; }
      //  [FromForm(Name = "full-name")]
        public string? FullName { get; set; }
     //   [FromForm(Name = "phone-number")]
        public string? PhoneNumber { get; set; }
     //   [FromForm(Name = "avatar-url")]
        public string? AvatarUrl { get; set; }
     //   [FromForm(Name = "zalo-user")]
        public string? ZaloUser { get; set; }
     //   [FromForm(Name = "email")]
        public string? Email { get; set; }
     //   [FromForm(Name = "create-date")]
        public DateTime? CreateDate { get; set; }
     //   [FromForm(Name = "update-date")]
        public DateTime? UpdateDate { get; set; }
      //  [FromForm(Name = "status")]
        public EStatus? Status { get; set; }
    }
}
