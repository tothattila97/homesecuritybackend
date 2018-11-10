using System;
using System.Collections.Generic;
using System.Text;

namespace HomeSecurity.Bll.Models
{
    public class UserProfileUpdateModel
    {
        public string Email { get; set; }
        public string Surname { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }
    }
}
