using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using MoreNote.Common.Config;
using MoreNote.Common.Utils;
using MoreNote.Logic.Entity;
using MoreNote.Logic.Service;

namespace MoreNote.Controllers
{
    public class APIController : Controller
    {
        /// <summary>
        /// 随机图片列表
        /// </summary>
        private static Dictionary<string,byte[]> _randomImageList = new Dictionary<string, byte[]>(60);
        int index = 0;
        /// <summary>
        /// 随即图片初始化的时间
        /// 这意味着 每小时的图片数量只有60个图片是随机选择的
        /// 每经过1小时更会一次图片
        /// </summary>
        private static int _initTime = -1;

        private void getImages(int size)
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
            int number = random.Next(1,60);
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

        public async Task<IActionResult> GetRandomImage(string fileId)
        {

            if (DateTime.Now.Hour!= _initTime)
            {
                getImages(60);
            }
            var headers = Request.Headers;
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var item in headers)
            {
                stringBuilder.Append(item.Key + "---" + item.Value+"\r\n");
            }
            string RealIP = headers["X-Forwarded-For"].ToString().Split(",")[0];
            AccessRecords accessRecords = new AccessRecords()
            {
                AccessId = SnowFlake_Net.GenerateSnowFlakeID(),
                IP = RealIP,
                X_Real_IP= headers["X-Real-IP"],
                X_Forwarded_For = headers["X-Forwarded-For"],
                Referrer = headers["Referer"],
                RequestHeader = stringBuilder.ToString(),
                AccessTime = DateTime.Now,
                UnixTime = (long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds,
                TimeInterval = -1,
                url = "/api/GetRandomImage"
            };
            await  AccessService.InsertAccessAsync(accessRecords).ConfigureAwait(false);
         
        Random random = new Random();
        int num = random.Next(0, _randomImageList.Count);

        string filepath = _randomImageList.ElementAt(num).Key;
        string fileName = System.IO.Path.GetFileName(filepath);
            return Redirect("/API/ImageService/" + fileName);
        }
        [Route("API/ImageService/{filepath}")]
        public async Task<IActionResult> ImageService(string filepath)
        {
            if (string.IsNullOrEmpty(filepath))
            {
                return NotFound();
            }
            string dir = ConfigManager.GetPostgreSQLConfig().randomImageDir;
         
            filepath = dir + Path.DirectorySeparatorChar + filepath;
            string fileExt = System.IO.Path.GetExtension(filepath);
            //获取文件的ContentType
            var provider = new FileExtensionContentTypeProvider();
            var memi = provider.Mappings[fileExt];
            if (_randomImageList[filepath]!=null)
            {
                return File(_randomImageList[filepath], memi);
            }
            else
            {
                using (var stream = System.IO.File.OpenRead(filepath))
                {
                    byte[] byteArray = new byte[stream.Length];
                    await stream.ReadAsync(byteArray);
                    _randomImageList[filepath] = byteArray;
                }
                return File(_randomImageList[filepath], memi);
            }
            
        }
    }
}