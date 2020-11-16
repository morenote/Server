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

namespace MoreNote.Controllers
{
    public class FileController : BaseController
    {
        public FileController(IHttpContextAccessor accessor) : base(accessor)
        {
        }
        public IActionResult Index()
        {
            return View();
        }
        [Authorize(Roles = "Admin,SuperAdmin")]
        public IActionResult UploadUPyun()
        {
            var webConfig = ConfigFileService.GetWebConfig();
            var options = new UPYunOSSOptions();
            options.bucket = webConfig.UpYunOSS.Bucket;
            options.save_key = "/{year}/{mon}/{day}/{filemd5}{.suffix}";
            options.expiration = UnixTimeHelper.GetTimeStampInInt32() + 60;
            var policy = UpYunOSS.GetPolicy(options);
            var signature = UpYunOSS.GetSignature(policy, webConfig.UpYunOSS.FormApiSecret);
            ViewBag.bucket = webConfig.UpYunOSS.Bucket;
            ViewBag.policy = policy;
            ViewBag.signature = signature;
            return View();
        }


    }
}
