﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using MoreNote.Common.ExtensionMethods;
using MoreNote.Logic.Entity.ConfigFile;
using MoreNote.Logic.Service;
using MoreNote.Logic.Service.FileStoreService;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace MoreNote.Controllers.API.APIV1
{
    //[ApiController]
    public class FileAPIController : APIBaseController
    {
        public NoteService noteService;
        public WebSiteConfig webSiteConfig;

        public FileAPIController(AttachService attachService
             , TokenSerivce tokenSerivce
             , NoteFileService noteFileService
             , UserService userService
             , ConfigFileService configFileService
             , IHttpContextAccessor accessor,
            NoteService noteService) : base(attachService, tokenSerivce, noteFileService, userService, configFileService, accessor)
        {
            this.noteService = noteService;
            this.webSiteConfig = configFileService.WebConfig;
        }

        //经过格式化的URL,有助于CDN或者反向代码服务器缓存图片
        //api/File/GetImageForWeb/xxxxx   xxxx=xxx.jpg
        [Route("api/File/Images/{fileId}")]
        public Task<IActionResult> GetImageForWeb(string fileId)
        { 
            return GetImage(fileId);
        }

        /// <summary>
        /// 获取用户头像
        /// </summary>
        /// <param name="userIdHex"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        [Route("api/File/Avatars/{userIdHex}/{filename}")]
        public async Task<IActionResult> GetAvatar(string userIdHex, string filename)
        {
            var fileUrlPath = $"{userIdHex}/images/logo";
            var objectName = $"{fileUrlPath}/{filename}";
            var provider = new FileExtensionContentTypeProvider();

            string fileExt = Path.GetExtension(filename);
            var memi = provider.Mappings[fileExt];
            var fileService = FileStoreServiceFactory.Instance(webSiteConfig);
            try
            {
                var data = await fileService.GetObjecByteArraytAsync(webSiteConfig.MinIOConfig.NoteFileBucketName, objectName);
                return File(data, memi);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return Content("NotFound");
            }
        }

        //todo: 输出image 需要get参数
        //api/File/GetImage?fileId=xxxx
         [Route("api/File/GetImage")]
        public async Task<IActionResult> GetImage(string fileId)
        {
            //try
            //{
            //    if (string.IsNullOrEmpty(fileId)) return Content("error");
            //    var myFileId = fileId.ToLongByHex();
            //    var noteFile = noteFileService.GetFile(myFileId);
            //    if (noteFile == null)
            //        //return Content("NoFoundImage");
            //        return NoFoundImage();
            //    //获取又拍云操作对象
            //    UpYun upyun = new UpYun(config.UpyunConfig.UpyunBucket, config.UpyunConfig.UpyunUsername, config.UpyunConfig.UpyunPassword);
            //    upyun.secret = config.UpyunConfig.UpyunSecret;
            //    string path = noteFile.Path;
            //    int unixTimestamp = UnixTimeHelper.GetTimeStampInInt32();
            //    unixTimestamp += 15;
            //    string _upt = upyun.CreatToken(unixTimestamp.ToString(), upyun.secret, path);
            //    return Redirect($"https://upyun.morenote.top{path}?_upt={_upt}");
            //}
            //catch (Exception ex)
            //{
            //    return NoFoundImage();
            //}

            try
            {
                if (string.IsNullOrEmpty(fileId))
                {
                    return Content("error");
                }
                var myFileId = fileId.ToLongByHex();
                var noteFile = noteFileService.GetFile(myFileId);
                if (noteFile == null)
                    //return Content("NoFoundImage");
                    return NoFoundImage();
                //获取操作对象
                string fileExt = Path.GetExtension(noteFile.Name);
                var fileService = FileStoreServiceFactory.Instance(webSiteConfig);

                var objectName = $"{noteFile.UserId.ToHex()}/images/{noteFile.CreatedTime.ToString("yyyy")}/{noteFile.CreatedTime.ToString("MM")}/{noteFile.FileId.ToHex()}{Path.GetExtension(noteFile.Name)}";
                var provider = new FileExtensionContentTypeProvider();
                var memi = provider.Mappings[fileExt];
                var data = await fileService.GetObjecByteArraytAsync(webSiteConfig.MinIOConfig.NoteFileBucketName, objectName);

                return File(data, memi);
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
        public async Task<IActionResult> GetAttach(string fileId)
        {
            if (string.IsNullOrEmpty(fileId)) return Content("error");
            if (fileId.Length == 24) fileId = fileId.Substring(0, 16);
            var attachFile = attachService.GetAttach(fileId.ToLongByHex());
            if (attachFile == null) return Content("NoFoundAttach");

            if (string.IsNullOrEmpty(fileId)) return Content("error");
            var myFileId = fileId.ToLongByHex();
            var attachInfo = attachService.GetAttach(myFileId);
            if (attachInfo == null)
                //return Content("NoFoundImage");
                return NoFoundImage();
            //获取操作对象
            string fileExt = Path.GetExtension(attachInfo.Name);
            var fileService = FileStoreServiceFactory.Instance(webSiteConfig);

            var objectName = $"{attachInfo.UserId.ToHex()}/images/{attachInfo.CreatedTime.ToString("yyyy")}/{attachInfo.CreatedTime.ToString("MM")}/{attachInfo.AttachId.ToHex()}{Path.GetExtension(attachInfo.Name)}";
            var data = await fileService.GetObjecByteArraytAsync(webSiteConfig.MinIOConfig.NoteFileBucketName, objectName);
            var provider = new FileExtensionContentTypeProvider();
            var memi = provider.Mappings[fileExt];

            return File(data, memi);
        }

        //todo:下载所有附件
        public IActionResult GetAllAttachs(string noteId, string token)
        {
            return null;
        }
    }
}