using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using MoreNote.Common.Utils;
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
        public APIController(IHttpContextAccessor accessor) : base(accessor)
        {


        }
        //private static Dictionary<string, string> typeName = new Dictionary<string, string>();
        private static readonly WebSiteConfig postgreSQLConfig = ConfigFileService.GetWebConfig();

        private static readonly UpYun upyun = new UpYun(postgreSQLConfig.upyunBucket, postgreSQLConfig.upyunUsername, postgreSQLConfig.upyunPassword);

        private static readonly Random random = new Random();

        /// <summary>
        /// 保险丝
        /// </summary>
        private static readonly int _randomImageFuseSize = postgreSQLConfig.randomImageFuseSize;

        /// <summary>
        /// 保险丝计数器
        /// 当访问量查过保险丝的能力时，直接熔断接口。
        /// </summary>
        private static int _fuseCount = 0;

        private static readonly object _fuseObj = new object();

        /// <summary>
        /// 随机数组的大小
        /// </summary>
        private static readonly int size = postgreSQLConfig.randomImageSize;

        /// <summary>
        /// 随即图片初始化的时间
        /// 这意味着 每小时的图片数量只有60个图片是随机选择的
        /// 每经过1小时更会一次图片
        /// </summary>
        private static int _initTime = -1;

        //目录分隔符
        private static readonly char dsc = Path.DirectorySeparatorChar;

        private static readonly string dir = ConfigFileService.GetWebConfig().randomImageDir;

        public async Task<IActionResult> GetRandomImage(string type)
        {
            var randomImageList = RandomImageService.GetRandomImageList();
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
                    return Content("接口并发太高，接口已经熔断");
                }
            }

            if (string.IsNullOrEmpty(type) || !randomImageList.ContainsKey(type))
            {
                type = "动漫综合2";
            }
            if (type.Equals("少女映画"))
            {
                string userHex = HttpContext.Session.GetString("_userId");
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
                url = "/api/GetRandomImage",
                RemoteIpAddress = remoteIpAddress,
                RemotePort = remotePort
            };
            await AccessService.InsertAccessAsync(accessRecords).ConfigureAwait(false);
            type = randomImage.TypeNameMD5;
            upyun.secret = postgreSQLConfig.upyunSecret; ;
            int unixTimestamp = UnixTimeHelper.GetTimeStampInInt32();
            unixTimestamp += 3;

            //开启token防盗链

            if (postgreSQLConfig.token_anti_theft_chain)
            {
                string _upt = upyun.CreatToken(unixTimestamp.ToString(), upyun.secret, $"/upload/{type}/{randomImage.FileSHA1}{ext}");
                return Redirect($"https://upyun.morenote.top/upload/{type}/{randomImage.FileSHA1}{ext}?_upt={_upt}");
            }
            else
            {
                return Redirect($"https://upyun.morenote.top/upload/{type}/{randomImage.FileSHA1}{ext}");
            }
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

                    using (DataContext db = new DataContext())
                    {
                        RandomImage imagedb = db.RandomImage.Where(b => b.FileSHA1.Equals(fileSHA1)).FirstOrDefault();
                        if (imagedb == null)
                        {
                            Response.StatusCode = (int)HttpStatusCode.NotFound;
                            return Content("未找到");
                        }
                        switch (message.type)
                        {
                            case UpyunType.delete:
                                imagedb.Delete = true;
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
                        db.SaveChanges();
                    }

                    // Do something
                }
                catch (Exception ex)
                {
                    Response.StatusCode = 404;
                    return Content("false");
                }
            }
            Response.StatusCode = 200;
            return Content("ok");
        }

        //浏览器检测
        public async Task<IActionResult> BrowserDetection()
        {
            return Redirect($"https://www.morenote.top/BrowserDetection.js");
        }

        /// <summary>
        /// 获取图形验证码
        /// </summary>
        /// <returns></returns>
        // [HttpGet("VerifyCode")]
        public async Task VerifyCode()
        {
            Response.ContentType = "image/jpeg";

            using (var stream = CheckCode.Create(out var code))
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
    }
}