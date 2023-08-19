using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using XichLip.WebApi.Base;

namespace XichLip.WebApi.Controllers
{
    [ApiController]
    [Route("api/sample")]
    public class SampleController : BaseApiController
    {
        [HttpGet]
        [Authorize]
        public IEnumerable<string> Get()
        {
            return new string[] { "Sample 1", "Sample 2", CurrentUserId.ToString() };
        }
        [HttpGet("Get2")]
        [AllowAnonymous]
        public IEnumerable<string> Get2()
        {
            return new string[] { "Sample 1", "Sample 2" };
        }
        [HttpGet]
        [Route("user")]
        public JsonResult GetDefaultUser()
        {
            return new JsonResult(new { username = "5001 123" });
        }
    }

}