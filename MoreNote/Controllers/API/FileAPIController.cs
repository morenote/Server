using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using MoreNote.Common.Util;
using MoreNote.Common.Utils;
using MoreNote.Logic.Service;
using System;
using System.IO;

namespace MoreNote.Controllers.API.APIV1
{
    [Route("api/File/[action]")]
    //[ApiController]
    public class FileAPIController : BaseAPIController
    {
        public FileAPIController(IHttpContextAccessor accessor) : base(accessor)
        {
        }

        //经过格式化的URL,有助于CDN或者反向代码服务器缓存图片
        //api/File/GetImageForWeb/xxxxx   xxxx=xxx.jpg
        [Route("api/File/GetImageForWeb/{fileId}")]
        public IActionResult GetImageForWeb(string fileId)
        {
            return  GetImage(fileId);
        }
        //todo: 输出image 需要get参数
        //api/File/GetImage?fileId=xxxx
        public IActionResult GetImage(string fileId)
        {
            try
            {
                if (string.IsNullOrEmpty(fileId))
                {
                    return Content("error");
                }
                if (fileId.Length == 24)
                {
                    fileId = fileId.Substring(0, 16);
                }
                var myFileId = MyConvert.HexToLong(fileId);
                var noteFile = FileService.GetFile(myFileId);
                if (noteFile == null)
                {
                    //return Content("NoFoundImage");
                    return NoFoundImage();
                }
                var stream = System.IO.File.OpenRead(noteFile.Path);
                string fileExt = Path.GetExtension(noteFile.Name);
                //获取文件的ContentType
                var provider = new FileExtensionContentTypeProvider();
                var memi = provider.Mappings[fileExt];
                return File(stream, memi);
            }catch(Exception ex)
            {
              return NoFoundImage();
            }
            
        }
        public IActionResult NoFoundImage()
        {

            string path;
            if (RuntimeEnvironment.Islinux)
            {
                //图片来自网络
                path = "upload/404.jpg";

            }
            else
            {
                path = "upload\\404.jpg";
            }
            var stream = System.IO.File.OpenRead(path);
            string fileExt = Path.GetExtension("404.jpg");
            //获取文件的ContentType
            var provider = new FileExtensionContentTypeProvider();
            var memi = provider.Mappings[fileExt];
            return File(stream, memi);
        }

        //todo:下载附件
        public IActionResult GetAttach(string fileId)
        {
           
            if (string.IsNullOrEmpty(fileId))
            {
                return Content("error");
            }
            if (fileId.Length == 24)
            {
                fileId = fileId.Substring(0, 16);
            }
            var attachFile = AttachService.GetAttach(MyConvert.HexToLong(fileId));
            if (attachFile == null)
            {
                return Content("NoFoundAttach");
            }
            var stream = System.IO.File.OpenRead(attachFile.Path);
            string fileExt = Path.GetExtension(attachFile.Name);
            //获取文件的ContentType
            var provider = new FileExtensionContentTypeProvider();
            var memi = provider.Mappings[fileExt];
            return File(stream, memi, Path.GetFileName(attachFile.Path));
        }
        //todo:下载所有附件
        public IActionResult GetAllAttachs(string noteId,string token)
        {
            return null;
        }
      
    }
}