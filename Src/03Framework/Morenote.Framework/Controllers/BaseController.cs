using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using MoreNote.Common.ExtensionMethods;
using MoreNote.Common.Utils;
using MoreNote.Language;
using MoreNote.Logic.Entity;
using MoreNote.Logic.Entity.ConfigFile;
using MoreNote.Logic.Service;
using MoreNote.Value;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UpYunLibrary;

namespace MoreNote.Framework.Controllers
{
    /**
     * 源代码基本是从GO代码直接复制过来的
     *
     * 只是简单的实现了API的功能
     *
     * 2020年01月27日
     * */

    public class BaseController : Controller
    {
        public AttachService attachService;
        /// <summary>
        /// 网站配置
        /// </summary>
        public WebSiteConfig config;

        public string defaultSortField = "UpdatedTime";
        public string leanoteUserId = "admin";
        public NoteFileService noteFileService;
        public int pageSize = 1000;
        public TokenSerivce tokenSerivce;

        /// <summary>
        /// 又拍云
        /// </summary>
        public UpYun upyun = null;

        public UserService userService;

        // 不能更改
        protected IHttpContextAccessor _accessor;
        private ConfigFileService configFileService;

        public BaseController(AttachService attachService
            , TokenSerivce tokenSerivce
            , NoteFileService noteFileService
            , UserService userService
            , ConfigFileService configFileService
            , IHttpContextAccessor accessor)
        {
            this.attachService = attachService;
            this.tokenSerivce = tokenSerivce;
            this.noteFileService = noteFileService;
            this.configFileService = configFileService;
            this.userService = userService;
            this._accessor = accessor;
            config = configFileService.GetWebConfig();
            if (config != null && config.UpYunCDN != null)
            {
                upyun = new UpYun(config.UpYunCDN.UpyunBucket, config.UpYunCDN.UpyunUsername, config.UpYunCDN.UpyunPassword);
            }
           
        }

        public enum FileTyte
        {
            /**
             * 文件分类
             * 视频 音频 图片 二进制 纯文本
             * */
            Video, Audio, Image, Binary, PlainText
        }

        public IActionResult Action()
        {
            return Content("error");
        }

        public long? ConvertUserIdToLong()
        {
            string hex = _accessor.HttpContext.Request.Form["userId"];
            if (string.IsNullOrEmpty(hex))
            {
                hex = _accessor.HttpContext.Request.Query["userId"];
            }
            if (string.IsNullOrEmpty(hex))
            {
                return 0;
            }
            return hex.ToLongByHex();
        }

        /// <summary>
        ///  得到第几页
        /// </summary>
        /// <returns></returns>
        public int GetPage()
        {
            var pageValue = Request.Query["page"];
            if (StringValues.IsNullOrEmpty(pageValue))
            {
                return 1;
            }
            int value = 1;
            Int32.TryParse(pageValue, out value);
            return value;

        }

        /// <summary>
        /// 通过HttpContext获得token
        /// todo:得到token, 这个token是在AuthInterceptor设到Session中的
        /// </summary>
        /// <returns></returns>
        public string GetTokenByHttpContext()
        {
            /**
             *  软件从不假设某个IP或者使用者借助cookie获得永久的使用权
             *  任何访问，必须显式的提供token证明
             *
             *  API服务不接受cookie中的信息，token总是显式提交的
             *
             **/
            string token = null;
            if (_accessor.HttpContext.Request.Form != null)
            {
                token = _accessor.HttpContext.Request.Form["token"];
            }

            if (string.IsNullOrEmpty(token))
            {
                token = _accessor.HttpContext.Request.Query["token"];
            }
            if (string.IsNullOrEmpty(token))
            {
                return "";
            }
            else
            {
                return token;
            }
        }
        public User SetUserInfo()
        {
            var userInfo=this.GetUserBySession();
            ViewBag.userInfo=userInfo;
            //todo:关于配置逻辑
            return userInfo;
        }

        public User GetUserBySession()
        {
            string userid_hex = _accessor.HttpContext.Session.GetString("_UserId");
            long? userid_number = userid_hex.ToLongByHex();
            User user = userService.GetUserByUserId(userid_number);
            return user;
        }

        public User GetUserByToken(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return null;
            }
            else
            {
                User user = tokenSerivce.GetUserByToken(token);
                return user;
            }
        }

        public User GetUserByToken()
        {
            string token = GetTokenByHttpContext();
            if (string.IsNullOrEmpty(token))
            {
                return null;
            }
            else
            {
                User user = tokenSerivce.GetUserByToken(token);
                return user;
            }
        }

        public long? GetUserIdBySession()
        {
            string userid_hex = _accessor.HttpContext.Session.GetString("_UserId");
            long? userid_number = userid_hex.ToLongByHex();
            return userid_number;
        }

        // todo:得到用户信息
        public long? GetUserIdByToken(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return 0;
            }
            else
            {
                User user = tokenSerivce.GetUserByToken(token);
                long? userid = (user == null ? 0 : user.UserId);
                return userid;
            }
        }

        public long? GetUserIdByToken()
        {
            string token = GetTokenByHttpContext();
            if (string.IsNullOrEmpty(token))
            {
                string userid_hex = _accessor.HttpContext.Session.GetString("userId");
                long? userid_number = userid_hex.ToLongByHex();
                return userid_number;
            }
            else
            {
                User user = tokenSerivce.GetUserByToken(token);
                long? userid = (user == null ? 0 : user.UserId);
                return userid;
            }
        }

        public bool HasLogined()
        {
            string userHex = HttpContext.Session.GetString("_UserId");
            if (string.IsNullOrEmpty(userHex))
            {
                //没登陆
                return false;
            }
            else
            {
                return true;
            }
        }
        public string SetLocale()
        {
            //todo:SetLocale未完成

             var lnag = "zh-cn";

             var locale =Request.Cookies["LEANOTE_LANG"];

             var  languageResource=LanguageFactory.GetLanguageResource(locale);
             ViewBag.msg = languageResource.GetMsg();

             
            ViewBag.member = languageResource.GetMember();
            ViewBag.markdown = languageResource.GetMarkdown();
            ViewBag.blog = languageResource.GetBlog();



            ViewBag.siteUrl ="/";
            ViewBag.leaUrl = "/";
            ViewBag.noteUrl = "/note/note";





            return null;
        }
        public void SetUserIdToSession(long? userId)
        {
            _accessor.HttpContext.Session.SetString("userId", userId.ToHex24());
        }
        // todo :上传附件
        public bool UploadAttach(string name, long? userId, long? noteId, out string msg, out long? serverFileId)
        {
            msg = "";
            serverFileId = 0;

            var uploadDirPath = $"/user/{userId.ToHex()}/upload/images/{DateTime.Now.ToString("yyyy_MM")}/";

            var diskFileId = SnowFlakeNet.GenerateSnowFlakeID();
            serverFileId = diskFileId;
            var httpFiles = _accessor.HttpContext.Request.Form.Files;
            //检查是否登录
            if (userId == 0)
            {
                userId = GetUserIdBySession();
                if (userId == 0)
                {
                    msg = "NoLogin";
                    return false;
                }
            }

            if (httpFiles == null || httpFiles.Count < 1)
            {
                return false;
            }
            var httpFile = httpFiles[name];
            var fileEXT = Path.GetExtension(httpFile.FileName).Replace(".", "");
            if (!IsAllowAttachExt(fileEXT))
            {
                msg = $"The_Attach_extension_{fileEXT}_is_blocked";
                return false;
            }
            var fileName = diskFileId.ToHex() + "." + fileEXT;
            //判断合法性
            if (httpFiles == null || httpFile.Length < 0)
            {
                return false;
            }
            //将文件保存在磁盘
            Task<bool> task = noteFileService.SaveUploadFileOnUPYunAsync(upyun, httpFile, uploadDirPath, fileName);
            bool result = task.Result;
            if (result)
            {
                //将结果保存在数据库
                AttachInfo attachInfo = new AttachInfo()
                {
                    AttachId = diskFileId,
                    UserId = userId,
                    NoteId = noteId,
                    UploadUserId = userId,
                    Name = fileName,
                    Title = httpFile.FileName,
                    Size = httpFile.Length,
                    Path = uploadDirPath + fileName,
                    Type = fileEXT.ToLower(),

                    CreatedTime = DateTime.Now
                    //todo: 增加特性=图片管理
                };
                var AddResult = attachService.AddAttach(attachInfo, true, out string AttachMsg);
                if (!AddResult)
                {
                    msg = "添加数据库失败";
                }
                return AddResult;
            }
            else
            {
                msg = "磁盘保存失败";
                return false;
            }
        }

        public void UploadAudio()
        {
        }

        //上传图片 png jpg
        public bool UploadImage()
        {
            return false;
        }

        public bool UploadImages(string name, long? userId, long? noteId, bool isAttach, out long? serverFileId, out string msg)
        {
            if (isAttach)
            {
                return UploadAttach(name, userId, noteId, out msg, out serverFileId);
            }
            msg = "";
            serverFileId = 0;

            var uploadDirPath = $"/user/{userId.ToHex()}/upload/images/{DateTime.Now.ToString("yyyy_MM")}/";

            var diskFileId = SnowFlakeNet.GenerateSnowFlakeID();
            serverFileId = diskFileId;
            var httpFiles = _accessor.HttpContext.Request.Form.Files;
            //检查是否登录
            if (userId == 0)
            {
                userId = GetUserIdBySession();
                if (userId == 0)
                {
                    msg = "NoLogin";
                    return false;
                }
            }

            if (httpFiles == null || httpFiles.Count < 1)
            {
                return false;
            }
            var httpFile = httpFiles[name];
            var fileEXT = Path.GetExtension(httpFile.FileName).Replace(".", "");
            if (!IsAllowImageExt(fileEXT))
            {
                msg = $"The_image_extension_{fileEXT}_is_blocked";
                return false;
            }
            var fileName = diskFileId.ToHex() + "." + fileEXT;
            //判断合法性
            if (httpFiles == null || httpFile.Length < 0)
            {
                return false;
            }
            //将文件保存在磁盘
            Task<bool> task = noteFileService.SaveUploadFileOnUPYunAsync(upyun, httpFile, uploadDirPath, fileName);
            bool result = task.Result;
            if (result)
            {
                //将结果保存在数据库
                NoteFile noteFile = new NoteFile()
                {
                    FileId = diskFileId,
                    UserId = userId,
                    AlbumId = 1,
                    Name = fileName,
                    Title = fileName,
                    Path = uploadDirPath + fileName,
                    Size = httpFile.Length,
                    CreatedTime = DateTime.Now
                    //todo: 增加特性=图片管理
                };
                var AddResult = noteFileService.AddImage(noteFile, 0, userId, true);
                if (!AddResult)
                {
                    msg = "添加数据库失败";
                }
                return AddResult;
            }
            else
            {
                msg = "磁盘保存失败";
                return false;
            }
        }
        public void UploadVideo()
        {
        }
        private bool IsAllowAttachExt(string ext)
        {
            //上传文件扩展名 白名单  后期会集中到一个类里面专门处理上传文件的问题
            HashSet<string> exts = new HashSet<string>() { "bmp","jpg","png","tif","gif","pcx","tga","exif","fpx","svg","psd","cdr","pcd","dxf","ufo","eps","ai","raw","WMF","webp",
                "zip","7z","rar"
                ,"mp4","mp3",
                "doc","docx","ppt","pptx","xls","xlsx"};
            if (exts.Contains(ext.ToLower()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //检查上传图片后缀名
        private bool IsAllowImageExt(string ext)
        {
            HashSet<string> exts = new HashSet<string>() { "bmp", "jpg", "jpeg", "png", "tif", "gif", "pcx", "tga", "exif", "fpx", "svg", "psd", "cdr", "pcd", "dxf", "ufo", "eps", "ai", "raw", "WMF", "webp" };
            if (exts.Contains(ext.ToLower()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}