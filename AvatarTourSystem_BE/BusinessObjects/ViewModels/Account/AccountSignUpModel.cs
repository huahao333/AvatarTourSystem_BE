using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels.Account
{
    public class AccountSignUpModel
    {
        //public string AccountPhone { get; set; } = string.Empty;
        //public string FullName { get; set; } = string.Empty;
        //public bool Gender { get; set; } = false;
        //public DateTime BirthDate { get; set; }
        //public string Address { get; set; } = string.Empty;

        //[Required(ErrorMessage = "Email is required!"), EmailAddress(ErrorMessage = "Please enter valid email!")]
        //public string AccountEmail { get; set; } = string.Empty;

        //[Required(ErrorMessage = "Password is required!")]
        //[Display(Name = "Password")]
        //[DataType(DataType.Password)]
        //[StringLength(12, MinimumLength = 7, ErrorMessage = "Password must be 7-12 Character")]
        //[PasswordPropertyText]
        //public string AccountPassword { get; set; } = "";
        //[Required(ErrorMessage = "Confirm Password is required!")]
        //[Display(Name = "Confirm Password")]
        //[DataType(DataType.Password)]
        //[Compare("AccountPassword", ErrorMessage = "Password and confirmation does not match!")]
        //[StringLength(12, MinimumLength = 7, ErrorMessage = "Password must be 7-12 Character")]
        //[PasswordPropertyText]
        //public string ConfirmAccountPassword { get; set; } = "";
        [Required(ErrorMessage = "Phone number is required!")]
        [RegularExpression(@"^[0-9]{10,15}$", ErrorMessage = "Please enter a valid phone number.")]
        public string AccountPhone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Full name is required!")]
        public string FullName { get; set; } = string.Empty;

        // Nếu bạn muốn sử dụng enum cho Gender, bạn có thể định nghĩa enum trước.
        // public Gender Gender { get; set; } = Gender.Unknown; // Ví dụ enum

        public bool Gender { get; set; } = false; // Hoặc enum như đã nêu

        [Required(ErrorMessage = "Birth date is required!")]
        public DateTime BirthDate { get; set; }

        public string Address { get; set; } = "";

        [Required(ErrorMessage = "Email is required!")]
        
        public string AccountEmail { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required!")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [StringLength(12, MinimumLength = 7, ErrorMessage = "Password must be 7-12 characters long.")]
        [PasswordPropertyText]
        public string AccountPassword { get; set; } = "";

        [Required(ErrorMessage = "Confirm Password is required!")]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("AccountPassword", ErrorMessage = "Password and confirmation do not match!")]
        [StringLength(12, MinimumLength = 7, ErrorMessage = "Password must be 7-12 characters long.")]
        [PasswordPropertyText]
        public string ConfirmAccountPassword { get; set; } = "";
    }
}
