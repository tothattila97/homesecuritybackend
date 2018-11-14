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

        public async Task<UserProfileModel> GetProfile(int userId)
            => (await UserManager.FindByIdAsync(userId.ToString()))?.MapTo<User, UserProfileModel>();

        public async Task UpdateProfile(UserProfileUpdateModel model, int userId)
        {
            var user = await UserManager.FindByIdAsync(userId.ToString());

            user.Email = model.Email;
            user.Surname = model.Surname;
            user.LastName = model.LastName;
            user.PhoneNumber = model.PhoneNumber;
            user.UserName = model.UserName;

            await UserManager.UpdateAsync(user);
        }

    }
}
