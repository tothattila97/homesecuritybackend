using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HomeSecurity.Bll.Models
{
    public class UserLoginModel
    {
        [Required(ErrorMessage = "Email/username is required!")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required!")]
        public string Password { get; set; }
        public bool IsPersistent { get; set; } = true;
    }
}
