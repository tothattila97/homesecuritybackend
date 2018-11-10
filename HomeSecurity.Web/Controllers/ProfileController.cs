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
    [Route("api/profile")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        // GET: api/Profil
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Profil/5
        [HttpGet("{id}", Name = "GetProfile")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Profil
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Profil/5
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
