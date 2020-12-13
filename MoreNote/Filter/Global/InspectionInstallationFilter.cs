﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using MoreNote.Logic.Entity.ConfigFile;
using MoreNote.Logic.Service;

namespace MoreNote.Filter.Global
{
    public class InspectionInstallationFilter : ActionFilterAttribute
    {
        public bool Disable { get; set; }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            WebSiteConfig config = ConfigFileService.GetWebConfig();
            if (Disable||config.IsAlreadyInstalled)
            {
                return;
            }
            var any = (filterContext.ActionDescriptor as ControllerActionDescriptor).MethodInfo.GetCustomAttributes(typeof(SkipInspectionInstallationFilter),false).Any();
            if (any)
            {
                return;
            }
            //未通过验证则跳转到无权限提示页
            RedirectResult content = new RedirectResult("/System/Install");
            filterContext.Result = content;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
           
        }
    }
}
