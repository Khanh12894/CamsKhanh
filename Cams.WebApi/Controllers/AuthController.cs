using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using XichLip.WebApi.Base;
using XichLip.WebApi.Constants;
using XichLip.WebApi.Interfaces;
using XichLip.WebApi.Models.Base;
using XichLip.WebApi.Models.SmsOTP;
using XichLip.WebApi.Models.User;
using XichLip.WebApi.Models.UserTokens;
using XichLip.WebApi.Utilities;
using WorkSimple.Infrastructure;
using WorkSimple.Infrastructure.Services;
using WorkSimple.Infrastructure.Utils;
using WsResource = XichLip.WebApi.Resources.WsResource;

namespace XichLip.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public partial class AuthController : BaseApiController
    {
        private readonly UserManager<UserModel> _userManager;

        private IConfiguration _config;
        private IUserRepositories _repo;
        private IUserStore<UserModel> _repoIdentity;
        private WsLanguage<WsResource> _lang;
        private readonly ILogger<AuthController> _logger;
        private readonly IEmailSender _emailSender;
        private readonly ITokenService _tokenService;
        private IPasswordHasher<UserModel> _passwordHasher;

        public AuthController(
            IConfiguration config,
            IUserRepositories repo,
            IUserStore<UserModel> repoIdentity,
            UserManager<UserModel> userManager,
            WsLanguage<WsResource> lang,
            ITokenService tokenService,
            ILogger<AuthController> logger,
            IPasswordHasher<UserModel> passwordHasher,
            IEmailSender emailSender
            )
        {
            _config = config;
            _repo = repo;
            _repoIdentity = repoIdentity;
            _logger = logger;
            _userManager = userManager;
            _lang = lang;
            _tokenService = tokenService;
            _passwordHasher = passwordHasher;
            _emailSender = emailSender;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<WsResponse> Login([FromBody] UserSSOModel model)
        {
            var response = new WsResponse();
            try
            {
                //_redisCache.StringSet("Test_Key", "Test Value",);
                if (ModelState.IsValid)
                {
                    var user = await _repo.FindByNameAsync(model.UserName);
                    if (user != null)
                    {
                        //if (user.IsLock != true)
                        //{
                        //if (user.Activated != null && user.Activated.Value)
                        //{
                        var hashPassword = _passwordHasher.HashPassword(user, model.Password);
                        var validPassword = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, model.Password);
                        if (validPassword == PasswordVerificationResult.Success || validPassword == PasswordVerificationResult.SuccessRehashNeeded)
                        {

                            var now = DateTime.UtcNow;

                            var claims = new Claim[]
                            {
                                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                                //new Claim(JwtRegisteredClaimNames.Email, user.Email),
                                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                                new Claim(JwtRegisteredClaimNames.Iat, now.ToUniversalTime().ToString(), ClaimValueTypes.Integer64),
                                    new Claim("wsid", user.UserId.ToString())
                            };

                            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_config["Audience:Secret"]));
                            var tokenValidationParameters = new TokenValidationParameters
                            {
                                ValidateIssuerSigningKey = true,
                                IssuerSigningKey = signingKey,
                                ValidateIssuer = true,
                                ValidIssuer = _config["Audience:Iss"],
                                ValidateAudience = true,
                                ValidAudience = _config["Audience:Aud"],
                                ValidateLifetime = true,
                                ClockSkew = TimeSpan.Zero,
                                RequireExpirationTime = true
                            };
                            var expired = ParseData.GetInt(_config["Audience:Expired"]) ?? 60;
                            var jwt = new JwtSecurityToken(
                                issuer: _config["Audience:Iss"],
                                audience: _config["Audience:Aud"],
                                claims: claims,
                                notBefore: now,
                                expires: now.Add(TimeSpan.FromMinutes(expired)),
                                signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
                            );
                            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

                            //var token = _tokenService.GenerateAccessToken(claims);
                            var newRefreshToken = _tokenService.GenerateRefreshToken();
                            //TODO: save refresh token
                            await _repo.SaveRefreshToken(new UserTokensModel() { UserId = user.UserId, Name = user.UserName, RefreshToken = newRefreshToken });
                            response.Status = WsConstants.StatusSuccess;

                            response.Data = new
                            {
                                token = encodedJwt,
                                refreshToken = newRefreshToken
                            };
                            return response;
                        }
                        else
                        {

                            response.Status = WsConstants.Statusfail;
                            //response.Message = string.Join(Environment.NewLine, ModelState.Values.SelectMany(v => v.Errors).Select(modelError => modelError.ErrorMessage).ToList());
                            response.Errors = new System.Collections.Generic.List<WsError>() {
                                new WsError { Code= WsConstants.CodeStatusFail,Field="Password", Message=_lang.Text("PasswordMismatch") }
                            };
                            response.Data = null;
                            return response;

                        }
                    }
                    else
                    {

                        response.Status = WsConstants.Statusfail;
                        //response.Message = string.Join(Environment.NewLine, ModelState.Values.SelectMany(v => v.Errors).Select(modelError => modelError.ErrorMessage).ToList());
                        response.Errors = new System.Collections.Generic.List<WsError>() {
                                new WsError { Code= WsConstants.CodeStatusFail,Field="UserName", Message=_lang.Text("AuthenticationFailed") }
                            };
                        response.Data = null;
                        return response;

                    }

                }
                else
                {
                    response.Status = WsConstants.Statusfail;
                    response.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => new WsError { Code = WsConstants.CodeBadRequest, Message = e.ErrorMessage }).ToList();
                    response.Data = null;
                    return response;
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(Utils.GetExceptionLog(ex));
                response.Status = WsConstants.Statusfail;
                response.Data = null;
                return response;
            }

        }
        
        [HttpGet("IsExists")]
        [AllowAnonymous]
        public async Task<WsResponse> IsExists(string userName)
        {
            var response = new WsResponse();
            var userModel = await _repo.FindByNameAsync(userName);
            if (userModel != null && userModel.UserId > 0)
            {
                response.Data = true;
            }
            else
            {
                response.Data = false;
            }
            return response;
        }

        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<WsResponse> Register(UserRegisterModel model)
        {
            var response = new WsResponse();
            try
            {

                if (ModelState.IsValid)
                {
                    var user = await _repo.FindByNameAsync(model.UserName);
                    if (user == null || user.UserId <= 0)
                    {
                        var hashPassword = _passwordHasher.HashPassword(user, model.Password);
                        user = new UserModel
                        {
                            UserName = model.UserName,
                            NormalizedUserName = model.UserName.ToUpper(),
                            PasswordHash = hashPassword,
                            //FirstName = model.FirstName,
                            //LastName = model.LastName,
                            //SsoIdentity = model.SsoIdentity,
                            //UserType = model.UserType,
                            Email = model.Email,
                            SecurityStamp = Guid.NewGuid().ToString(),
                            //OTP = model.OTP,
                            //Activated = false//model.UserName.Contains("@") == true ? false : true

                        };
                        var createStatus = await _repo.Register(user);


                        //if (user.UserId > 0)
                        //{
                        //    response.Data = user.UserId;
                        //    if (model.Email != "" && model.Email != null)
                        //    {

                        //        var isSendEmail = ParseData.GetBool(_config["AppSettings:SendEmail"]);
                        //        string mailgui = _config["EmailSender:UserName"];
                        //        string passgui = _config["EmailSender:Password"];
                        //        if (isSendEmail)
                        //        {

                        //            //    Console.WriteLine("Mail To");//mail nhận
                        //            MailAddress to = new MailAddress(model.Email);
                        //            //   Console.WriteLine("Mail From");//gửi từ
                        //            MailAddress from = new MailAddress(mailgui);
                        //            MailMessage mail = new MailMessage(from, to);
                        //            mail.IsBodyHtml = true;
                        //            mail.Subject = "XÁC NHẬN ĐĂNG KÝ TÀI KHOẢN";

                        //            mail.Body = "<p>Kích hoạt tài khoản Quản lý giáo dục </p>" +
                        //                   "Người dùng Nhập mã OTP: " + model.OTP + "<br/>" +
                        //                   "Trân trọng!<br/><br/>" +
                        //                   "<b>Công ty Cổ phần Giáo dục Bình Đẳng</b><br/>" +
                        //                   "Địa chỉ: Tầng 7, Tòa nhà Ladeco, 266 Đội Cấn, Ba Đình, Hà Nội<br/>" +
                        //                   "Điện thoại: 0247 303 1889 - Hotline: 1900 638 332 ";

                        //            SmtpClient smtp = new SmtpClient();
                        //            smtp.Host = "smtp.gmail.com";
                        //            smtp.Port = 587;

                        //            smtp.Credentials = new NetworkCredential(
                        //                mailgui, passgui);
                        //            smtp.EnableSsl = true;
                        //            smtp.Send(mail);

                        //        }
                        //        else
                        //        {

                        //            int saveCode = _repo.ForgotPassword(user.UserId, "123456").Result;
                        //            if (saveCode < 0)
                        //            {
                        //                response.Status = WsConstants.Statusfail;
                        //                response.Errors = new List<WsError> { new WsError { Code = WsConstants.CodeStatusFail, Message = _lang.Text("ErrorProcessing") } };
                        //            }
                        //        }
                        //    }
                        //    else// send otp
                        //    {
                        //        // send otp
                        //        string NoiDung = "LiberEduVn xin thong bao: Ma OTP cua quy khach la " + model.OTP + ", truy cap website https://liber.edu.vn de biet them thong tin chi tiet.";
                        //        var sendOTPStatus = SendOTP_SMS(user, NoiDung).Result;
                        //        if (sendOTPStatus.Status && sendOTPStatus.IsSent)
                        //        {
                        //            response.Status = WsConstants.StatusSuccess;
                        //            //response.Message = ResourceMessage.successSendOTP;
                        //            response.Status = WsConstants.StatusSuccess;
                        //        }
                        //        else if (!sendOTPStatus.Status && !sendOTPStatus.IsSent)
                        //        {
                        //            //response.Message = ResourceMessage.errorSendOTPTimeAvailabled;
                        //            response.Status = WsConstants.Statusfail;
                        //            response.Data = sendOTPStatus.Data;//
                        //        }
                        //        else
                        //        {
                        //            //response.Message = ResourceMessage.errorSendOTPFailed;
                        //            response.Status = WsConstants.Statusfail;
                        //            response.Data = sendOTPStatus.Data;//

                        //        }


                        //    }

                        //}
                        //else
                        //{
                        //    response.Data = 0;
                        //}
                    }
                }
                else
                {
                    response.Status = WsConstants.Statusfail;
                    response.Errors = ModelState.Where(ms => ms.Value.Errors.Any()).Select(x => new { x.Key, x.Value.Errors }).Select(e => new WsError { Code = WsConstants.CodeBadRequest, Field = e.Key, Message = string.Join(", ", e.Errors.Select(s => s.ErrorMessage)) }).ToList();
                }


            }
            catch (Exception ex)
            {
                string s = ex.InnerException.ToString();
            }
            return response;
        }


        [HttpPost("CheckOTP")]
        [AllowAnonymous]
        public async Task<WsResponse> CheckOTP(UserOTPModel model)
        {
            var response = new WsResponse();
            if (ModelState.IsValid)
            {

                var createStatus = await _repo.CheckOTP(model);
                if (createStatus > 0)
                {
                    response.Data = createStatus;

                }
                else
                {
                    response.Data = 0;
                }

            }
            else
            {
                response.Status = WsConstants.Statusfail;
                response.Errors = ModelState.Where(ms => ms.Value.Errors.Any()).Select(x => new { x.Key, x.Value.Errors }).Select(e => new WsError { Code = WsConstants.CodeBadRequest, Field = e.Key, Message = string.Join(", ", e.Errors.Select(s => s.ErrorMessage)) }).ToList();
            }
            return response;
        }


        //[HttpGet("SendOTP")]
        //[AllowAnonymous]
        //public async Task<WsResponse> SendOTP(string username)
        //{
        //    var response = new WsResponse();
        //    var user = await _repo.FindByNameAsync(username);
        //    string OTP = Utils.RandomString(6, true);
        //    if (user != null)
        //    {
        //        //send otp vào email
        //        if (username.Contains("@") == true)
        //        {

        //            var isSendEmail = ParseData.GetBool(_config["AppSettings:SendEmail"]);
        //            string mailgui = _config["EmailSender:UserName"];
        //            string passgui = _config["EmailSender:Password"];
        //            if (isSendEmail)
        //            {

        //                //    Console.WriteLine("Mail To");//mail nhận
        //                MailAddress to = new MailAddress(username);
        //                //   Console.WriteLine("Mail From");//gửi từ
        //                MailAddress from = new MailAddress(mailgui);
        //                MailMessage mail = new MailMessage(from, to);
        //                mail.IsBodyHtml = true;
        //                mail.Subject = "XÁC NHẬN ĐĂNG KÝ TÀI KHOẢN";

        //                mail.Body = "<p>Kích hoạt tài khoản Quản lý giáo dục </p>" +
        //                       "Người dùng Nhập mã OTP:" + OTP + "<br/>" +
        //                       "Trân trọng!<br/><br/>" +
        //                       "<b>Nhà xuất bản Giáo dục Việt Nam</b><br/>" +
        //                       "Địa chỉ: Số 81 Trần Hưng Đạo, Hoàn Kiếm, Hà Nội<br/>" +
        //                       "Điện thoại: 024.38220801 - Fax: 024.39422010.";

        //                SmtpClient smtp = new SmtpClient();
        //                smtp.Host = "smtp.gmail.com";
        //                smtp.Port = 587;

        //                smtp.Credentials = new NetworkCredential(
        //                    mailgui, passgui);
        //                smtp.EnableSsl = true;
        //                smtp.Send(mail);
        //                UserModel item = new UserModel();
        //                item.UserName = username;
        //                item.OTP = OTP;
        //                var update = _repo.UpdateOTP(item);
        //                response.Status = WsConstants.StatusSuccess;
        //                response.Data = user.UserId;


        //            }
        //        }
        //        else// gửi vào sdt

        //        {
        //            // send otp
        //            string NoiDung = "LiberEduVn xin thong bao: Ma OTP cua quy khach la " + OTP + ", truy cap website https://liber.edu.vn de biet them thong tin chi tiet.";
        //            var sendOTPStatus = SendOTP_SMS(user, NoiDung).Result;
        //            if (sendOTPStatus.Status && sendOTPStatus.IsSent)
        //            {
        //                response.Status = WsConstants.StatusSuccess;
        //                //response.Message = ResourceMessage.successSendOTP;
        //                response.Status = WsConstants.StatusSuccess;
        //                UserModel item = new UserModel();
        //                item.UserName = username;
        //                item.OTP = OTP;
        //                var update = _repo.UpdateOTP(item);
        //                response.Status = WsConstants.StatusSuccess;
        //                response.Data = user.UserId;
        //            }
        //            else if (!sendOTPStatus.Status && !sendOTPStatus.IsSent)
        //            {
        //                //response.Message = ResourceMessage.errorSendOTPTimeAvailabled;
        //                response.Status = WsConstants.Statusfail;
        //                response.Data = sendOTPStatus.Data;//

        //            }
        //            else
        //            {
        //                //response.Message = ResourceMessage.errorSendOTPFailed;
        //                response.Status = WsConstants.Statusfail;
        //                response.Data = sendOTPStatus.Data;//

        //            }
        //        }


        //    }
        //    else
        //    {
        //        response.Status = WsConstants.Statusfail;
        //        response.Errors = new List<WsError>() {
        //                        new WsError { Code= WsConstants.CodeStatusFail,Field=username.Contains("@") == true?"Email":"PhoneNumber", Message=username.Contains("@") == true?"UserName không tồn tại":"Số điện thoại không tồn tại" }};
        //        response.Data = 0;
        //        return response;
        //    }
        //    return response;
        //}


        private Task SendEmailChangePassword(string email, string subject, string body)
        {
            _emailSender.SendEmailAsync(email, subject, body);
            return Task.FromResult(1);
        }

        [AllowAnonymous]
        [HttpPost("GetUserByEmail")]
        public async Task<WsResponse> GetUserByEmail(string email)
        {
            var response = new WsResponse();
            response.Status = WsConstants.Statusfail;
            var user = _repo.GetUserByEmail(email).Result;

            if (user != null && user.UserId > 0)
            {
                response.Status = WsConstants.StatusSuccess;
                response.Data = user.UserId;

            }
            else
            {

                int saveCode = _repo.ForgotPassword(user.UserId, "123456").Result;
                if (saveCode < 0)
                {
                    response.Status = WsConstants.Statusfail;
                    response.Errors = new List<WsError> { new WsError { Code = WsConstants.CodeStatusFail, Message = _lang.Text("ErrorProcessing") } };
                }
            }


            return response;
        }

        [AllowAnonymous]
        [HttpPost("Forgot-Password")]
        public async Task<WsResponse> ForgotPassword([FromBody] ForgotPasswordModel model)
        {
            var response = new WsResponse();
            response.Status = WsConstants.Statusfail;
            var user = _repo.GetUserByEmail(model.Email).Result;

            if (user != null && user.UserId > 0)
            {
                if (user.Activated != null && user.Activated.Value)
                {

                    response.Status = WsConstants.StatusSuccess;
                    var isSendEmail = ParseData.GetBool(_config["AppSettings:SendEmail"]);


                    string mailgui = _config["EmailSender:UserName"];
                    string passgui = _config["EmailSender:Password"];
                    if (isSendEmail)
                    {


                        string code = Utils.RandomString(20, false);
                        int saveCode = _repo.ForgotPassword(user.UserId, code).Result;
                        if (saveCode != null && saveCode >= 0)
                        {


                            //    Console.WriteLine("Mail To");//mail nhận
                            MailAddress to = new MailAddress(model.Email);
                            //   Console.WriteLine("Mail From");//gửi từ
                            MailAddress from = new MailAddress(mailgui);
                            MailMessage mail = new MailMessage(from, to);
                            mail.IsBodyHtml = true;
                            mail.Subject = "LẤY LẠI MẬT KHÂU TÀI KHOẢN";

                            mail.Body = "<p>Lấy lại mật khẩu tài khoản Quản lý giáo dục:" + model.Email + " </p>" +
                                   "Thầy/cô vui lòng nháy chuột tại đây để đặt lại mật khẩu: <a href=" + model.Url + "?code=" + code + "&userid=" + user.UserId + ">Lấy lại mật khẩu</a><br/>" +
                                   "Trân trọng!<br/><br/>" +
                                   "<b>Nhà xuất bản Giáo dục Việt Nam</b><br/>" +
                                   "Địa chỉ: Số 81 Trần Hưng Đạo, Hoàn Kiếm, Hà Nội<br/>" +
                                   "Điện thoại: 024.38220801 - Fax: 024.39422010.";

                            SmtpClient smtp = new SmtpClient();
                            smtp.Host = "smtp.gmail.com";
                            smtp.Port = 587;

                            smtp.Credentials = new NetworkCredential(
                                mailgui, passgui);
                            smtp.EnableSsl = true;
                            smtp.Send(mail);
                        }
                        else
                        {
                            response.Status = WsConstants.Statusfail;
                            response.Errors = new List<WsError> { new WsError { Code = WsConstants.CodeStatusFail, Message = _lang.Text("ErrorProcessing") } };
                        }

                    }
                    else
                    {

                        int saveCode = _repo.ForgotPassword(user.UserId, "123456").Result;

                        if (saveCode < 0)
                        {
                            response.Status = WsConstants.Statusfail;
                            response.Errors = new List<WsError> { new WsError { Code = WsConstants.CodeStatusFail, Message = _lang.Text("ErrorProcessing") } };
                        }
                    }
                }
                else
                {
                    response.Status = WsConstants.Statusfail;
                    response.Errors = new List<WsError>() {
                    new WsError { Code= WsConstants.CodeStatusFail,Field="Acctivated", Message=_lang.Text("Tài khoản "+model.Email+" chưa được kích hoạt. Vui lòng nhập mã kích hoạt tài khoản!") }
                    };
                    return response;
                }
            }
            else
            {
                response.Status = WsConstants.Statusfail;
                response.Errors = new List<WsError> { new WsError { Code = WsConstants.CodeStatusFail, Message = _lang.Text("NonExistsUser") } };
            }
            return response;
        }


        [AllowAnonymous]

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] UserChangePasswordModel model)
        {
            var response = new WsResponse();
            if (ModelState.IsValid)
            {
                string userName = "";
                if (User.Identity.IsAuthenticated)
                {
                    userName = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                }
                else
                {
                    response.Status = WsConstants.Statusfail;
                    response.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => new WsError { Code = WsConstants.CodeBadRequest, Message = _lang.Text("access_denied") }).ToList();
                    response.Data = null;
                    return Ok(response);
                }
                var user = await _userManager.FindByNameAsync(userName);

                if (user != null)
                {

                    var result2 = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.Password);
                    if (result2.Succeeded)
                    {
                        var now = DateTime.UtcNow;

                        var claims = new Claim[]
                        {
                                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                                //new Claim(JwtRegisteredClaimNames.Email, user.Email),
                                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                                new Claim(JwtRegisteredClaimNames.Iat, now.ToUniversalTime().ToString(), ClaimValueTypes.Integer64),
                                    new Claim("wsid", user.UserId.ToString())
                        };

                        var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_config["Audience:Secret"]));
                        var tokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = signingKey,
                            ValidateIssuer = true,
                            ValidIssuer = _config["Audience:Iss"],
                            ValidateAudience = true,
                            ValidAudience = _config["Audience:Aud"],
                            ValidateLifetime = true,
                            ClockSkew = TimeSpan.Zero,
                            RequireExpirationTime = true
                        };
                        var expired = ParseData.GetInt(_config["Audience:Expired"]) ?? 60;
                        var jwt = new JwtSecurityToken(
                            issuer: _config["Audience:Iss"],
                            audience: _config["Audience:Aud"],
                            claims: claims,
                            notBefore: now,
                            expires: now.Add(TimeSpan.FromMinutes(expired)),
                            signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
                        );
                        var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
                        var newRefreshToken = _tokenService.GenerateRefreshToken();
                        //await _repo.SaveRefreshToken(new Models.UserTokens.UserTokensModel() { UserId = user.UserId, Name = userName, RefreshToken = newRefreshToken });


                        response.Status = WsConstants.StatusSuccess;

                        response.Data = new
                        {
                            accessToken = encodedJwt,
                            refreshToken = newRefreshToken
                        };
                        return Ok(response);

                    }
                    else
                    {
                        response.Status = WsConstants.Statusfail;
                        response.Errors = result2.Errors.Select(e => new WsError { Code = WsConstants.CodeBadRequest, Field = e.Code, Message = e.Description }).ToList();
                        response.Data = null;
                        return Ok(response);
                    }

                }
            }
            response.Status = WsConstants.Statusfail;
            response.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => new WsError { Code = WsConstants.CodeBadRequest, Message = e.ErrorMessage }).ToList();
            response.Data = null;
            return Ok(response);
        }


        [AllowAnonymous]
        [HttpPost("ResetPassword")]
        public async Task<WsResponse> ResetPassword([FromBody] UserForgotPasswordModel model)
        {
            var response = new WsResponse();
            response.Status = WsConstants.Statusfail;
            if (ModelState.IsValid)
            {
                var user = _repo.GetUserById(model.UserId).Result;
                if (user != null && user.UserId > 0)
                {
                    var checkCode = _repo.CheckCodeForgotPassword(user.UserId, model.Code).Result;
                    if (checkCode == 1)
                    {
                        var result = _repo.ChangeNewPassword(user.UserId, model.Password).Result;
                        if (result != null)
                        {
                            response.Status = WsConstants.StatusSuccess;
                            response.Data = result;



                            return response;
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
                        response.Errors = new List<WsError> { new WsError { Code = WsConstants.CodeStatusFail, Message = _lang.Text("Invalid_Code") } };
                    }
                }
                else
                {
                    response.Status = WsConstants.Statusfail;
                    response.Errors = new List<WsError> { new WsError { Code = WsConstants.CodeStatusFail, Message = _lang.Text("NonExistsUser") } };
                }
            }
            return response;
        }


        [AllowAnonymous]
        [HttpPost("ResetPasswordNotCode")]
        public async Task<WsResponse> ResetPasswordNotCode([FromBody] UserForgotPasswordModel model)
        {
            var response = new WsResponse();
            response.Status = WsConstants.Statusfail;
            if (ModelState.IsValid)
            {
                var user = _repo.GetUserById(model.UserId).Result;
                if (user != null && user.UserId > 0)
                {

                    var result = _repo.ChangeNewPassword(user.UserId, model.Password).Result;
                    if (result != null)
                    {
                        response.Status = WsConstants.StatusSuccess;
                        response.Data = result;
                        return response;
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
                    response.Errors = new List<WsError> { new WsError { Code = WsConstants.CodeStatusFail, Message = _lang.Text("NonExistsUser") } };
                }
            }
            return response;
        }

        [AllowAnonymous]
        [HttpPost("GetEmailByUserId")]
        public async Task<WsResponse> GetEmailByUserId([FromBody] UserModel model)
        {
            var response = new WsResponse();
            response.Status = WsConstants.Statusfail;
            var user = _repo.GetEmailByUserId(model).Result;

            if (user != null && user.UserId > 0)
            {
                response.Status = WsConstants.StatusSuccess;
                response.Data = user.Email;

            }
            else
            {
                response.Status = WsConstants.Statusfail;
                response.Errors = new List<WsError> { new WsError { Code = WsConstants.CodeStatusFail, Message = _lang.Text("ErrorProcessing") } };

            }


            return response;
        }
        [AllowAnonymous]
        [HttpPost("GetAdminType")]
        public async Task<WsResponse> GetAdminType([FromBody] UserModel model)
        {
            var response = new WsResponse();
            response.Status = WsConstants.Statusfail;
            var user = _repo.GetEmailByUserId(model).Result;

            if (user != null && user.UserId > 0)
            {
                response.Status = WsConstants.StatusSuccess;
                response.Data = user.AdminTypeId;

            }
            else
            {
                response.Status = WsConstants.Statusfail;
                response.Errors = new List<WsError> { new WsError { Code = WsConstants.CodeStatusFail, Message = _lang.Text("ErrorProcessing") } };

            }


            return response;
        }

        [HttpPost("List")]
        [AllowAnonymous]
        public async Task<WsResponse> List([FromBody] UserRequestModel model)
        {
            WsResponse response = new WsResponse();

            if (ModelState.IsValid)
            {
                var res = _repo.GetList(CurrentUserId).Result;
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
            }
            else
            {
                response.Status = WsConstants.Statusfail;
                response.Errors = ModelState.Where(ms => ms.Value.Errors.Any()).Select(x => new { x.Key, x.Value.Errors }).Select(e => new WsError { Code = WsConstants.CodeBadRequest, Field = e.Key, Message = string.Join(", ", e.Errors.Select(s => s.ErrorMessage)) }).ToList();
            }
            return response;
        }

    }

    public class Audience
    {
        public string Secret { get; set; }
        public string Iss { get; set; }
        public string Aud { get; set; }
    }
}
