using System;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using MoreNote.Common.ExtensionMethods;
using MoreNote.Common.Utils;
using MoreNote.Logic.Entity.ConfigFile;
using MoreNote.Logic.Service;
using UpYunLibrary;

namespace MoreNote.Controllers.API.APIV1
{
    [Route("api/File/[action]")]
    //[ApiController]
    public class FileAPIController : APIBaseController
    {



        public NoteService noteService;
      
       public FileAPIController(AttachService attachService
            , TokenSerivce tokenSerivce
            , NoteFileService noteFileService
            , UserService userService
            , ConfigFileService configFileService
            , IHttpContextAccessor accessor,
           NoteService noteService) : base(attachService, tokenSerivce, noteFileService, userService, configFileService, accessor)
       {
           this.noteService = noteService;
       }

        //经过格式化的URL,有助于CDN或者反向代码服务器缓存图片
        //api/File/GetImageForWeb/xxxxx   xxxx=xxx.jpg
        [Route("api/File/GetImageForWeb/{fileId}")]
        public IActionResult GetImageForWeb(string fileId)
        {
            return GetImage(fileId);
        }

        //todo: 输出image 需要get参数
        //api/File/GetImage?fileId=xxxx
        public IActionResult GetImage(string fileId)
        {
            try
            {
                if (string.IsNullOrEmpty(fileId)) return Content("error");
                var myFileId = fileId.ToLongByHex();
                var noteFile = noteFileService.GetFile(myFileId);
                if (noteFile == null)
                    //return Content("NoFoundImage");
                    return NoFoundImage();
                //获取又拍云操作对象
                UpYun upyun = new UpYun(config.UpYunCDN.UpyunBucket, config.UpYunCDN.UpyunUsername, config.UpYunCDN.UpyunPassword);
                upyun.secret = config.UpYunCDN.UpyunSecret; 
                string path = noteFile.Path;
                int unixTimestamp = UnixTimeHelper.GetTimeStampInInt32();
                unixTimestamp += 15;
                string _upt = upyun.CreatToken(unixTimestamp.ToString(), upyun.secret, path);
                return Redirect($"https://upyun.morenote.top{path}?_upt={_upt}");
            }
            catch (Exception ex)
            {
                return NoFoundImage();
            }
        }

        public IActionResult NoFoundImage()
        {
            return Redirect($"https://upyun.morenote.top/404.jpg");
        }

        //todo:下载附件
        public IActionResult GetAttach(string fileId)
        {
            if (string.IsNullOrEmpty(fileId)) return Content("error");
            if (fileId.Length == 24) fileId = fileId.Substring(0, 16);
            var attachFile = attachService.GetAttach(fileId.ToLongByHex());
            if (attachFile == null) return Content("NoFoundAttach");
            
            //获取又拍云操作对象
            UpYun upyun = new UpYun(config.UpYunCDN.UpyunBucket, config.UpYunCDN.UpyunUsername, config.UpYunCDN.UpyunPassword);
            upyun.secret = config.UpYunCDN.UpyunSecret; 
            string path = attachFile.Path;
            long? unixTimestamp = UnixTimeHelper.GetTimeStampInLong();
            unixTimestamp = unixTimestamp + 5;
            string _upt = upyun.CreatToken(unixTimestamp.ToString(), upyun.secret, path);
            return Redirect($"https://upyun.morenote.top{path}?_upt={_upt}");
        }

        //todo:下载所有附件
        public IActionResult GetAllAttachs(string noteId, string token)
        {
            return null;
        }
    }
}