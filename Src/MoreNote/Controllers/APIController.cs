using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Morenote.Framework.Filter.Global;
using MoreNote.Common.ExtensionMethods;
using MoreNote.Common.Utils;

using MoreNote.Framework.Controllers;
using MoreNote.Logic.DB;
using MoreNote.Logic.Entity;
using MoreNote.Logic.Entity.ConfigFile;
using MoreNote.Logic.Service;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using UpYunLibrary;
using UpYunLibrary.ContentRecognition;

namespace MoreNote.Controllers
{
    public class APIController : BaseController
    {
        private AccessService AccessService { get; set; }
        private RandomImageService randomImageService;
        private DataContext dataContext;
        private ConfigFileService configFileService;
    

        /// <summary>
        /// 保险丝
        /// </summary>
        private  readonly int _randomImageFuseSize;

        /// <summary>
        /// 保险丝计数器
        /// 当访问量查过保险丝的能力时，直接熔断接口。
        /// </summary>
        private static int _fuseCount = 0;

        private static readonly object _fuseObj = new object();
        
        /// <summary>
        /// 随机数组的大小
        /// </summary>
        private  int  size;

        /// <summary>
        /// 随即图片初始化的时间
        /// 这意味着 每小时的图片数量只有60个图片是随机选择的
        /// 每经过1小时更会一次图片
        /// </summary>
        private static int _initTime = -1;

        //目录分隔符
        private static readonly char dsc = Path.DirectorySeparatorChar;
          //private static Dictionary<string, string> typeName = new Dictionary<string, string>();
        private static  WebSiteConfig webcConfig;

        private static  UpYun upyun ;

        private static  Random random = new Random();

        private AccessService accessService;
        public APIController(AttachService attachService
            , TokenSerivce tokenSerivce
            , NoteFileService noteFileService
            , UserService userService
            , ConfigFileService configFileService
            , IHttpContextAccessor accessor,
            AccessService accessService,
            DataContext dataContext,
            RandomImageService randomImageService
           
            ) : base(attachService, tokenSerivce, noteFileService, userService, configFileService, accessor)
        {
            this.AccessService = accessService;
            this.dataContext = dataContext;
            this.randomImageService= randomImageService;
            this.configFileService = configFileService;
            webcConfig = configFileService.WebConfig;
            upyun = new UpYun(webcConfig.UpyunConfig.UpyunBucket, webcConfig.UpyunConfig.UpyunUsername,
                webcConfig.UpyunConfig.UpyunPassword);
           _randomImageFuseSize=  webcConfig.PublicAPI.RandomImageFuseSize;
            size = webcConfig.PublicAPI.RandomImageSize;
        }
      
        


      
        class RandomImageResult
        {
            public int Error { get; set; }
            public  int Result { get; set; }
            public  int Count { get; set; }
            public  List<string> Images { get; set; }
        }
        public async Task<IActionResult> GetRandomImage(string type,string format ="raw",int jsonSize=1)
        {
            var randomImageList = randomImageService.GetRandomImageList();
            lock (_fuseObj)
            {
                _fuseCount++;
            }
            RandomImage randomImage = null;
            if (DateTime.Now.Hour != _initTime)
            {
                _fuseCount = 0;
                _initTime = DateTime.Now.Hour;
            }
            else
            {
                if (_fuseCount > _randomImageFuseSize)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadGateway;
                    return Content("接口遭到攻击，并发量超出限制值，触发防火墙策略。");
                }
            }

            if (string.IsNullOrEmpty(type) || !randomImageList.ContainsKey(type))
            {
                type = "动漫综合2";
            }
            if (type.Equals("少女映画"))
            {
                string userHex = HttpContext.Session.GetString("_UserId");
                if (string.IsNullOrEmpty(userHex))
                {
                    //没登陆
                    return Redirect("/Auth/login");
                }
            }
            int index = random.Next(randomImageList[type].Count - 1);
            randomImage = randomImageList[type][index];

            string ext = Path.GetExtension(randomImage.FileName);
            IHeaderDictionary headers = Request.Headers;
            StringBuilder stringBuilder = new StringBuilder();
            foreach (KeyValuePair<string, Microsoft.Extensions.Primitives.StringValues> item in headers)
            {
                stringBuilder.Append(item.Key + "---" + item.Value + "\r\n");
            }
            string RealIP = headers["X-Forwarded-For"].ToString().Split(",")[0];
            string remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            string remotePort = Request.HttpContext.Connection.RemotePort.ToString();
            AccessRecords accessRecords = new AccessRecords()
            {
                AccessId = SnowFlakeNet.GenerateSnowFlakeID(),
                IP = RealIP,
                X_Real_IP = headers["X-Real-IP"],
                X_Forwarded_For = headers["X-Forwarded-For"],
                Referrer = headers["Referer"],
                RequestHeader = stringBuilder.ToString(),
                AccessTime = DateTime.Now,
                UnixTime = (long)UnixTimeHelper.GetTimeStampInInt32(),
                TimeInterval = -1,
                URL = "/api/GetRandomImage",
                RemoteIPAddress = remoteIpAddress,
                RemotePort = remotePort
            };
            //访问日志
            await AccessService.InsertAccessAsync(accessRecords).ConfigureAwait(false);
            string typeMD5 = randomImage.TypeNameMD5;
            upyun.secret = webcConfig.UpyunConfig.UpyunSecret; ;
            int unixTimestamp = UnixTimeHelper.GetTimeStampInInt32();
            Console.WriteLine("现在的时间="+unixTimestamp);
            unixTimestamp += 15;
             Console.WriteLine("过期时间="+unixTimestamp);

            //开启token防盗链

            switch (format)
            {
                case "raw":
                    if (webcConfig.PublicAPI.CanTokenAntiTheftChain)
                    {
                        string _upt = upyun.CreatToken(unixTimestamp.ToString(), upyun.secret, $"/upload/{typeMD5}/{randomImage.FileSHA1}{ext}");
                        return Redirect($"https://upyun.morenote.top/upload/{typeMD5}/{randomImage.FileSHA1}{ext}?_upt={_upt}");
                    }
                    else
                    {
                        return Redirect($"https://upyun.morenote.top/upload/{typeMD5}/{randomImage.FileSHA1}{ext}");
                    }
                case "json":
                    if (jsonSize<0)
                    {
                        jsonSize = 1;
                    }

                    if (jsonSize>20)
                    {
                        jsonSize = 20;
                    }
                    List<string> images=new List<string>();

                    for (int i = 0; i < jsonSize; i++)
                    {
                        string img=GetOneURL( type);
                        images.Add(img);
                    }
                    
                    RandomImageResult randomImageResult=new RandomImageResult()
                    {
                        Error = 0,
                        Result = 200,
                        Count = images.Count,
                        Images =images
                    };
                    return Json(randomImageResult, Common.Utils.MyJsonConvert.GetSimpleOptions());
                default:
                    Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    return Content("format=??");

            }
            
        }

        private  string GetOneURL(string type)
        {
            var randomImageList = randomImageService.GetRandomImageList();
            RandomImage randomImage = null;
            int index = random.Next(randomImageList[type].Count - 1);
            randomImage = randomImageList[type][index];

            string ext = Path.GetExtension(randomImage.FileName);
            int unixTimestamp = UnixTimeHelper.GetTimeStampInInt32();
            string _upt1 = upyun.CreatToken(unixTimestamp.ToString(), upyun.secret, $"/upload/{type}/{randomImage.FileSHA1}{ext}");
            if (webcConfig.PublicAPI.CanTokenAntiTheftChain)
            {

                return $"https://upyun.morenote.top/upload/{type}/{randomImage.FileSHA1}{ext}?_upt={_upt1}";
            }
            else
            {
                return $"https://upyun.morenote.top/upload/{type}/{randomImage.FileSHA1}{ext}";
            }
        }

        public IActionResult ResolutionStrategy(String StrategyID)
        {
            if (string.IsNullOrEmpty(StrategyID))
            {
                    Response.StatusCode=404;
                    return Content("StrategyID does not exist");
            }
           
                try
                {
                     var rl=dataContext.ResolutionLocation.Where(b=>b.StrategyID.Equals(StrategyID.ToLongByNumber())).OrderBy(e=>e.Score).FirstOrDefault();
                     return Redirect(rl.URL);
                }
                catch (Exception)
                {
                    Response.StatusCode=404;
                    return Content("StrategyID does not exist"); 
                }
            
        }
        public IActionResult GetRandomImageFuseSize()
        {
            return Content(_fuseCount.ToString());

        }
        [HttpPost]
        public async Task<IActionResult> UpYunImageServiceHook()
        {
            using (StreamReader reader = new StreamReader(Request.Body))
            {
                try
                {
                    string body = await reader.ReadToEndAsync().ConfigureAwait(true);
                    ContentIdentifiesHookMessages message = JsonSerializer.Deserialize<ContentIdentifiesHookMessages>(body, Common.Utils.MyJsonConvert.GetOptions());
                    if (string.IsNullOrEmpty(message.uri) || message.type == UpyunType.test)
                    {
                        Response.StatusCode = 200;
                        return Content("未找到");
                    }
                    string fileSHA1 = Path.GetFileNameWithoutExtension(message.uri);

                   
                        RandomImage imagedb = dataContext.RandomImage.Where(b => b.FileSHA1.Equals(fileSHA1)).FirstOrDefault();
                        if (imagedb == null)
                        {
                            Response.StatusCode = (int)HttpStatusCode.NotFound;
                            return Content("未找到");
                        }
                        switch (message.type)
                        {
                            case UpyunType.delete:
                                imagedb.IsDelete = true;
                                break;

                            case UpyunType.shield:
                                imagedb.Block = true;
                                break;

                            case UpyunType.cancel_shield:
                                imagedb.Block = false;
                                break;

                            case UpyunType.forbidden:
                                imagedb.Block = true;
                                break;

                            case UpyunType.cancel_forbidden:
                                imagedb.Block = false;
                                break;

                            default:
                                break;
                        }
                        dataContext.SaveChanges();
                    // Do something
                }
                catch (Exception ex)
                {
                    Response.StatusCode = 404;
                    return Content("false"+ex.Message);
                }
            }
            Response.StatusCode = 200;
            return Content("ok");
        }

        //浏览器检测
        public IActionResult BrowserDetection()
        {
            return Redirect($"https://www.morenote.top/BrowserDetection.js");
        }

        /// <summary>
        /// 获取图形验证码
        /// </summary>
        /// <returns></returns>
        // [HttpGet("VerifyCode")]
        [SkipInspectionInstallationFilter]
        public async Task VerifyCode()
        {
            try
            {
                Response.ContentType = "image/jpeg";

                using (var stream = VerificationCode.GenerateImage(out var code))
                {
                    var buffer = stream.ToArray();

                    //存session
                    HttpContext.Session.SetString("VerifyCode", code.ToLower());

                    //使用标志，不允许重复使用一个验证码。
                    //这个验证码被消费一次后，要置0。
                    HttpContext.Session.SetInt32("VerifyCodeValid", 1);

                    //验证码生成时间。
                    HttpContext.Session.SetInt32("VerifyCodeTime", UnixTimeHelper.GetTimeStampInInt32());

                    //string sessionID = Request.Cookies["SessionID"];
                    //RedisManager.SetString(sessionID, code);

                    // Response.Cookies.Append("code",code);

                    // 将验证码的token放入cookie
                    // Response.Cookies.Append(VERFIY_CODE_TOKEN_COOKIE_NAME, await SecurityServices.GetVerifyCodeToken(code));

                    await Response.Body.WriteAsync(buffer, 0, buffer.Length).ConfigureAwait(false);
                }
            }
            catch (Exception e)
            {
                var buffer=ASCIIEncoding.ASCII.GetBytes(e.Message);
                await Response.Body.WriteAsync(buffer, 0, buffer.Length).ConfigureAwait(false);
                Console.WriteLine(e.Message);
                return;
            }
           
        }
    }
}