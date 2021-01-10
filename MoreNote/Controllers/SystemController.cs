using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MoreNote.Filter.Global;
using MoreNote.Value;
using Microsoft.AspNetCore.Http;
using MoreNote.Common.Utils;
using MoreNote.Logic.Entity;
using MoreNote.Logic.Entity.ConfigFile;
using System.Text.Json;
using System.IO;
using MoreNote.Common.Utils;
using MoreNote.Logic.Service;

namespace MoreNote.Controllers
{
    public class SystemController : BaseController
    {
        private ConfigFileService configFileService;
        public SystemController(DependencyInjectionService dependencyInjectionService) : base( dependencyInjectionService)
        {
            this.configFileService = dependencyInjectionService.ServiceProvider.GetService(typeof(ConfigFileService))as ConfigFileService;

        }
        [SkipInspectionInstallationFilter]
        public IActionResult Install()
        {
            WebSiteConfig webSiteConfig=configFileService.GetWebConfig();
            if(webSiteConfig!=null&&webSiteConfig.IsAlreadyInstalled)
            {
                  string path=RuntimeEnvironment.IsWindows?@"C:\morenote\WebSiteConfig.json":"/morenote/WebSiteConfig.json";
                return Content($"请设置{path}的IsAlreadyInstalled变量为false");
            }
            ViewBag.config=string.Empty;
            if (webSiteConfig!=null)
            {
                 string json=JsonSerializer.Serialize(webSiteConfig);
                ViewBag.config=json;
            }
            ViewBag.Title = "网站初始化向导";
            ViewBag.msg = LanguageResource.GetMsg();
            return View();
        } 

         [SkipInspectionInstallationFilter]
        public IActionResult DoInstall(string captcha,string config)
        {
            WebSiteConfig localWebSiteConfig=configFileService.GetWebConfig();
            string path=RuntimeEnvironment.IsWindows?@"C:\morenote\WebSiteConfig.json":"/morenote/WebSiteConfig.json";
            if(localWebSiteConfig!=null&&localWebSiteConfig.IsAlreadyInstalled)
            {
                 Re re = new Re() { Ok = false, Msg = $"请设置{path}的IsAlreadyInstalled变量为false" };
                return Json(re, MyJsonConvert.GetSimpleOptions());
            }
            string verifyCode = HttpContext.Session.GetString("VerifyCode");
            int? verifyCodeValid= HttpContext.Session.GetInt32("VerifyCodeValid");
            int time = HttpContext.Session.GetInt32("VerifyCodeTime").GetValueOrDefault(0);
            int valid = HttpContext.Session.GetInt32("VerifyCodeValid").GetValueOrDefault(0);
            if (valid != 1 || !UnixTimeHelper.IsValid(time, 2000))
            {
                Re re = new Re() { Ok = false, Msg = "验证码过期或失效" };
                return Json(re, MyJsonConvert.GetSimpleOptions());
            }
            //销毁验证码的标志
            HttpContext.Session.SetInt32("VerifyCodeValid", 0);
            if (string.IsNullOrEmpty(verifyCode) || string.IsNullOrEmpty(captcha)||verifyCodeValid==null||verifyCodeValid==0)
            {
                Re re = new Re() { Ok = false, Msg = "错误参数" };
                return Json(re, MyJsonConvert.GetSimpleOptions());
            }
            else
            {       Re re = new Re() { Ok = true };
                    WebSiteConfig webSiteConfig=JsonSerializer.Deserialize<WebSiteConfig>(config);
                    //检查配置文件
                    if (webSiteConfig.PostgreSql==null)
                    {
                          re = new Re() { Ok = false, Msg = "PostgreSql错误参数" };
                         return Json(re, MyJsonConvert.GetSimpleOptions());
                    }
                    configFileService.Save(webSiteConfig,ConfigFileService.GetConfigPath());
                    //登录成功
                    re = new Re() { Ok = true };
                    return Json(re, MyJsonConvert.GetSimpleOptions());
            }
        }
        public IActionResult InstallSuccess()
        {
            return View();
        }
    }
}
