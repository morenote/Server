using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using MoreNote.Common.Utils;
using MoreNote.Logic.Entity;
using MoreNote.Logic.Service;

namespace MoreNote.LeaNoteAPI
{
    [Route("api/[controller]/[action]")]

    public class AuthController : Controller
    {
        /// <summary>
        /// 登陆
        ///  成功返回 {Ok: true, Item: token }
        ///  失败返回 {Ok: false, Msg: ""}
        /// </summary>
        /// <param name="email"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Login(string email, string pwd)
        {
            string tokenStr = "";
            User user;
            if (AuthService.LoginByPWD(email, pwd, out tokenStr,out user))
            {
                AuthOk authOk = new AuthOk()
                {
                    Ok = true,
                    Token = tokenStr,
                    UserId = user.UserId.ToString("x"),
                    Email = user.Email,
                    Username = user.UsernameRaw
                };
                return Json(authOk,MyJsonConvert.GetSimpleOptions());

            }
            else
            {
                ApiRe apiRe = new ApiRe()
                {
                    Ok = false,
                    Msg = "用户名或密码有误"

                };
                string json = JsonSerializer.Serialize(apiRe,MyJsonConvert.GetSimpleOptions());
                return Json(apiRe, MyJsonConvert.GetSimpleOptions());
            }
           
        }
        //todo:注销函数
        public IActionResult Logout()
        {
            return null;
        }
        //todo:注册
        public IActionResult Register()
        {
            return null;
        }
    }
}