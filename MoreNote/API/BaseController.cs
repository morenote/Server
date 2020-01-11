using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MoreNote.API
{
    public class BaseController : Controller
    {
        //todo:得到token, 这个token是在AuthInterceptor设到Session中的
        public IActionResult getToken()
        {
            return Content("");
        }
        // todo:得到用户信息
        public IActionResult getUserId()
        {
            return null;
        }
        // todo :上传附件
        public IActionResult uploadAttach()
        {
            return null;
        }
        // todo :上传图片
        public IActionResult upload()
        {
            return null;
        }
    }
}