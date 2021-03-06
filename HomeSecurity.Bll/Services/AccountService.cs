﻿using HomeSecurity.Bll.Database;
using HomeSecurity.Bll.Database.Entities;
using HomeSecurity.Bll.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<bool> SignUp(UserSignUpModel model)
        {
            var registrationDate = DateTimeOffset.Now;
            var containerName = new string(model.UserName.Where(char.IsLetterOrDigit).ToArray()).ToLower();
            var user = new User
            {
                UserName = model.UserName,
                FirstName = model.FirstName,
                LastName = model.LastName,
                DateOfBirth = model.DateOfBirth,
                PhoneNumber = model.PhoneNumber,
                DateOfLastLogin = registrationDate,
                DateOfRegistration = registrationDate,
                Gender = model.Gender,
                Email = model.Email,
                ContainerId = containerName + Guid.NewGuid().ToString()
            };

            var result = await UserManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await SignInManager.SignInAsync(user, false);
                return true;
            }
            else
                return false;
        }

        public async Task<bool> Login(UserLoginModel model)
        {
            var user = await UserManager.FindByEmailAsync(model.Email);
            var result = await SignInManager.PasswordSignInAsync(user.UserName, model.Password, model.IsPersistent, true);

            if (result.Succeeded)
            {
                user.DateOfLastLogin = DateTimeOffset.Now;
                await UserManager.UpdateAsync(user);
                return true;
            }
            else
                return false;
        }

        public async Task Logout()
            => await SignInManager.SignOutAsync();

        public async Task<bool> ChangePassword(ChangePasswordModel model, int userId)
        {
            // A modelből ha csak az egyik elem jön le akkor is megtaláljuk a felhasználót
            var user = await UserManager.FindByEmailAsync(userId.ToString());

            var result = await UserManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (!result.Succeeded)
                return false;
            else
                return true;
        }

        public async Task<bool> DeleteUser(int userId)
        {
            //HttpContext bekötése hogy tudjuk melyik usert kell törölni
            var deletableUser = await UserManager.FindByIdAsync(userId.ToString());
            var result = await UserManager.DeleteAsync(deletableUser);
            if (!result.Succeeded)
                return false;
            else
                return true;
                
        }
    }
}
