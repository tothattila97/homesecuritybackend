using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace HomeSecurity.Web.Controllers
{
    [Route("api/upload")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        public IConfiguration Configuration { get; }

        public UploadController(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        
        [HttpPost]
        public async Task UploadImage()
        {
            var accountName = Configuration.GetSection("AzureStorageConfig").GetValue<string>("AccountName");
            var accountKey = Configuration.GetSection("AzureStorageConfig").GetValue<string>("AccountKey");

            
        }
    }
}
