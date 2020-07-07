using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoreNote.Common.Utils;
using MoreNote.Logic.Entity;
using MoreNote.Logic.Service;

namespace MoreNote.Controllers.API.APIV1
{
    [Route("API/APPStore/[action]")]
    public class ApiV1APPStoreController : ApiV1BaseController
    {
        public ApiV1APPStoreController(IHttpContextAccessor accessor) : base(accessor)
        {


        }

        [Route("/api/1.0/app/[action]")]
        public IActionResult getlist()
        {

            /**
             *  String json = "";
             return Content(json);
             * */
            AppInfo[] appInfos = APPStoreInfoService.GetAPPList();
            APPStoreInfo aPPStoreInfoList = new APPStoreInfo()
            {
                resp_data = new Resp_Data()
                {
                    app_list = appInfos
                }
            };
            
            return Json(aPPStoreInfoList,MyJsonConvert.GetOptions());
           
        }
        [Route("/api/1.0/app/update/callback/[action]")]
        public IActionResult callback()
        {

            return Content("callback");
        }
        [Route("/api/1.0/app/[action]")]
        [HttpPost]
        public IActionResult AddAPP(long appid,string appautor ,string appdetail,string appname,string apppackage,
            string appdownurl,string applogourl,string appversion,string imglist,long appsize,bool agreement,string password)
        {
            if (!password.Equals("9qMDpc4anxbckAFX47LIz7uaqpubicKSZMyd47RSbO3D7kgR51ui3V5dLFPDu7WS"))
            {
                return Content("管理员密码错误");
            }
            if (string.IsNullOrEmpty(appautor))
            {
                return Content("appautor 不能为空");
            }
            if (string.IsNullOrEmpty(appdetail))
            {
                return Content("appdetail 不能为空");
            }
            if (string.IsNullOrEmpty(appname))
            {
                return Content("appname 不能为空");
            }
            if (string.IsNullOrEmpty(apppackage))
            {
                return Content("apppackage 不能为空");
            }
            if (string.IsNullOrEmpty(appdownurl))
            {
                return Content("appdownurl 不能为空");
            }
            if (string.IsNullOrEmpty(applogourl))
            {
                return Content("applogourl 不能为空");
            }
            if (string.IsNullOrEmpty(appversion))
            {
                return Content("appversion 不能为空");
            }
            if (string.IsNullOrEmpty(imglist))
            {
                return Content("imglist 不能为空");
            }
            if (appsize==0)
            {
                return Content("appsize 不能为空");
            }
            if (!agreement)
            {
                return Content("你没有同意协议");
            }
            AppInfo appInfo = new AppInfo()
            {
                appid = SnowFlakeNet.GenerateSnowFlakeID(),
                appautor = appautor,
                appdetail = appdetail,
                appname = appname,
                apppackage = apppackage,
                appdownurl = appdownurl,
                applogourl = applogourl,
                appversion = appversion,
                imglist = imglist.Split("\n"),
                appsize = appsize.ToString()
            };
            if (APPStoreInfoService.AddAPP(appInfo))
            {
                return Content("发布成功，APP id="+ appInfo.appid);

            }
            else
            {
                return Content("发布成功，APP id=" + appInfo.appid);

            }
            
        }
    }
}