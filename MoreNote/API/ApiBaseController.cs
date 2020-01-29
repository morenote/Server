using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoreNote.Common.Utils;
using MoreNote.Logic.Entity;
using MoreNote.Logic.Service;

namespace MoreNote.API
{

    /**
     * 源代码基本是从GO代码直接复制过来的
     * 
     * 只是简单的实现了API的功能
     * 
     * 2020年01月27日
     * */
    public class ApiBaseController : Controller
    {
        public int pageSize = 1000;
        public string defaultSortField = "UpdatedTime";
        public string leanoteUserId = "admin" ;// 不能更改
        protected IHttpContextAccessor _accessor;
        public ApiBaseController(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }
        public IActionResult action()
        {
            return Content("error");
        }
        //todo:得到token, 这个token是在AuthInterceptor设到Session中的
        public  string GetToken()
        {
            /**
             *  软件从不假设某个IP或者使用者借助cookie获得永久的使用权
             *  任何访问，必须显式的提供token证明
             *  
             *  API服务不接受cookie中的信息，token总是显式提交的
             * 
             **/
            string token = _accessor.HttpContext.Request.Form["token"];
            if (string.IsNullOrEmpty(token))
            {
                token= _accessor.HttpContext.Request.Query["token"];
            }
            if (string.IsNullOrEmpty(token))
            {
                return "";
            }
            else
            {
                return token;
            }
           
        }
        public long GetUserIdBySession()
        {
            string userid_hex = _accessor.HttpContext.Session.GetString("userId");
            long userid_number = MyConvert.HexToLong(userid_hex);
            return userid_number;
        }
        // todo:得到用户信息
        public long getUserIdByToken(string token)
        {
          
            if (string.IsNullOrEmpty(token))
            {
              return 0;
            }
            else
            {
                User user = TokenSerivce.GetUserByToken(token);
                long userid = (user == null ? 0 : user.UserId);
                return userid;
            }
        }
        public User getUserByToken(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return null;
            }
            else
            {
                User user = TokenSerivce.GetUserByToken(token);
                return user;
            }
        }
        public  long getUserIdByToken()
        {
            string token=GetToken();
            if (string.IsNullOrEmpty(token))
            {
                string userid_hex= _accessor.HttpContext.Session.GetString("userId");
                long userid_number=MyConvert.HexToLong(userid_hex);
                return userid_number;
            }
            else
            {
                User user= TokenSerivce.GetUserByToken(token);
                long userid  = (user == null?0:user.UserId);
                return userid;
            }
        }
        public void SetUserIdToSession(long userId)
        {
            _accessor.HttpContext.Session.SetString("userId",userId.ToString("x"));
        }
        public  User getUserByToken()
        {
            string token = GetToken();
            if (string.IsNullOrEmpty(token))
            {
                return null;
            }
            else
            {
                User user = TokenSerivce.GetUserByToken(token);
                return user;
            }
        }

        public long ConvertUserIdToLong()
        {
            string hex = _accessor.HttpContext.Request.Form["userId"];
            if (string.IsNullOrEmpty(hex))
            {
                hex = _accessor.HttpContext.Request.Query["userId"];
            }
            if (string.IsNullOrEmpty(hex))
            {
                return 0;
            }
            return MyConvert.HexToLong(hex);
        }
        // todo :上传附件
        public IActionResult uploadAttach()
        {
            return null;
        }
  
        // todo :上传图片
        public IActionResult upload()
        {
            return null;
        }
    }
}