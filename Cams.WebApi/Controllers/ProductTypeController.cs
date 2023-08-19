using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkSimple.Infrastructure;
using WorkSimple.Infrastructure.Services;
using XichLip.WebApi.Base;
using XichLip.WebApi.Constants;
using XichLip.WebApi.Interface;
using XichLip.WebApi.Models.Base;
using WsResource = XichLip.WebApi.Resources.WsResource;

namespace XichLip.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductTypeController : BaseApiController
    {
        private IConfiguration _config;
        private IProductTypeRepository _repo;
        private WsLanguage<WsResource> _lang;
        private readonly ILogger<AuthController> _logger;
        private readonly IEmailSender _emailSender;
        private readonly ITokenService _tokenService;

        public ProductTypeController(
            IConfiguration config,
            IProductTypeRepository repo,
            WsLanguage<WsResource> lang,
            ITokenService tokenService,
            ILogger<AuthController> logger,
            IEmailSender emailSender
            )
        {
            _config = config;
            _repo = repo;
            _logger = logger;
            _lang = lang;
            _tokenService = tokenService;
            _emailSender = emailSender;
        }
        [AllowAnonymous]
        [HttpPost("Select-list")]
        public async Task<WsResponse> GetProductTypeSelectList()
        {
            WsResponse response = new WsResponse();
            response.Status = WsConstants.Statusfail;
            var res = _repo.GetProductTypeSelectList().Result;

            if (res != null)
            {
                response.Status = WsConstants.StatusSuccess;
                response.Data = res;
            }
            else
            {
                response.Status = WsConstants.Statusfail;
                response.Errors = new List<WsError> { new WsError { Code = WsConstants.CodeStatusFail, Message = _lang.Text("ErrorProcessing") } };
            }

            return response;
        }
        
    }
}
