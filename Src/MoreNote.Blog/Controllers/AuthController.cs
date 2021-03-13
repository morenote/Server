using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using MoreNote.Common.ExtensionMethods;
using MoreNote.Common.Utils;
using MoreNote.Logic.Entity;
using MoreNote.Logic.Service;
using MoreNote.Value;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MoreNote.Filter.Global;
using MoreNote.Logic.Model;

namespace MoreNote.Controllers
{
 
    public class AuthController : BaseController
    {
        private AuthService authService;
        private ConfigService configService;

        public AuthController(AttachService attachService
            , TokenSerivce tokenSerivce
            , NoteFileService noteFileService
            , UserService userService
            , ConfigFileService configFileService
            , IHttpContextAccessor accessor, AuthService authService, ConfigService configService) : base(attachService, tokenSerivce, noteFileService, userService, configFileService, accessor)
        {
            this.authService = authService;
            this.configService = configService;

        }
        //public IActionResult Index()
        //{
        //    return View();
        //}
      
       public IActionResult Login()
        {
            ViewBag.Title = "请登录";
            ViewBag.msg = LanguageResource.GetMsg();
            ConfigSetting configSetting = new ConfigSetting();
           
            ViewBag.ConfigSetting = configSetting;
            return View();
        }

        //public IActionResult DoLogin(string email, string pwd, string captcha)
        public async Task<IActionResult> DoLogin(string email, string pwd, string captcha)
        {
            string verifyCode = HttpContext.Session.GetString("VerifyCode");
            int time = HttpContext.Session.GetInt32("VerifyCodeTime").GetValueOrDefault(0);
            int valid = HttpContext.Session.GetInt32("VerifyCodeValid").GetValueOrDefault(0);
            if (valid != 1 || !UnixTimeHelper.IsValid(time, 15))
            {
                Re re = new Re() { Ok = false, Msg = "验证码过期或失效" };
                return Json(re, MyJsonConvert.GetSimpleOptions());
            }
            //销毁验证码的标志
            HttpContext.Session.SetInt32("VerifyCodeValid", 0);
            if (string.IsNullOrEmpty(verifyCode) || string.IsNullOrEmpty(captcha))
            {
                Re re = new Re() { Ok = false, Msg = "错误参数" };
                return Json(re, MyJsonConvert.GetSimpleOptions());
            }
            else
            {
                if (captcha.Equals("0")||!captcha.ToLower().Equals(verifyCode))
                {
                    Re re = new Re() { Ok = false, Msg = "验证码错误" };
                    return Json(re, MyJsonConvert.GetSimpleOptions());
                }
                if (!authService.LoginByPWD(email, pwd, out string token, out User user))
                {
                    //登录失败
                    Re re = new Re() { Ok = false, Msg = "wrongUsernameOrPassword" };
                    return Json(re, MyJsonConvert.GetSimpleOptions());
                }
                else
                {
                    var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                    identity.AddClaim(new Claim(ClaimTypes.Sid, user.UserId.ToString()));
                    identity.AddClaim(new Claim(ClaimTypes.Name, user.Username));
                    identity.AddClaim(new Claim(ClaimTypes.Name, user.Username));
                    if (!string.IsNullOrEmpty(user.Role))
                    {
                        identity.AddClaim(new Claim(ClaimTypes.Role, user.Role));//角色 用户组
                    }
                    if (user.Jurisdiction!=null&&user.Jurisdiction.Any())
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

                    //登录成功
                    HttpContext.Session.SetString("_token", token);
                    HttpContext.Session.SetString("_UserId", user.UserId.ToHex24());
                    Re re = new Re() { Ok = true };
                    return Json(re, MyJsonConvert.GetSimpleOptions());
                }
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
            Dictionary<string, string> msg = LanguageResource.GetMsg();
            ViewBag.msg = msg;

            ViewBag.iu = iu;
            ViewBag.from = from;
            ViewBag.openRegister = configService.IsOpenRegister();

            return View();
        }

        public JsonResult DoRegister(string email, string pwd, string iu)
        {
            if (!configService.IsOpenRegister())
            {
                return Json(new ApiRe()
                {
                    Ok = false,
                    Msg = "管理员已经将注册功能关闭"
                }, MyJsonConvert.GetSimpleOptions());
            }
            bool result = authService.Register(email, pwd,iu.ToLongByHex());
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
                    Msg = "注册失败"
                }, MyJsonConvert.GetSimpleOptions());
            }
        }
    }
}