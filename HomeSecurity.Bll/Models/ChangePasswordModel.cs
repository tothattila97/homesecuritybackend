using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HomeSecurity.Bll.Models
{
    public class ChangePasswordModel
    {
        [EmailAddress(ErrorMessage = "Email is required!")]
        public string Email { get; set; }
        public string UserName { get; set; }
        [Required(ErrorMessage = "Current password is required!")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }
        [Required(ErrorMessage = "New password is required!")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [Required(ErrorMessage = "Confirmation of new password is required!")]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmNewPasswrod { get; set; }
    }
}
