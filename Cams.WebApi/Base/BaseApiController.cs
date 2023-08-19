using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace XichLip.WebApi.Base
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BaseApiController : Controller
    {
        public long CurrentUserId
        {
            get
            {
                if (User != null && User.Identity != null && User.Identity.IsAuthenticated)
                {
                    return long.Parse(User.FindFirstValue("wsid"));
                }
                else
                {
                    return 0;
                }

            }
        }
        public string CurrentUserName
        {
            get
            {
                if (User.Identity.IsAuthenticated)
                {
                    return User.FindFirst(ClaimTypes.Name).Value;
                }
                else
                {
                    return string.Empty;
                }

            }
        }
        protected string GetCurrentUserId(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var tokenS = handler.ReadToken(token) as JwtSecurityToken;
            //var jti = tokenS.Claims.FirstOrDefault().Value;
            return tokenS.Subject;
        }
        protected async Task<string> GetCurrentAccessToken()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            return accessToken;
        }
        
    }
}