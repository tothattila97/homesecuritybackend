using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HomeSecurity.Web.Controllers
{
    [Produces("application/json")]
    [Consumes("application/json")]
    [Route("api/account/[action]")]
    [ApiController]
    public class AccountController : Controller
    {
        
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        public IActionResult Logout()
        {
            return View();
        }
    }
}
