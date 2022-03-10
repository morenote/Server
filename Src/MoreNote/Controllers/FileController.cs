using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoreNote.Common.ExtensionMethods;
using MoreNote.Common.Utils;
using MoreNote.Framework.Controllers;
using MoreNote.Logic.Entity;
using MoreNote.Logic.Service;
using MoreNote.Logic.Service.FileStoreService;
using MoreNote.Logic.Service.Logging;

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UpYunLibrary.OSS;

namespace MoreNote.Controllers
{
    
    public class FileController : BaseController
    {
        private ConfigFileService configFileService;
        private NoteService noteService;
        public FileController(AttachService attachService
            , TokenSerivce tokenSerivce
            , NoteFileService noteFileService
            , UserService userService
            , ConfigFileService configFileService
            ,NoteService noteService
            , IHttpContextAccessor accessor
           , ILoggingService loggingService) :
            base(attachService, tokenSerivce, noteFileService, userService, configFileService, accessor, loggingService)
        {
            this.configFileService = configFileService;
            this.noteService=noteService;
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
            options.expiration = UnixTimeUtil.GetTimeStampInInt32() + 60;
            var policy = UpYunOSS.GetPolicy(options);
            var signature = UpYunOSS.GetSignature(policy, webConfig.UpyunConfig.FormApiSecret);
            ViewBag.bucket = webConfig.UpyunConfig.UpyunBucket;
            ViewBag.policy = policy;

            ViewBag.signature = signature;
            return View();
        }
        public async Task<IActionResult> PasteImage(string noteId)
        {
            var id=noteId.ToLongByHex();
            var re=await uploadImage("pasteImage",null);
            if (id!=null)
            {
                var userid=GetUserIdBySession();
                var note=noteService.GetNoteById(id);
                if (note.UserId!=null)
                {
                    var noteUserId=note.UserId;
                    if (noteUserId!=userid)
                    {
                        //todo:支持共享编辑
                        re.Ok=false;

                    }

                }


            }
            return Json(re,MyJsonConvert.GetLeanoteOptions());

        }

        public async Task<IActionResult> UploadAvatar()
        {
            var re=await uploadImage("logo",null);

            if (re.Ok)
            {
                re.Ok=userService.UpdateAvatar(GetUserIdBySession(),re.Id);
                if (re.Ok)
                {
                    UpdateSession("Logo",re.Id);
                }
            }

            return Json(re,MyJsonConvert.GetSimpleOptions());


        }


        private async System.Threading.Tasks.Task<ResponseMessage> uploadImage(string from, long? albumId)
        {
            var fileUrlPath = string.Empty;
            long? fileId = SnowFlakeNet.GenerateSnowFlakeID();
            var resultCode = 0;  // 1表示正常
            var resultMsg = "error";
            var Ok = false;
           
            var re = new ResponseMessage()
            {
                Id = fileId.ToHex24(),
                Code = resultCode,
                Msg = resultMsg,
                Ok = Ok
            };
            var httpFiles = _accessor.HttpContext.Request.Form.Files;
            if (httpFiles == null || httpFiles.Count < 1)
            {
                return re;
            }
            var httpFile = httpFiles["file"];
            if (httpFile == null || httpFile.Length == 0)
            {
                return re;
            }

            var userid = GetUserIdBySession();
            var nowTime = DateTime.Now;
            if (from.Equals("logo") || from.Equals("blogLogo"))
            {
                fileUrlPath = $"{userid.ToHex()}/images/logo";
            }
            else
            {
                fileUrlPath = $"{userid.ToHex()}/images/{ nowTime.ToString("yyyy")}/{nowTime.ToString("MM")}";
            }

            var filename = httpFile.FileName;
            var ext = string.Empty;
            var whitelist = new HashSet<string>
            {
                ".gif",".jpg",".png",".bmp",".jpeg",".webp"
            };
            if (from.Equals("pasteImage"))
            {
                ext = Path.GetExtension(filename);
                if (string.IsNullOrEmpty(ext))
                {
                    ext = ".png";
                }
            }
            else
            {
                ext = Path.GetExtension(filename);
            }
            if (!whitelist.Contains(ext.ToLower()))
            {
                resultMsg = "Please upload image";

                return re;
            }
            long maxFileSize=0;
            switch (from)
            {
                case "logo":
                    maxFileSize=1024*1024*config.FileStoreConfig.UploadAvatarMaxSizeMB;
                    break;
                case "blogLogo":
                    maxFileSize = 1024 * 1024 * config.FileStoreConfig.UploadBlogLogoMaxSizeMB;
                    break;
                default:
                    maxFileSize = 1024 * 1024 * config.FileStoreConfig.UploadImageMaxSizeMB;
                    break;
            }
            if (httpFile.Length>maxFileSize)
            {
                re.Code = 0;
                re.Msg=$"The file Size is bigger than {maxFileSize/(1024*1024)}M";
                return re;
            }
            //写入对象储存
            filename = fileId.ToHex() + ext;
            var objectName=$"{fileUrlPath}/{filename}";
           
            var memi=GetMemi(ext);
            bool result = await noteFileService.SaveFile(objectName, httpFile, memi);
            //File对象
            var fileInfo=new NoteFile()
            {
                FileId=fileId,
                UserId=userid,
                AlbumId=1,

                Name =filename,
                Title=httpFile.FileName,
                Path=objectName,
                Size=httpFile.Length,
                CreatedTime=nowTime
                
            };
            result= noteFileService.AddImage(fileInfo,albumId,userid, from == "" || from == "pasteImage");
            re.Ok=result;
            re.Item=fileInfo;

            //re.Id=@$"/api/File/Avatars/{userid.ToHex()}/{filename}";

            return re;
        }
    }
}