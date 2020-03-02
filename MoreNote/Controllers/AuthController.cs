using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoreNote.Common.Utils;
using MoreNote.Logic.Entity;
using MoreNote.Logic.Service;
using MoreNote.Value;
using System.Collections.Generic;

namespace MoreNote.Controllers
{
    public class AuthController : Controller
    {
        //public IActionResult Index()
        //{
        //    return View();
        //}
        public IActionResult Login()
        {
            ViewBag.msg = LanguageResource.GetMsg();
            return View();
        }
        public IActionResult DoLogin(string email,string pwd,string captcha)
        {
          
            string verifyCode= HttpContext.Session.GetString("VerifyCode");
          
            if (string.IsNullOrEmpty(verifyCode)||string.IsNullOrEmpty(captcha))
            {
               
                Re re = new Re() { Ok=false,Msg="错误参数"};
                return Json(re,MyJsonConvert.GetSimpleOptions());

            }
            else
            {
                if (!captcha.ToLower().Equals(verifyCode))
                {
                   
                    Re re = new Re() { Ok = false, Msg = "验证码错误" };
                    return Json(re, MyJsonConvert.GetSimpleOptions());
                }
                string token;
                User user;
                if (!AuthService.LoginByPWD(email, pwd, out token, out user))
                {
                    //登录失败
                    Re re = new Re() { Ok = false, Msg = "wrongUsernameOrPassword" };
                    return Json(re, MyJsonConvert.GetSimpleOptions());
                }
                else
                {
                    //登录成功
                    HttpContext.Session.SetString("_token", token);
                    HttpContext.Session.SetString("_userId", user.UserId.ToString("x"));
                    Re re = new Re() { Ok = true };
                    return Json(re, MyJsonConvert.GetSimpleOptions());

                }
              

                return Content("11");
            }
           

        }

        public IActionResult Register(string iu, string from)
        {
            if (iu == null) iu = "";
            if (from == null) from = "";


            //return Content("An API listing authors of docs.asp.net.");
            ViewBag.title = "leanote";
            Dictionary<string, string> msg = LanguageResource.GetMsg();
            ViewBag.msg = msg;

            ViewBag.iu = iu;
            ViewBag.from = from;
            ViewBag.openRegister = ConfigService.IsOpenRegister();

            return View();
        }

        public JsonResult DoRegister(string email, string pwd, string iu)
        {
            if (!ConfigService.IsOpenRegister())
            {
                return Json(new ApiRe()
                {
                    Ok = false,
                    Msg = "管理员已经将注册功能关闭"
                }, MyJsonConvert.GetSimpleOptions());
            }
            bool result = AuthService.Register(email, pwd, MyConvert.HexToLong(iu));
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
                },MyJsonConvert.GetSimpleOptions());
            }

        }
    }
}