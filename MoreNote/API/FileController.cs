using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using MoreNote.Common.Utils;
using MoreNote.Logic.Service;
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
            if (string.IsNullOrEmpty(fileId))
            {
                return Content("error");
            }
            var file=FileService.GetFile(MyConvert.HexToLong(fileId));
            var stream = System.IO.File.OpenRead(file.Path);
            string fileExt = Path.GetExtension(file.Name);
            //获取文件的ContentType
            var provider = new FileExtensionContentTypeProvider();
            var memi = provider.Mappings[fileExt];
            return File(stream, memi, Path.GetFileName(file.Path));
        }
        //todo:下载附件
        public IActionResult GetAttach(string fileId)
        {
            if (string.IsNullOrEmpty(fileId))
            {
                return Content("error");
            }
            var file = AttachService.GetAttach(MyConvert.HexToLong(fileId));
            var stream = System.IO.File.OpenRead(file.Path);
            string fileExt = Path.GetExtension(file.Name);
            //获取文件的ContentType
            var provider = new FileExtensionContentTypeProvider();
            var memi = provider.Mappings[fileExt];
            return File(stream, memi, Path.GetFileName(file.Path));
        }
        //todo:下载所有附件
        public IActionResult GetAllAttachs(string noteId,string token)
        {
            return null;
        }
      
    }
}