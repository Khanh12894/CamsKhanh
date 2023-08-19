using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WorkSimple.Infrastructure;
using WorkSimple.Infrastructure.Services;
using XichLip.WebApi.Base;
using XichLip.WebApi.Constants;
using XichLip.WebApi.Interface;
using XichLip.WebApi.Models.Base;
using XichLip.WebApi.Models.User;
using WsResource = XichLip.WebApi.Resources.WsResource;

namespace XichLip.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class UsersController : BaseApiController
    {

        private IConfiguration _config;
        private IUsersRepository _repo;
        private WsLanguage<WsResource> _lang;
        private readonly ILogger<AuthController> _logger;
        private readonly IEmailSender _emailSender;
        private readonly ITokenService _tokenService;
        private readonly IHostingEnvironment _hostingEnvironment;

        public UsersController(
            IConfiguration config,
            IUsersRepository repo,
            WsLanguage<WsResource> lang,
            ITokenService tokenService,
            ILogger<AuthController> logger,
            IEmailSender emailSender,
            IHostingEnvironment hostingEnvironment
            )
        {
            _config = config;
            _repo = repo;
            _logger = logger;
            _lang = lang;
            _tokenService = tokenService;
            _emailSender = emailSender;
            _hostingEnvironment = hostingEnvironment;
        }
        [AllowAnonymous]
        [HttpPost("GetList")]
        public async Task<WsResponse> GetList([FromBody] UsersRequestModel model)
        {
            WsResponse response = new WsResponse();
            response.Status = WsConstants.Statusfail;
            var res = _repo.GetList(model, CurrentUserId).Result;

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
        [AllowAnonymous]
        [HttpGet("detail")]
        public async Task<WsResponse> Get(long id)
        {
            WsResponse response = new WsResponse();
            string token = await GetCurrentAccessToken();
            var res = _repo.Get(id, CurrentUserId).Result;
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
        [AllowAnonymous]
        [HttpPost("Create")]
        public async Task<WsResponse> Create([FromBody] UsersModel model)
        {

            WsResponse response = new WsResponse();
            response.Status = WsConstants.Statusfail;
            if (ModelState.IsValid)
            {
                var res = _repo.Save(model, CurrentUserId).Result;
                if (res >= 1)
                {
                    response.Status = WsConstants.StatusSuccess;
                    response.Data = model;
                }
                else
                {
                    response.Status = WsConstants.Statusfail;
                    response.Errors = new List<WsError> { new WsError { Code = WsConstants.CodeStatusFail, Message = _lang.Text("ErrorProcessing") } };
                }
            }
            else
            {
                response.Status = WsConstants.Statusfail;
                response.Errors = ModelState.Where(ms => ms.Value.Errors.Any()).Select(x => new { x.Key, x.Value.Errors }).Select(e => new WsError { Code = WsConstants.CodeBadRequest, Field = e.Key, Message = string.Join(", ", e.Errors.Select(s => s.ErrorMessage)) }).ToList();
            }
            return response;
        }
        [AllowAnonymous]
        [HttpPost("Update")]
        public async Task<WsResponse> Update([FromBody] UsersModel model)
        {
            WsResponse response = new WsResponse();
            response.Status = WsConstants.Statusfail;

            if (ModelState.IsValid)
            {
                if ((model == null) || (model.Seq < 1))
                {
                    response.Errors = new List<WsError> { new WsError { Code = WsConstants.CodeStatusFail, Field = "PoolId", Message = string.Format(_lang.Text("ErrorParamId"), "PoolId") } };
                    return response;
                }

                var res = _repo.Save(model, CurrentUserId).Result;
                if (res > 0)
                {
                    response.Status = WsConstants.StatusSuccess;
                    response.Data = model;
                }
                else
                {
                    response.Errors = new List<WsError> { new WsError { Code = WsConstants.CodeStatusFail, Message = _lang.Text("ErrorProcessing") } };
                }
            }
            else
            {
                response.Errors = ModelState.Where(ms => ms.Value.Errors.Any())
                    .Select(x => new { x.Key, x.Value.Errors }).Select(e => new WsError { Code = WsConstants.CodeBadRequest, Field = e.Key, Message = string.Join(", ", e.Errors.Select(s => s.ErrorMessage)) }).ToList();
            }
            return response;
        }
        [AllowAnonymous]
        [HttpPost("Delete")]
        public async Task<WsResponse> Delete(BaseRequestGetModel model)
        {
            WsResponse response = new WsResponse();
            response.Status = WsConstants.Statusfail;

            if (ModelState.IsValid)
            {
                var res = _repo.Delete(model.Id, CurrentUserId).Result;
                if (res >= 0)
                {
                    if (res == 1)
                    {
                        response.Status = WsConstants.StatusSuccess;
                    }
                    else
                    {
                        response.Errors = new List<WsError> { new WsError { Code = res, Message = _lang.Text("ErrorProcessing") } };
                    }
                }
                else
                {
                    response.Errors = new List<WsError> { new WsError { Code = WsConstants.CodeStatusFail, Message = _lang.Text("ErrorProcessing") } };
                }
            }
            else
            {
                {
                    response.Errors = ModelState.Where(ms => ms.Value.Errors.Any()).Select(x => new { x.Key, x.Value.Errors }).Select(e => new WsError { Code = WsConstants.CodeBadRequest, Field = e.Key, Message = string.Join(", ", e.Errors.Select(s => s.ErrorMessage)) }).ToList();
                }
            }
            return response;
        }





    }
}
