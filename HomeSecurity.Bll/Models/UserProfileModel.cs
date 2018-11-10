using HomeSecurity.Bll.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace HomeSecurity.Bll.Models
{
    public class UserProfileModel
    {
        public string Email { get; set; }
        public string Surname { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTimeOffset DateOfBirth { get; set; }
        public Gender Gender { get; set; }
    }
}
