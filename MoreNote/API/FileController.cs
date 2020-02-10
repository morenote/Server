using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using MoreNote.Common.Util;
using MoreNote.Common.Utils;
using MoreNote.Logic.Service;
using System;
using System.IO;

namespace MoreNote.API
{
    [Route("api/[controller]/[action]")]
    //[ApiController]
    public class FileController : ApiBaseController
    {
        public FileController(IHttpContextAccessor accessor) : base(accessor)
        {
        }

        //todo: 输出image
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
                    return Content("NoFoundImage");
                }
                var stream = System.IO.File.OpenRead(noteFile.Path);
                string fileExt = Path.GetExtension(noteFile.Name);
                //获取文件的ContentType
                var provider = new FileExtensionContentTypeProvider();
                var memi = provider.Mappings[fileExt];
                return File(stream, memi);
            }catch(Exception ex)
            {
              return  NOFound();
            }
            
        }
        public IActionResult NOFound()
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