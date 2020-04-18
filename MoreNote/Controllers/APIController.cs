using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MoreNote.Common.Config;
using MoreNote.Common.Config.Model;
using MoreNote.Common.Util;
using MoreNote.Common.Utils;
using MoreNote.Logic.Entity;
using MoreNote.Logic.Service;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using UpYunLibrary;
using UpYunLibrary.ContentRecognition;

namespace MoreNote.Controllers
{
    public class APIController : Controller
    {
        /// <summary>
        /// 随机图片列表
        /// </summary>
        private static Dictionary<string, List<RandomImage>> _randomImageList = new Dictionary<string, List<RandomImage>>();
        //private static Dictionary<string, string> typeName = new Dictionary<string, string>();
        static WebSiteConfig postgreSQLConfig = ConfigManager.GetPostgreSQLConfig();
        static UpYun upyun = new UpYun(postgreSQLConfig.upyunBucket, postgreSQLConfig.upyunUsername, postgreSQLConfig.upyunPassword);
        static Random random = new Random();
        /// <summary>
        /// 保险丝
        /// </summary>
        static int _randomImageFuseSize = postgreSQLConfig.randomImageFuseSize;
        /// <summary>
        /// 保险丝计数器
        /// 当访问量查过保险丝的能力时，直接熔断接口。
        /// </summary>
        static int _fuseCount = 0;
        static private object _fuseObj = new object();
        /// <summary>
        /// 图片游标
        /// </summary>
        private int index = 0;
        /// <summary>
        /// 随机数组的大小
        /// </summary>
        private static int size = postgreSQLConfig.randomImageSize;
       
        /// <summary>
        /// 随即图片初始化的时间
        /// 这意味着 每小时的图片数量只有60个图片是随机选择的
        /// 每经过1小时更会一次图片
        /// </summary>
        private static int _initTime = -1;

        //目录分隔符
        private static char dsc = Path.DirectorySeparatorChar;
        static string dir = ConfigManager.GetPostgreSQLConfig().randomImageDir;
     
        public async Task<IActionResult> GetRandomImage(string type)
        {
            int hour = DateTime.Now.Hour;
            lock (_fuseObj)
            {
                _fuseCount++;
            }
            string ext = null;
            if (string.IsNullOrEmpty(type))
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
            if (!_randomImageList.ContainsKey(type))
            {
                _randomImageList.Add(type, new List<RandomImage>(size));
            }
            RandomImage randomImage = RandomImageService.GetRandomImage(type);
            if (_randomImageList[type].Count<size)
            {
                randomImage = RandomImageService.GetRandomImage(type);
                if (random==null)
                {
                    return NotFound();
                }
                _randomImageList[type].Add(randomImage);
            }
            else
            {
                if (DateTime.Now.Hour!=_initTime)
                {
                    _fuseCount = 0;
                     _initTime = DateTime.Now.Hour;
                    _randomImageList[type].Clear();
                    randomImage = RandomImageService.GetRandomImage(type);
                    if (random == null)
                    {
                        return NotFound();
                    }
                    _randomImageList[type].Add(randomImage);
                }
                else
                {
                    if ( _fuseCount > _randomImageFuseSize)
                    {
                        Response.StatusCode =(int) HttpStatusCode.BadGateway;
                        return Content("接口并发太高，接口已经熔断");
                    }
                    int index = random.Next(_randomImageList[type].Count-1);
                    randomImage = _randomImageList[type][index];
                }
            }
          
            if (randomImage==null)
            {
                return NotFound();
            }
            ext = Path.GetExtension(randomImage.FileName);
            var headers = Request.Headers;
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var item in headers)
            {
                stringBuilder.Append(item.Key + "---" + item.Value + "\r\n");
            }
            string RealIP = headers["X-Forwarded-For"].ToString().Split(",")[0];
            AccessRecords accessRecords = new AccessRecords()
            {
                AccessId = SnowFlake_Net.GenerateSnowFlakeID(),
                IP = RealIP,
                X_Real_IP = headers["X-Real-IP"],
                X_Forwarded_For = headers["X-Forwarded-For"],
                Referrer = headers["Referer"],
                RequestHeader = stringBuilder.ToString(),
                AccessTime = DateTime.Now,
                UnixTime = (long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds,
                TimeInterval = -1,
                url = "/api/GetRandomImage"
            };
            await AccessService.InsertAccessAsync(accessRecords).ConfigureAwait(false);
            type = randomImage.TypeNameMD5;
            upyun.secret = postgreSQLConfig.upyunSecret; ;
            Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
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
        public  async Task<IActionResult> UpYunImageServiceHook()
        {
            var x = Request;
            using (var reader = new StreamReader(Request.Body))
            {
                var body =await reader.ReadToEndAsync();
                var message= JsonSerializer.Deserialize<ContentIdentifiesHookMessages>(body, MyJsonConvert.GetOptions());

                // Do something
            }

            return Content("true");

        }




    }
}