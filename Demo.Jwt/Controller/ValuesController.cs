using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Jwt.Controller
{
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/value1
        [HttpGet]
        [Route("api/value1")]
        public ActionResult<IEnumerable<string>> Get() 
        {
            return new string[] { "value1", "value1" };
        }

        // GET api/value2
        [HttpGet]
        [Route("api/value2")]
        [Authorize]
        public ActionResult<IEnumerable<string>> Get2()
        {
            var auth = HttpContext.AuthenticateAsync();
            var userName = auth.Result.Principal.Claims.First(t => t.Type.Equals(ClaimTypes.NameIdentifier))?.Value;
            return new string[] { "value2", "value2", $"userName={userName}" };
        }


    }
}