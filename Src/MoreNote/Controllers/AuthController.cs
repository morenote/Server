using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Tokens;
using Morenote.Framework.Http;
using MoreNote.Common.ExtensionMethods;
using MoreNote.Common.Utils;
using MoreNote.Framework.Controllers;
using MoreNote.Logic.Entity;
using MoreNote.Logic.Entity.ConfigFile;
using MoreNote.Logic.Model;
using MoreNote.Logic.Service;
using MoreNote.Logic.Service.Logging;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Controllers
{
    public class AuthController : BaseController
    {
        private AuthService authService;
        private ConfigService configService;
        public WebSiteConfig config;
        private IDistributedCache distributedCache;

        private  readonly string errorCountKey="doLoginErrorCount";
        public AuthController(AttachService attachService
            , TokenSerivce tokenSerivce
            , NoteFileService noteFileService
            , UserService userService
            , ConfigFileService configFileService
            ,IDistributedCache distributedCache
            
            , IHttpContextAccessor accessor,
            AuthService authService, 
            ConfigService configService,
            ILoggingService loggingService) : 
            base(attachService, tokenSerivce, noteFileService, userService, configFileService, accessor,loggingService)
        {
            this.distributedCache = distributedCache;
            this.authService = authService;
            this.configService = configService;
            this.config = configService.config;
        }

        public IActionResult QuickLogin()
        {
            var token = Request.Cookies["token"];
            var user = tokenSerivce.GetUserByToken(token);
            if (user != null && tokenSerivce.VerifyToken(token))
            {
                HttpContext.Session.SetString("UserId", user.UserId.ToHex24());
                HttpContext.Session.SetBool("Verified", user.Verified);
                HttpContext.Session.SetString("token", token);
                //anti csrf
                HttpContext.Response.Cookies.Append("token", token, 
                    new CookieOptions() { HttpOnly = true,
                        Domain=config.APPConfig.Domain,
                        SameSite=SameSiteMode.Lax,
                        Secure=true, 
                        MaxAge = TimeSpan.FromDays(30) });
            }
            else
            {
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return Content("Authentication failed, please login with password or clear cookie");
            }
            return Redirect("/member");
        }

        //public IActionResult Index()
        //{
        //    return View();
        //}
        /// <summary>
        /// 登录 页面
        /// </summary>
        /// <returns></returns>
        public IActionResult Login()
        {
            var number=distributedCache.GetInt(errorCountKey);
            ViewBag.Title = "请登录";
            SetLocale();
            ConfigSetting configSetting = new ConfigSetting();
            ViewBag.quickLogin = Request.Cookies["token"] != null;
            ViewBag.ConfigSetting = configSetting;
            //是否需要验证码服务
            if (this.config.SecurityConfig.NeedVerificationCode == NeedVerificationCode.OFF
                ||((this.config.SecurityConfig.NeedVerificationCode == NeedVerificationCode.AUTO)&&number<10))
            {

                ViewBag.needCaptcha="false";
            }
            else
            {
                 ViewBag.needCaptcha="true";
            }
            ViewBag.errorCount=number;
           
            return View();
        }

        //public IActionResult DoLogin(string email, string pwd, string captcha)
        public async Task<IActionResult> DoLogin(string email, string pwd, string captcha)
        {
            var number=distributedCache.GetInt(errorCountKey);
            //是否需要验证码
            if (this.config.SecurityConfig.NeedVerificationCode == NeedVerificationCode.ON||
                (this.config.SecurityConfig.NeedVerificationCode == NeedVerificationCode.AUTO)&&number>10)
            {
                string errorMessage = string.Empty;
                //检查验证码是否一样
                if (!CheckVerifyCode(captcha, out errorMessage))
                {
                    ResponseMessage re = new ResponseMessage() { Ok = false, Msg = errorMessage };
                    return Json(re, MyJsonConvert.GetSimpleOptions());
                }
            }

            if (!authService.LoginByPWD(email, pwd, out string token, out User user))
            {
                //登录失败
                ResponseMessage re = new ResponseMessage() { Ok = false, Msg = "wrongUsernameOrPassword" };
             
                number++;
                distributedCache.SetInt(errorCountKey,number);
                return Json(re, MyJsonConvert.GetSimpleOptions());
            }
            else
            {
                 distributedCache.SetInt(errorCountKey,0);
                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                identity.AddClaim(new Claim(ClaimTypes.Sid, user.UserId.ToString()));
                identity.AddClaim(new Claim(ClaimTypes.Name, user.Username));

                if (!string.IsNullOrEmpty(user.Role))
                {
                    identity.AddClaim(new Claim(ClaimTypes.Role, user.Role));//角色 用户组
                }

              
                if (user.Jurisdiction != null && user.Jurisdiction.Any())
                {
                    foreach (var item in user.Jurisdiction)
                    {
                        identity.AddClaim(new Claim(item.AuthorizationType, item.AuthorizationValue));//授权

                    }
                }
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTime.Now.AddDays(1)
                }).ConfigureAwait(false);
                //var token=   tokenSerivce.GenerateToken();

                //登录成功
                HttpContext.Session.SetString("token", token);
                HttpContext.Session.SetString("UserId", user.UserId.ToHex24());
                HttpContext.Session.SetBool("Verified", user.Verified);

                HttpContext.Response.Cookies.Append("token", token, 
                    new CookieOptions() { HttpOnly = true,
                    Domain=config.APPConfig.Domain,
                    SameSite=SameSiteMode.Lax,
                    Secure=true, 
                    MaxAge = TimeSpan.FromDays(30) });

                ResponseMessage re = new ResponseMessage() { Ok = true };

               
                var jti= idGenerator.NextId();
                //签署JWT
                var claims = new List<Claim>()
                {
                    new Claim(JwtRegisteredClaimNames.Jti, jti.ToHex()),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Sid, user.UserId.ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Sub, "{B362F518-1C49-437B-962B-8D83A0A0285E}"),
                };
                //网站密钥
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.SecurityConfig.Secret));

                var jwtToken = new JwtSecurityToken(
                   
                    issuer: config.APPConfig.SiteUrl,
                    audience: config.APPConfig.SiteUrl,
                    claims: claims,
                    notBefore: DateTime.Now,
                    
                    expires: DateTime.Now.AddYears(100),

                    signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)

                );
                var jwt = new JwtSecurityTokenHandler().WriteToken(jwtToken);
                re.Payload=jwt;
                return Json(re, MyJsonConvert.GetSimpleOptions());
            }
        }

        public IActionResult Register(string iu, string from)
        {
            if (iu == null)
            {
                iu = "";
            }

            if (from == null)
            {
                from = "";
            }

            //return Content("An API listing authors of docs.asp.net.");
            ViewBag.title = "leanote";
            SetLocale();

            ViewBag.iu = iu;
            ViewBag.from = from;
            ViewBag.openRegister = configService.IsOpenRegister();

            return View();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="email">注册者的电子邮件</param>
        /// <param name="pwd">注册者的口令</param>
        /// <param name="iu"></param>
        /// <returns></returns>
        public JsonResult DoRegister(string email, string pwd, string iu, string captcha)
        {
            if (!configFileService.WebConfig.SecurityConfig.OpenRegister)
            {
                return Json(new ApiRe()
                {
                    Ok = false,
                    Msg = "管理员已经将注册功能关闭"
                }, MyJsonConvert.GetSimpleOptions());
            }
            string errorMessage = string.Empty;
            bool result = authService.Register(email, pwd, iu.ToLongByHex(), out errorMessage);
            if (result)
            {
                return Json(new ApiRe()
                {
                    Ok = true,
                    Msg = "Success"
                }, MyJsonConvert.GetSimpleOptions());
            }
            else
            {
                return Json(new ApiRe()
                {
                    Ok = false,
                    Msg = $"注册失败:{errorMessage}"
                }, MyJsonConvert.GetSimpleOptions());
            }
        }

        /// <summary>
        /// 登出并删除登录信息
        /// </summary>
        /// <returns></returns>
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("UserId");
            HttpContext.Session.Remove("Verified");
            HttpContext.Session.Remove("token");
            HttpContext.Response.Cookies.Delete("token");


           
            return Redirect("/member");
        }
    }
}