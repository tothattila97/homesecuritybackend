using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HomeSecurity.Bll.Database.Entities;
using HomeSecurity.Bll.Models;
using HomeSecurity.Bll.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace HomeSecurity.Web.Controllers
{
    [Route("api/upload")]
    [ApiController]
    [Authorize]
    public class UploadController : Controller
    {
        public IConfiguration Configuration { get; }
        public UserManager<User> UserManager { get; }
        public UploadService UploadService { get; }
        public string AzureConnectionString { get; } = "DefaultEndpointsProtocol=https;AccountName=homesecurityimages;AccountKey=yHqvH+aXFHEGSzfft0tUNjS9UDxCEPqPvOvw0ZHwPBWUOriJTrVxG1VmIrHdNxGRaWqgSmqOuFVWZIrzGt8fkA==;EndpointSuffix=core.windows.net";
        public string AccountName { get; }
        public string AccountKey { get; }

        public UploadController(
            IConfiguration configuration,
            UserManager<User> userManager,
            UploadService uploadService)
        {
            Configuration = configuration;
            UserManager = userManager;
            UploadService = uploadService;
            AccountName = Configuration.GetSection("AzureStorageConfig").GetValue<string>("AccountName");
            AccountKey = Configuration.GetSection("AzureStorageConfig").GetValue<string>("AccountKey");
        }

        [Consumes("multipart/form-data")]
        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile imageFile, [FromForm] bool isNotifiableByEmail)
        {
            bool isUploaded = false;
            try
            {

                if (imageFile == null)
                    return BadRequest("Nincs fogadott feltöltendő fájl");
                if (AccountKey == string.Empty || AccountName == string.Empty)
                    return BadRequest("Nem tudjuk feloldani az Azure tárhelyed, csekkold hogy van megadva accountName és accountKey!");

                if (UploadService.IsImage(imageFile) && imageFile.Length > 0)
                {
                    using (Stream stream = imageFile.OpenReadStream())
                    {
                        isUploaded = await UploadService.UploadImageToPersonalBlobStorage(stream, DateTime.Now.ToString("yyMMddHHmmssffffff") + imageFile.FileName, AzureConnectionString, await GetCurrentUserIdAsync(), isNotifiableByEmail);
                    }
                }
                else
                    return new UnsupportedMediaTypeResult();

                return new OkResult();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        private async Task<int> GetCurrentUserIdAsync()
            => (await UserManager.GetUserAsync(HttpContext.User)).Id;
    }
}
