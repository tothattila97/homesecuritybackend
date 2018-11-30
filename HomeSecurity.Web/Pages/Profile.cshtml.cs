using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeSecurity.Bll.Database.Entities;
using HomeSecurity.Bll.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace HomeSecurity.Web.Pages
{
    public class ProfileModel : PageModel
    {
        public IConfiguration Configuration { get; }
        public UserManager<User> UserManager { get; }

        [BindProperty]
        public ProfileDataModel Profile { get; set; }

        public ProfileModel(IConfiguration configuration,
            UserManager<User> userManager)
        {
            Configuration = configuration;
            UserManager = userManager;
        }

        public class ProfileDataModel
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string UserName { get; set; }
            public string Email { get; set; }
            public string PhoneNumber { get; set; }
            public DateTimeOffset DateOfBirth { get; set; }
            public Gender Gender { get; set; }
        }

        public void OnGet()
        {
            Profile = UserManager.GetUserAsync(HttpContext.User).GetAwaiter().GetResult().MapTo<User,ProfileDataModel>();       
        }
    }
}