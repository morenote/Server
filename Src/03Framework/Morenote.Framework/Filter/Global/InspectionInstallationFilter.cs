using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using MoreNote.Config.ConfigFile;
using MoreNote.Logic.Service;

namespace Morenote.Framework.Filter.Global
{
    public class InspectionInstallationFilter : ActionFilterAttribute
    {
        public InspectionInstallationFilter(ConfigFileService configFileService)
        {
            this.configFileService= configFileService;
            config = configFileService.WebConfig;

        }
        public bool Disable { get; set; }
        private WebSiteConfig config;
        private ConfigFileService configFileService;
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
           
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
