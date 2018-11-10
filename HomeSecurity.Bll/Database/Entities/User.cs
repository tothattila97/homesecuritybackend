using HomeSecurity.Bll.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HomeSecurity.Bll.Database.Entities
{
    public class User: IdentityUser<int>
    {
        [Required]
        public string Surname { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string ContainerId { get; set; }
        [Required]
        public DateTimeOffset DateOfBirth { get; set; }
        public DateTimeOffset DateOfRegistration { get; set; }
        public DateTimeOffset DateOfLastLogin { get; set; }
        public Gender Gender { get; set; }
    }
}
