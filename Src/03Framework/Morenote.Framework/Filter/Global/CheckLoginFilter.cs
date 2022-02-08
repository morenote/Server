using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Morenote.Framework.Http;
using MoreNote.Logic.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Morenote.Framework.Filter.Global
{
    /// <summary>
    /// 检查用户是否登录
    /// </summary>
    public class CheckLoginFilter : Attribute, IAuthorizationFilter
    {
        public TokenSerivce tokenSerivce;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;
        public bool CheckVerified { get; set; }

        public CheckLoginFilter(TokenSerivce tokenSerivce)
        {
            this.tokenSerivce = tokenSerivce;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var userid = context.HttpContext.Session.GetString("UserId");
            var token = context.HttpContext.Session.GetString("token");

            if (token == null || string.IsNullOrEmpty(token))
            {
                context.Result = new RedirectResult("/Auth/Login");
                return;
            }
            //验证token
            if (!tokenSerivce.VerifyToken(token))
            {
                context.HttpContext.Session.Remove("token");
                context.Result = new RedirectResult("/Auth/Login");
                return;
            }
            if (string.IsNullOrEmpty(userid))
            {
                context.Result = new RedirectResult("/Auth/Login");
            }

            if (string.IsNullOrEmpty(userid))
            {
                context.Result = new RedirectResult("/Auth/Login");
            }

            if (CheckVerified)
            {
                var verified = _session.GetBool("Verified");
                if (verified == null || !verified.Value)
                {
                    context.Result = new RedirectResult("/Auth/Verify");
                }
            }
        }
    }
}