using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
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
        /// <summary>
        /// 默认1000
        /// </summary>
        public int pageSize = 1000;
        public TokenSerivce tokenSerivce;
     

   

        public UserService userService;

        // 不能更改
        protected IHttpContextAccessor _accessor;
        protected ConfigFileService configFileService;

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
            config = configFileService.WebConfig;
        }
        /// <summary>
        /// 检查验证码
        /// </summary>
        /// <returns></returns>
        public bool CheckVerifyCode(string captcha, out string message)
        {
            string verifyCode = HttpContext.Session.GetString("VerifyCode");
            int time = HttpContext.Session.GetInt32("VerifyCodeTime").GetValueOrDefault(0);
            int valid = HttpContext.Session.GetInt32("VerifyCodeValid").GetValueOrDefault(0);
            if (valid != 1 || !UnixTimeHelper.IsValid(time, 60))//验证码的保质期是60秒
            {
                message="验证码过期或失效";
                return false;
            }
            //销毁验证码的标志
            HttpContext.Session.SetInt32("VerifyCodeValid", 0);
            if (string.IsNullOrEmpty(verifyCode) || string.IsNullOrEmpty(captcha))
            {
                message="错误参数";
                return false;
            }
            else
            {
                if (captcha.Equals("0")||!captcha.ToLower().Equals(verifyCode))
                {
                    message="验证码错误";
                    return false;
                }
            }
            message="";
            return true;
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
            if (userInfo!=null&&userInfo.Username.Equals(config.SecurityConfig.AdminUsername))
            {
                ViewBag.isAdmin=true;
            }
            else
            {
                ViewBag.isAdmin = false;
            }
            return userInfo;
        }

        public User GetUserBySession()
        {
            string userid_hex = _accessor.HttpContext.Session.GetString("UserId");
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
            string userid_hex = _accessor.HttpContext.Session.GetString("UserId");
            long? userid_number = userid_hex.ToLongByHex();
            return userid_number;
        }
        public void UpdateSession(string key,string value)
        {
            _accessor.HttpContext.Session.SetString(key,value);
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
                string userid_hex = _accessor.HttpContext.Session.GetString("UserId");
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
        public User GetUserAndBlogUrl()
        {
           var userid=GetUserIdBySession();
            if (userid==null)
            {
                return new User();

            }
            else
            {
                return userService.GetUserAndBlogUrl(userid);
            }
        }

        public bool HasLogined()
        {
            string userHex = HttpContext.Session.GetString("UserId");
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
        /// <summary>
        /// 获得国际化语言资源文件
        /// </summary>
        /// <returns></returns>
        public LanguageResource GetLanguageResource()
        {
            var lnag = "zh-cn";

            var locale = Request.Cookies["LEANOTE_LANG"];

            if (string.IsNullOrEmpty(locale))
            {
                locale = lnag;
            }
            var languageResource = LanguageFactory.GetLanguageResource(locale);
            return languageResource;
        }
        /// <summary>
        /// 设置区域性信息
        /// </summary>
        /// <returns></returns>
        public string SetLocale()
        {
            //todo:SetLocale未完成

             var lnag = "zh-cn";

             var locale =Request.Cookies["LEANOTE_LANG"];

            if (string.IsNullOrEmpty(locale))
            {
                locale=lnag;
            }
            


             var  languageResource=LanguageFactory.GetLanguageResource(locale);

             ViewBag.msg = languageResource.GetMsg();

             
            ViewBag.member = languageResource.GetMember();
            ViewBag.markdown = languageResource.GetMarkdown();
            ViewBag.blog = languageResource.GetBlog();
            ViewBag.noteconf = languageResource.GetNote();
            ViewBag.tinymce_editor = languageResource.GetTinymce_editor();
            ViewBag.demonstrationOnly=configFileService.WebConfig.GlobalConfig.DemonstrationOnly;


            ViewBag.locale=locale;
            ViewBag.siteUrl = config.APPConfig.SiteUrl;
            ViewBag.blogUrl=config.APPConfig.BlogUrl;
            ViewBag.leaUrl = config.APPConfig.LeaUrl;
            ViewBag.noteUrl = config.APPConfig.NoteUrl;
           

            return null;
        }
        public void SetUserIdToSession(long? userId)
        {
            _accessor.HttpContext.Session.SetString("UserId", userId.ToHex24());
        }
        // todo :上传附件
        public bool UploadAttach(string name, long? userId, long? noteId, out string msg, out long? serverFileId)
        {
            msg = "";
            serverFileId = 0;
            FileStoreConfig config=configFileService.WebConfig.FileStoreConfig;


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
            // Task<bool> task = noteFileService.SaveUploadFileOnUPYunAsync(upyun, httpFile, uploadDirPath, fileName);
            //Task<bool> task = noteFileService.SaveUploadFileOnDiskAsync(httpFile, uploadDirPath, fileName);

            var ext = Path.GetExtension(fileName);
            var provider = new FileExtensionContentTypeProvider();
            var memi = provider.Mappings[ext];
            var nowTime = DateTime.Now;
            var objectName = $"{userId.ToHex()}/attachments/{ nowTime.ToString("yyyy")}/{nowTime.ToString("MM")}/{diskFileId.ToHex()}{ext}";
            bool result = noteFileService.SaveFile(objectName, httpFile, memi).Result;
            
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
                    Path =  fileName,
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
        /// <summary>
        /// 获取文件MEMI
        /// </summary>
        /// <param name="ext"></param>
        /// <returns></returns>
        public  string GetMemi(string ext)
        {
            try
            {
                var provider = new FileExtensionContentTypeProvider();
                if (provider.Mappings.ContainsKey(ext))
                {
                    var memi = provider.Mappings[ext];
                    return memi;
                }
                else
                {
                    return "application/octet-stream";
                }
               

            }
            catch (Exception)
            {
                return "application/octet-stream";
                throw;
            }
            
        }

        public bool UploadImages(string name, long? userId, long? noteId, bool isAttach, out long? serverFileId, out string msg)
        {
            if (isAttach)
            {
                return UploadAttach(name, userId, noteId, out msg, out serverFileId);
            }
            msg = "";
            serverFileId = 0;
            FileStoreConfig config = configFileService.WebConfig.FileStoreConfig;
            string uploadDirPath = null;
    
           

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
            var ext= Path.GetExtension(httpFile.FileName);
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
            //Task<bool> task = noteFileService.SaveUploadFileOnDiskAsync(httpFile, uploadDirPath, fileName);
           
            var provider = new FileExtensionContentTypeProvider();
            var memi = provider.Mappings[ext];
            var nowTime = DateTime.Now;
            var objectName= $"{userId.ToHex()}/images/{ nowTime.ToString("yyyy")}/{nowTime.ToString("MM")}/{diskFileId.ToHex()}{ext}";
            bool result = noteFileService.SaveFile(objectName, httpFile, memi).Result;

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
                    CreatedTime = nowTime
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