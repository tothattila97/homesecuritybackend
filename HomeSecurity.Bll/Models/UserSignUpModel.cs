using HomeSecurity.Bll.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HomeSecurity.Bll.Models
{
    public class UserSignUpModel
    {
        [Required(ErrorMessage = "Email is required!")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Username is required!")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password is required!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Confirm password is required!")]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPasswrod { get; set; }
        [Required(ErrorMessage = "Phonenumber is required!")]
        [Phone]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Date of birth is required!")]
        public DateTimeOffset DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        [Required]
        public string Surname { get; set; }
        public string LastName { get; set; }
    }
}
