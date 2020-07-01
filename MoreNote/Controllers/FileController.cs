using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UpYunLibrary.OSS;

using Microsoft.AspNetCore.Mvc;

using MoreNote.Common.Config;
using MoreNote.Common.Utils;
using Microsoft.AspNetCore.Authorization;

namespace MoreNote.Controllers
{
    public class FileController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [Authorize(Roles = "Admin,SuperAdmin")]
        public IActionResult UploadUPyun()
        {
            var webConfig = ConfigManager.GetWebConfig();
            var options = new UPYunOSSOptions();
            options.bucket = webConfig.UpYunOSSConfig.Bucket;
            options.save_key = "/{year}/{mon}/{day}/{filemd5}{.suffix}";
            options.expiration = UnixTimeHelper.GetTimeStampInInt32() + 60;
            var policy = UpYunOSS.GetPolicy(options);
            var signature = UpYunOSS.GetSignature(policy, webConfig.UpYunOSSConfig.FormApiSecret);
            ViewBag.bucket = webConfig.UpYunOSSConfig.Bucket;
            ViewBag.policy = policy;
            ViewBag.signature = signature;
            return View();
        }


    }
}
