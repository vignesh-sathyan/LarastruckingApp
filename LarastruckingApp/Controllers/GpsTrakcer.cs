using System;
using System.Collections.Generic;
using System.Linq;
using LarastruckingApp.Controllers;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LarastruckingApp.Controllers
{
    [Authorize]
    public class GpsTrakcer : ApiController
    {
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        [HttpPost]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
        [Route("[api/controller]")]
        public string Gets()
        {
            return "values";
        }
    }
}