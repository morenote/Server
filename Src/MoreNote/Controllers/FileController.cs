using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UpYunLibrary.OSS;

using Microsoft.AspNetCore.Mvc;

using MoreNote.Common.Utils;
using Microsoft.AspNetCore.Authorization;
using MoreNote.Logic.Service;
using Microsoft.AspNetCore.Http;
using MoreNote.Framework.Controllers;

namespace MoreNote.Controllers
{
    public class FileController : BaseController
    {
        private ConfigFileService configFileService;
        public FileController(AttachService attachService
            , TokenSerivce tokenSerivce
            , NoteFileService noteFileService
            , UserService userService
            , ConfigFileService configFileService
            , IHttpContextAccessor accessor) : base(attachService, tokenSerivce, noteFileService, userService, configFileService, accessor)
        {
           
           this.configFileService= configFileService;
            ;
        }
        public IActionResult Index()
        {
            return View();
        }
        [Authorize(Roles = "Admin,SuperAdmin")]
        public IActionResult UploadUPyun()
        {
            var webConfig = configFileService.WebConfig;
            var options = new UPYunOSSOptions();
            options.bucket = webConfig.UpyunConfig.UpyunBucket;
            options.save_key = "/{year}/{mon}/{day}/{filemd5}{.suffix}";
            options.expiration = UnixTimeHelper.GetTimeStampInInt32() + 60;
            var policy = UpYunOSS.GetPolicy(options);
            var signature = UpYunOSS.GetSignature(policy, webConfig.UpyunConfig.FormApiSecret);
            ViewBag.bucket = webConfig.UpyunConfig.UpyunBucket;
            ViewBag.policy = policy;
            ViewBag.signature = signature;
            return View();
        }


    }
}
