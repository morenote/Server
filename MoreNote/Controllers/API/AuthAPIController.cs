using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoreNote.Common.ExtensionMethods;
using MoreNote.Common.Utils;
using MoreNote.Logic.Entity;
using MoreNote.Logic.Service;
using System.Text.Json;

namespace MoreNote.Controllers.API.APIV1
{
    [Route("api/Auth/[action]")]

    public class AuthAPIController : BaseAPIController
    {
        public AuthAPIController(IHttpContextAccessor accessor) : base(accessor)
        {

        }

        /// <summary>
        /// 登陆
        ///  成功返回 {Ok: true, Item: token }
        ///  失败返回 {Ok: false, Msg: ""}
        /// </summary>
        /// <param name="email"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
       //[HttpPost]
        public JsonResult Login(string email, string pwd)
        {
            string tokenStr = "";
            User user;
            if (AuthService.LoginByPWD(email, pwd, out tokenStr, out user))
            {
                SetUserIdToSession(user.UserId);
                 AuthOk authOk = new AuthOk()
                {
                    Ok = true,
                    Token = tokenStr,
                    UserId = user.UserId.ToHex24(),
                    Email = user.Email,
                    Username = user.Username
                };
                return Json(authOk, MyJsonConvert.GetSimpleOptions());
            }
            else
            {
                ApiRe apiRe = new ApiRe()
                {
                    Ok = false,
                    Msg = "用户名或密码有误"
                };
                string json = JsonSerializer.Serialize(apiRe, MyJsonConvert.GetSimpleOptions());
                return Json(apiRe, MyJsonConvert.GetSimpleOptions());
            }
        }
        //todo:注销函数
        public JsonResult Logout()
        {
            //ex:API当前不使用cookie和session判断用户身份，
            //API调用必须显式的提供token字段，以证明身份


            ApiRe apiRe = new ApiRe()
            {
                Ok = false,
                Msg = "未提供注销功能"
            };
            return Json(apiRe, MyJsonConvert.GetSimpleOptions());
        }
        //todo:注册
        public JsonResult Register(string email,string pwd)
        {
            //ex:API当前不使用cookie和session判断用户身份，
            //API调用必须显式的提供token字段，以证明身份
            ApiRe apiRe;
            if (AuthService.Register(email,pwd,0))
            {
                apiRe = new ApiRe()
                {
                    Ok = true,
                    Msg = "注册成功"
                };
            }
            else
            {
                apiRe = new ApiRe()
                {
                    Ok = false,
                    Msg = "注册失败"
                };
            }
            return Json(apiRe, MyJsonConvert.GetSimpleOptions());
        }
    }
}