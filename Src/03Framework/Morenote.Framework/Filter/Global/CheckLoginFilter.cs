using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Morenote.Framework.Http;
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
         private readonly IHttpContextAccessor _httpContextAccessor;
         private ISession _session => _httpContextAccessor.HttpContext.Session;
        public bool CheckVerified{get;set;}

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var userid=context.HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userid))
            {
                 context.Result=new RedirectResult("/Auth/Login");
            }
            if (CheckVerified)
            {
                var verified= _session.GetBool("Verified");
                if (verified==null|| !verified.Value)
                {
                    context.Result=new RedirectResult("/Auth/Verify");
                }
            }
           
        }
    }
}
