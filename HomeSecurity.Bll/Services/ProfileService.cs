using HomeSecurity.Bll.Database;
using HomeSecurity.Bll.Database.Entities;
using HomeSecurity.Bll.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeSecurity.Bll.Services
{
    public class ProfileService
    {
        public HomeSecurityDbContext Context { get; set; }
        public UserManager<User> UserManager { get; set; }

        public ProfileService(
            HomeSecurityDbContext context,
            UserManager<User> userManager)
        {
            Context = context;
            UserManager = userManager;
        }

        public async Task<UserProfileModel> GetProfile()
            => (await UserManager.FindByIdAsync("1"))?.MapTo<User, UserProfileModel>();

        public async Task UpdateProfile(UserProfileUpdateModel model)
        {
            var user = await UserManager.FindByIdAsync("1");

            user.Email = model.Email;
            user.Surname = model.Surname;
            user.LastName = model.LastName;
            user.PhoneNumber = model.PhoneNumber;
            user.UserName = model.UserName;

            await UserManager.UpdateAsync(user);
        }

    }
}
