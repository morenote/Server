using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MoreNote.Value;
using Microsoft.AspNetCore.Http;
using MoreNote.Common.Utils;
using MoreNote.Logic.Entity;
using MoreNote.Logic.Entity.ConfigFile;
using System.Text.Json;
using System.IO;
using MoreNote.Logic.Service;
using MoreNote.Framework.Controllers;
using Morenote.Framework.Filter.Global;

namespace MoreNote.Controllers
{
    public class SystemController : BaseController
    {
        private ConfigFileService configFileService;
        public SystemController(AttachService attachService
            , TokenSerivce tokenSerivce
            , NoteFileService noteFileService
            , UserService userService
            , ConfigFileService configFileService
            , IHttpContextAccessor accessor) : base(attachService, tokenSerivce, noteFileService, userService, configFileService, accessor)
        {
            this.configFileService = configFileService;

        }
        [SkipInspectionInstallationFilter]
        public IActionResult Install()
        {
            WebSiteConfig webSiteConfig=configFileService.WebConfig;
            if(webSiteConfig!=null&&webSiteConfig.IsAlreadyInstalled)
            {
                  string path=RuntimeEnvironment.IsWindows?@"C:\morenote\WebSiteConfig.json":"/morenote/WebSiteConfig.json";
                return Content($"请设置{path}的IsAlreadyInstalled变量为false");
            }
            ViewBag.config=string.Empty;
       

            ViewBag.Title = "网站初始化向导";
             SetLocale();
            return View();
        } 

         [SkipInspectionInstallationFilter]
        public IActionResult DoInstall(string captcha,string config)
        {
            WebSiteConfig localWebSiteConfig=configFileService.WebConfig;
            string path=RuntimeEnvironment.IsWindows?@"C:\morenote\WebSiteConfig.json":"/morenote/WebSiteConfig.json";
            if(localWebSiteConfig!=null&&localWebSiteConfig.IsAlreadyInstalled)
            {
                 ResponseMessage re = new ResponseMessage() { Ok = false, Msg = $"请设置{path}的IsAlreadyInstalled变量为false" };
                return Json(re, MyJsonConvert.GetSimpleOptions());
            }
            string verifyCode = HttpContext.Session.GetString("VerifyCode");
            int? verifyCodeValid= HttpContext.Session.GetInt32("VerifyCodeValid");
            int time = HttpContext.Session.GetInt32("VerifyCodeTime").GetValueOrDefault(0);
            int valid = HttpContext.Session.GetInt32("VerifyCodeValid").GetValueOrDefault(0);
            if (valid != 1 || !UnixTimeUtil.IsValid(time, 2000))
            {
                ResponseMessage re = new ResponseMessage() { Ok = false, Msg = "验证码过期或失效" };
                return Json(re, MyJsonConvert.GetSimpleOptions());
            }
            //销毁验证码的标志
            HttpContext.Session.SetInt32("VerifyCodeValid", 0);
            if (string.IsNullOrEmpty(verifyCode) || string.IsNullOrEmpty(captcha)||verifyCodeValid==null||verifyCodeValid==0)
            {
                ResponseMessage re = new ResponseMessage() { Ok = false, Msg = "错误参数" };
                return Json(re, MyJsonConvert.GetSimpleOptions());
            }
            else
            {       ResponseMessage re = new ResponseMessage() { Ok = true };
                    WebSiteConfig webSiteConfig=JsonSerializer.Deserialize<WebSiteConfig>(config);
                    //检查配置文件
                    if (webSiteConfig.PostgreSql==null)
                    {
                          re = new ResponseMessage() { Ok = false, Msg = "PostgreSql错误参数" };
                         return Json(re, MyJsonConvert.GetSimpleOptions());
                    }
                    configFileService.Save(webSiteConfig,ConfigFileService.GetConfigPath());
                    //登录成功
                    re = new ResponseMessage() { Ok = true };
                    return Json(re, MyJsonConvert.GetSimpleOptions());
            }
        }
        public IActionResult InstallSuccess()
        {
            return View();
        }
    }
}
