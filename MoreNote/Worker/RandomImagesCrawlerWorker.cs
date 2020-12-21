using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MoreNote.Logic.Entity.ConfigFile;
using MoreNote.Common.Utils;
using MoreNote.Common.Utils;
using MoreNote.Controllers;
using MoreNote.Logic.Entity;
using MoreNote.Logic.Service;
using UpYunLibrary;

namespace MoreNoteWorkerService
{
    /// <summary>
    /// 随机图片接口的爬虫
    /// </summary>
    public class RandomImagesCrawlerWorker : BackgroundService
    {
        private readonly ILogger<RandomImagesCrawlerWorker> _logger;
        /// <summary>
        /// 网站配置
        /// </summary>
        static WebSiteConfig config = ConfigFileService.GetWebConfig();
        /// <summary>
        /// 又拍云
        /// </summary>
        static UpYun upyun = new UpYun(config.UpYunCDN.UpyunBucket, config.UpYunCDN.UpyunUsername, config.UpYunCDN.UpyunPassword);
        public RandomImagesCrawlerWorker()
        {

        }
       
        Random random = new Random();
       
        public RandomImagesCrawlerWorker(ILogger<RandomImagesCrawlerWorker> logger)
        {
            _logger = logger;
          
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var imageTypeList = RandomImageService.GetImageTypeList();
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    //_logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                    ////填充
                    //var randomImageList = APIController.RandomImageList;
                    //var size = randomImageList.Count;
                    //int max = 120;
                    //string name ="";
                    //GetHttpWebRequest("动漫综合2", out name);
                    var number = random.Next(imageTypeList.Count);
                    await GetHttpWebRequestForAnYaAsync(imageTypeList[number]).ConfigureAwait(false);
                    int time = DateTime.Now.Hour;
                    //每过60秒随机抓取一次
                    //频率太高，站长会顺着网线过来打人
                    await Task.Delay(TimeSpan.FromSeconds(config.Spiders.Reptile_Delay_Second), stoppingToken).ConfigureAwait(false);
                }
                catch(Exception ex)
                {
                    _logger.LogInformation(ex.Message, DateTimeOffset.Now);
                    await Task.Delay(TimeSpan.FromSeconds(config.Spiders.Reptile_Delay_Second), stoppingToken).ConfigureAwait(false);
                }
               
            
            }
        }
        private async Task GetHttpWebRequestForAnYaAsync(string type)
        {
          
            string url = "";
            if (type.Equals("少女映画"))
            {
                url = "https://api.r10086.com/少女映画.php?password=20";
            }
            else
            {
                url = $"https://api.r10086.com/" + type + ".php";
            }
            //建立请求
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            //添加Referer信息
            request.Headers.Add(HttpRequestHeader.Referer, "http://www.bz08.cn/");
            //伪装成谷歌浏览器 
            //request.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/39.0.2171.71 Safari/537.36");
            request.Headers.Add(HttpRequestHeader.UserAgent, "Power By www.morenote.top");
            //添加cookie认证信息
            Cookie cookie = new Cookie("PHPSESSID", "s9gajue8h7plf7n5ab8fehiuoq");
            cookie.Domain = "api.r10086.com";
            if (request.CookieContainer == null)
            {
                request.CookieContainer = new CookieContainer();
            }
            request.CookieContainer.Add(cookie);
            //发送请求获取Http响应
            HttpWebResponse response = (HttpWebResponse) await request.GetResponseAsync().ConfigureAwait(false);
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
            while ((i = await receiveStream.ReadAsync(buffer1, 0, buffer1.Length).ConfigureAwait(false)) > 0)
            {
                stmMemory.Write(buffer1, 0, i);
            }
            //写入磁盘
            string name = System.IO.Path.GetFileName(originalString);
            byte[] imageBytes = stmMemory.ToArray();
            string fileSHA1 = SHAEncryptHelper.Hash1Encrypt(imageBytes);
            //上传到又拍云
            if (!RandomImageService.Exist(type, fileSHA1))
            {
                upyun.writeFile($"/upload/{SHAEncryptHelper.MD5Encrypt(type)}/{fileSHA1}{Path.GetExtension(name)}", imageBytes, true);
                RandomImage randomImage = new RandomImage()
                {
                    RandomImageId = SnowFlakeNet.GenerateSnowFlakeID(),
                    TypeName = type,
                    TypeNameMD5 = SHAEncryptHelper.MD5Encrypt(type),
                    TypeNameSHA1 = SHAEncryptHelper.Hash1Encrypt(type),
                    FileName = name,
                    FileNameMD5 = SHAEncryptHelper.MD5Encrypt(name),
                    FileNameSHA1 = SHAEncryptHelper.Hash1Encrypt(name),
                    FileSHA1 = fileSHA1,
                    Sex = false,
                };
                //记录到数据库
                await RandomImageService.InsertImageAsync(randomImage).ConfigureAwait(false);

            }
            
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
            stmMemory.Close();
            receiveStream.Close();
            response.Close();
           
        }
       
    
    }
}
