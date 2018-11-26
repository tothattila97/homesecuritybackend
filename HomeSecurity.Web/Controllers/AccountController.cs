using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using HomeSecurity.Bll.Database.Entities;
using HomeSecurity.Bll.Models;
using HomeSecurity.Bll.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HomeSecurity.Web.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : Controller
    {
        public AccountService AccountService { get; set; }
        public UserManager<User> UserManager { get; }

        public AccountController(
            AccountService accountService,
            UserManager<User> userManager)
        {
            AccountService = accountService;
            UserManager = userManager;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp(UserSignUpModel model)
         => await AccountService.SignUp(model) == "" ? Ok() : (IActionResult)BadRequest();

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginModel model)
        {
            var claimsIdentity = new ClaimsIdentity(new List<Claim> {
                new Claim(ClaimTypes.Actor, (await UserManager.FindByNameAsync(model.UserName)).Id.ToString()) },
                CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {

            };
            await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties);

            return await AccountService.Login(model) == "" ? Ok() : (IActionResult)BadRequest();
        }
        [Authorize]
        [HttpPost("logout")]
        public async Task Logout()
        {
            await AccountService.Logout();
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);
        }
        [Authorize]
        [HttpPost("changepassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
            => await AccountService.ChangePassword(model, await GetCurrentUserIdAsync()) == "" 
                        ? Ok() : (IActionResult)BadRequest();

        [Authorize]
        [HttpDelete("deleteaccount")]
        public async Task<IActionResult> DeleteUser()
            => await AccountService.DeleteUser(await GetCurrentUserIdAsync()) == "" 
                        ? Ok() : (IActionResult)BadRequest();

        private async Task<int> GetCurrentUserIdAsync() => (await UserManager.GetUserAsync(HttpContext.User)).Id;
    }
}
