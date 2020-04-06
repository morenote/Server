using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

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
using System.Threading.Tasks;
using UpYunLibrary;

namespace MoreNote.Controllers
{
    public class APIController : Controller
    {
        /// <summary>
        /// 随机图片列表
        /// </summary>
        private static Dictionary<string, Dictionary<string, string>> _randomImageList = new Dictionary<string, Dictionary<string, string>>();
        //private static Dictionary<string, string> typeName = new Dictionary<string, string>();
        static WebSiteConfig postgreSQLConfig = ConfigManager.GetPostgreSQLConfig();
        static UpYun upyun = new UpYun(postgreSQLConfig.upyunBucket, postgreSQLConfig.upyunUsername, postgreSQLConfig.upyunPassword);


        /// <summary>
        /// 图片游标
        /// </summary>
        private int index = 0;
        private int size = 60;

        /// <summary>
        /// 随即图片初始化的时间
        /// 这意味着 每小时的图片数量只有60个图片是随机选择的
        /// 每经过1小时更会一次图片
        /// </summary>
        private static int _initTime = -1;

        //目录分隔符
        private static char dsc = Path.DirectorySeparatorChar;
        static string dir = ConfigManager.GetPostgreSQLConfig().randomImageDir;
        private void getImages()
        {
            _randomImageList.Clear();
            index = 0;
            string dir = ConfigManager.GetPostgreSQLConfig().randomImageDir;
            if (!Directory.Exists(dir))
            {
                return;
            }
            string[] files = Directory.GetFiles(dir);
            Random random = new Random();
            int number = random.Next(1, 60);
            for (int i = 0; i < 60; i++)
            {
                int num = random.Next(0, files.Length);
                if (_randomImageList.Keys.Contains(files[num]))
                {
                }
                else
                {
                    _randomImageList.Add(files[num], null);
                }
            }
            _initTime = DateTime.Now.Hour;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> GetRandomImage(string type)
        {
            string ext = null;
            if (string.IsNullOrEmpty(type))
            {
                type = "动漫综合2";
            }
            string key = null;
            if (!_randomImageList.Keys.Contains(type))
            {
                _randomImageList[type] = new Dictionary<string, string>();
            }
            if (_randomImageList[type].Count < size)
            {
                string filename = "";
                GetHttpWebRequest(type, out filename);
                key = filename;
                ext = Path.GetExtension(key);
                key = SHAEncrypt_Helper.Hash1Encrypt(key);
                _randomImageList[type].Add(key,  ext);
            }
            else
            {
                if (DateTime.Now.Hour!= _initTime)
                {
                    _randomImageList[type].Clear();
                    _initTime = DateTime.Now.Hour;
                    string filename = "";
                    GetHttpWebRequest(type, out filename);
                    key = filename;
                    ext = Path.GetExtension(key);
                    key = SHAEncrypt_Helper.Hash1Encrypt(key);
                    _randomImageList[type].Add(key, ext);

                }
                else
                {
                    Random random = new Random();
                    int num = random.Next(0, _randomImageList[type].Count);
                    key = _randomImageList[type].ElementAt(num).Key;
                    ext = _randomImageList[type].ElementAt(num).Value;
                   
                }
            }
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
            //Random random = new Random();
            //int num = random.Next(0, _randomImageList.Count);

            //string filepath = _randomImageList.ElementAt(num).Key;
            //string fileName = System.IO.Path.GetFileName(filepath);
            //string url = System.Net.WebUtility.UrlEncode($"/API/ImageService/{type}/{key}");
            //string url = System.Net.WebUtility.UrlEncode($"ImageService/{type}/{key}");
            type = SHAEncrypt_Helper.Hash1Encrypt(type);
            upyun.secret = postgreSQLConfig.upyunSecret; ;
            Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            unixTimestamp += 5;
            string _upt = upyun.CreatToken(unixTimestamp.ToString(), upyun.secret, $"/upload/{type}/{key}{ext}");
            return Redirect($"https://upyun.morenote.top/upload/{type}/{key}{ext}?_upt={_upt}");
        }

        [Route("API/ImageService/{type}/{filepath}")]
        public async Task<IActionResult> ImageService(string type,string filepath)
        {
            
            if (string.IsNullOrEmpty(filepath))
            {
                return NotFound();
            }
            filepath = Path.GetFileNameWithoutExtension(filepath);
            //if (!typeName.ContainsKey(type))
            //{
            //    return NotFound();
            //}
            //type = typeName[type];
            if (!_randomImageList.ContainsKey(type))
            {
                return NotFound();
            }
            Dictionary<string,string> dic = _randomImageList[type];
            if (!dic.ContainsKey(filepath))
            {
                return NotFound();
            }
            //string dir = ConfigManager.GetPostgreSQLConfig().randomImageDir;
            filepath = dic[filepath];
            //filepath = dir + Path.DirectorySeparatorChar + filepath;
            string fileExt = System.IO.Path.GetExtension(filepath);
            //获取文件的ContentType
            var provider = new FileExtensionContentTypeProvider();
            var memi = provider.Mappings[fileExt];
         
            using (var stream = System.IO.File.OpenRead(filepath))
            {
                byte[] byteArray = new byte[stream.Length];
                await stream.ReadAsync(byteArray);
                //_randomImageList[filepath] = byteArray;
                return File(byteArray, memi);
            }
        }

        private  byte[] GetHttpWebRequest(string type,out string fileName)
        {
         
            
            string url = "";
            if (type.Equals("少女映画"))
            {
                 url = "https://api.r10086.com:8000/少女映画.php?password=20";
            }
            else
            {
                url = $"https://api.r10086.com:8000/" + type + ".php";
            }
            //建立请求
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            //添加Referer信息
            request.Headers.Add(HttpRequestHeader.Referer, "http://www.bz08.cn/");
            //伪装成谷歌浏览器
            //request.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/39.0.2171.71 Safari/537.36");
            request.Headers.Add(HttpRequestHeader.UserAgent, "I am a cute web crawler");
            //添加cookie认证信息
            Cookie cookie = new Cookie("PHPSESSID", "s9gajue8h7plf7n5ab8fehiuoq");
            cookie.Domain = "api.r10086.com";
            if (request.CookieContainer == null)
            {
                request.CookieContainer = new CookieContainer();
            }
            request.CookieContainer.Add(cookie);
            //发送请求获取Http响应
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            //HttpWebResponse response = (HttpWebResponse)(await request.GetResponseAsync().ConfigureAwait(false));

            var originalString = response.ResponseUri.OriginalString;
            Console.WriteLine(originalString);
            //获取响应流
            Stream receiveStream = response.GetResponseStream();
            //获取响应流的长度
            int length = (int)response.ContentLength;
            //读取到内存
            MemoryStream stmMemory = new MemoryStream();
            byte[] buffer1 = new byte[length];
            int i;
            //将字节逐个放入到Byte 中
            while ((i = receiveStream.Read(buffer1, 0, buffer1.Length)) > 0)
            {
                stmMemory.Write(buffer1, 0, i);
            }

            //写入磁盘
            string name = System.IO.Path.GetFileName(originalString);
            //上传到又拍云
            upyun.writeFile($"/upload/{SHAEncrypt_Helper.Hash1Encrypt(type)}/{SHAEncrypt_Helper.Hash1Encrypt(name)}{Path.GetExtension(name)}", stmMemory.ToArray(), true);
            //name = $"{dir}{dsc}upload{dsc}{type}{dsc}{name}";
            //if (!Directory.Exists($"{dir}{dsc}upload{dsc}{type}"))
            //{
            //    Directory.CreateDirectory($"{dir}{dsc}upload{dsc}{type}");
            //}
            //if (!System.IO.File.Exists(name))
            //{
            //    FileStream file = new FileStream(name, FileMode.Create, FileAccess.ReadWrite);
            //    file.Write(stmMemory.ToArray());
            //    file.Flush();
            //    file.Close();
            //}
            //FileStream file = new FileStream("1.jpg",FileMode.Create, FileAccess.ReadWrite);
            //关闭流
            receiveStream.Close();
            response.Close();
            fileName = name;
            return buffer1;
        }


      
    }
}