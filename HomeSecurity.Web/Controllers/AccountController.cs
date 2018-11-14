using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeSecurity.Bll.Database.Entities;
using HomeSecurity.Bll.Models;
using HomeSecurity.Bll.Services;
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
        public async Task SignUp(UserSignUpModel model)
            => await AccountService.SignUp(model);

        [HttpPost("login")]
        public async Task Login(UserLoginModel model)
            => await AccountService.Login(model);

        [Authorize]
        [HttpPost("logout")]
        public async Task Logout()
            => await AccountService.Logout();

        [Authorize]
        [HttpPost("changepassword")]
        public async Task ChangePassword(ChangePasswordModel model)
            => await AccountService.ChangePassword(model, await GetCurrentUserIdAsync());

        [Authorize]
        [HttpDelete("deleteaccount")]
        public async Task DeleteUser()
            => await AccountService.DeleteUser(await GetCurrentUserIdAsync());

        private async Task<int> GetCurrentUserIdAsync() => (await UserManager.GetUserAsync(HttpContext.User)).Id;
    }
}
