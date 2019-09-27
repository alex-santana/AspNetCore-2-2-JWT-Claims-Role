using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCore_2_2_JWT.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static AspNetCore_2_2_JWT.Extensions.CustomAuthorization;

namespace AspNetCore_2_2_JWT.Controllers
{
    [Authorize(Roles = Roles.ROLE_API_ADM)]
    [Route("api/cursos")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [Authorize(Roles= Roles.ROLE_API_BASIC)]
        [ClaimsAuthorize("Curso", "Get")]
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "Angular", "React", "Asp.Net Core", "Ionic", "Docker" };
        }

        // GET api/values/5
        [ClaimsAuthorize("Curso", "Get")]
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [ClaimsAuthorize("Curso", "Add")]
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [ClaimsAuthorize("Curso", "Upd")]
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [ClaimsAuthorize("Curso", "Del")]
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}


