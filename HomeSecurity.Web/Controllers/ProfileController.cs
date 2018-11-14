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
    [Produces("application/json")]
    [Consumes("application/json")]
    [Route("api/profile")]
    [ApiController]
    [Authorize]
    public class ProfileController : ControllerBase
    {
        public ProfileService ProfileService { get; set; }
        public UserManager<User> UserManager { get; }

        public ProfileController(
            ProfileService profileService,
            UserManager<User> userManager)
        {
            ProfileService = profileService;
            UserManager = userManager;
        }

        [HttpGet]
        public async Task GetProfil()
            => await ProfileService.GetProfile(await GetCurrentUserIdAsync());

        [HttpPut]
        public async Task<IActionResult> UpdateProfile(UserProfileUpdateModel model)
            => await ProfileService.UpdateProfile(model, await GetCurrentUserIdAsync()) == "" 
                        ? Ok() : (IActionResult)BadRequest();

        private async Task<int> GetCurrentUserIdAsync() => (await UserManager.GetUserAsync(HttpContext.User)).Id;
    }
}
