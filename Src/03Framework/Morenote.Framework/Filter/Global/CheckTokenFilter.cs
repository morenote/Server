using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MoreNote.Common.Utils;
using MoreNote.Logic.Entity;
using MoreNote.Logic.Service;
using MoreNote.Models.DTO.Leanote;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Morenote.Framework.Filter.Global
{
    /// <summary>
    /// 检查用户Token是否登录
    /// </summary>
    public class CheckTokenFilter : Attribute, IAuthorizationFilter
    {
        public TokenSerivce tokenSerivce;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;

        public CheckTokenFilter(TokenSerivce tokenSerivce)
        {
            this.tokenSerivce = tokenSerivce;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var token = context.HttpContext.Session.GetString("token");

            if (token == null || string.IsNullOrEmpty(token))
            {
                token = context.HttpContext.Request.Query["token"];
            }
            if (token == null || string.IsNullOrEmpty(token))
            {
                if (context.HttpContext.Request.Form!=null && context.HttpContext.Request.Form.Any())
                {
                    token = context.HttpContext.Request.Form["token"];
                }
               
            }
            if (token == null || string.IsNullOrEmpty(token))
            {
                token = context.HttpContext.Request.Headers["token"];
            }
            if (token == null || string.IsNullOrEmpty(token))
            {
                token = context.HttpContext.Session.GetString("token");
            }
           
            if (token==null || string.IsNullOrEmpty(token) || !tokenSerivce.VerifyToken(token))
            {
                context.HttpContext.Session.Remove("token");
                //context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                ApiReDTO apiRe = new ApiReDTO()
                {
                    Ok = false,
                    Msg = "NOTLOGIN",
                };
                //return Json(apiRe, MyJsonConvert.GetOptions());
                context.Result = new JsonResult(apiRe,MyJsonConvert.GetSimpleOptions() );

                return;
            }
        }
    }
}