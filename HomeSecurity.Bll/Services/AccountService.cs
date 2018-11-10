using HomeSecurity.Bll.Database;
using HomeSecurity.Bll.Database.Entities;
using HomeSecurity.Bll.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HomeSecurity.Bll.Services
{
    public class AccountService
    {
        public HomeSecurityDbContext Context { get; set; }
        public UserManager<User> UserManager { get; set; }
        public SignInManager<User> SignInManager { get; set; }

        public AccountService(
            HomeSecurityDbContext context,
            UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            Context = context;
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public async Task SignUp(UserSignUpModel model)
        {
            var registrationDate = DateTimeOffset.Now;
            var user = new User
            {
                UserName = model.UserName,
                Surname = model.Surname,
                LastName = model.LastName,
                DateOfBirth = model.DateOfBirth,
                PhoneNumber = model.PhoneNumber,
                DateOfLastLogin = registrationDate,
                DateOfRegistration = registrationDate,
                Gender = model.Gender,
                Email = model.Email,
                ContainerId = model.UserName + Guid.NewGuid().ToString()
            };

            var result = await UserManager.CreateAsync(user);
            if (result.Succeeded)
                await SignInManager.SignInAsync(user, false);
        }

        public async Task Login(UserLoginModel model)
        {
            var user = await UserManager.FindByNameAsync(model.UserName);
            var result = await SignInManager.PasswordSignInAsync(user, model.Password, model.IsPersistent, true);

            if (result.Succeeded)
            {
                user.DateOfLastLogin = DateTimeOffset.Now;
                await UserManager.UpdateAsync(user);

            }
        }

        public async Task Logout()
            => await SignInManager.SignOutAsync();

        public async Task ChangePassword(ChangePasswordModel model)
        {
            // A modelből ha csak az egyik elem jön le akkor is megtaláljuk a felhasználót
            var user = await UserManager.FindByEmailAsync(model.Email);
            user = await UserManager.FindByNameAsync(model.UserName);

            var result = await UserManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (!result.Succeeded)
                throw new Exception("A jelszó helyreállítás nem sikerült");
        }

        public async Task DeleteUser()
        {
            //HttpContext bekötése hogy tudjuk melyik usert kell törölni
            var deletableUser = await UserManager.FindByIdAsync("1");
            var result = await UserManager.DeleteAsync(deletableUser);
            if (!result.Succeeded)
                throw new Exception("A felhasználói fiók törlése nem sikerült!");
        }
    }
}
