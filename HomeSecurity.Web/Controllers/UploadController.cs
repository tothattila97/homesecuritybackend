using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HomeSecurity.Web.Controllers
{
    [Route("api/upload")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        // GET: api/Upload
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Upload/5
        [HttpGet("{id}", Name = "GetUpload")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Upload
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Upload/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
