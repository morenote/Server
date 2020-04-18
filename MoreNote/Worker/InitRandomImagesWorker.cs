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
using MoreNote.Common.Config;
using MoreNote.Common.Config.Model;
using MoreNote.Common.Util;
using MoreNote.Common.Utils;
using MoreNote.Controllers;
using MoreNote.Logic.Entity;
using MoreNote.Logic.Service;
using UpYunLibrary;

namespace MoreNoteWorkerService
{
    public class InitRandomImagesWorker : BackgroundService
    {
        private readonly ILogger<InitRandomImagesWorker> _logger;
        /// <summary>
        /// 网站配置
        /// </summary>
        static WebSiteConfig config = ConfigManager.GetPostgreSQLConfig();
        /// <summary>
        /// 又拍云
        /// </summary>
        static UpYun upyun = new UpYun(config.upyunBucket, config.upyunUsername, config.upyunPassword);
        public InitRandomImagesWorker()
        {

        }
        List<string > typeList=new List<string>();
        Random random = new Random();

        public InitRandomImagesWorker(ILogger<InitRandomImagesWorker> logger)
        {
            _logger = logger;
            typeList.Add("动漫综合1");
            typeList.Add("动漫综合2");
            typeList.Add("动漫综合3");
            typeList.Add("动漫综合4");
            typeList.Add("动漫综合5");
            typeList.Add("动漫综合6");
            typeList.Add("动漫综合7");
            typeList.Add("动漫综合8");
            typeList.Add("动漫综合9");
            typeList.Add("动漫综合10");
            typeList.Add("动漫综合11");
            typeList.Add("动漫综合12");
            typeList.Add("动漫综合13");
            typeList.Add("动漫综合14");
            typeList.Add("动漫综合15");
            typeList.Add("动漫综合16");
            typeList.Add("动漫综合17");
            typeList.Add("动漫综合18");

            typeList.Add("火影忍者1");

      
            
            typeList.Add("缘之空1");

            typeList.Add("东方project1");

            typeList.Add("物语系列1");
            typeList.Add("物语系列2");

            typeList.Add("少女前线1");

            typeList.Add("风景系列1");
            typeList.Add("风景系列2");
            typeList.Add("风景系列3");
            typeList.Add("风景系列4");
            typeList.Add("风景系列5");
            typeList.Add("风景系列6");
            typeList.Add("风景系列7");
            typeList.Add("风景系列8");
            typeList.Add("风景系列9");

            typeList.Add("明日方舟1");
            typeList.Add("明日方舟2");


            typeList.Add("重装战姬1");


            typeList.Add("P站系列1");
            typeList.Add("P站系列2");


            typeList.Add("CG系列1");
            typeList.Add("CG系列2");
            typeList.Add("CG系列3");
            typeList.Add("CG系列4");
            typeList.Add("CG系列5");


            typeList.Add("守望先锋");

            typeList.Add("王者荣耀");

            typeList.Add("少女写真1");
            typeList.Add("少女写真2");
            typeList.Add("少女写真3");
            typeList.Add("少女写真4");
            typeList.Add("少女写真5");
            typeList.Add("少女写真6");


            typeList.Add("死库水萝莉");
            typeList.Add("萝莉");
            typeList.Add("极品美女图片");
            typeList.Add("日本COS中国COS");
            typeList.Add("少女映画");
            

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
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
                    var number = random.Next(typeList.Count);
                    await GetHttpWebRequestForAnYaAsync(typeList[number]).ConfigureAwait(false);
                    int time = DateTime.Now.Hour;
                    //每过60秒随机抓取一次
                    //频率太高，站长会顺着网线过来打人
                    await Task.Delay(TimeSpan.FromSeconds(config.Reptile_Delay_Second), stoppingToken).ConfigureAwait(false);
                }
                catch(Exception ex)
                {
                    _logger.LogInformation(ex.Message, DateTimeOffset.Now);
                    await Task.Delay(TimeSpan.FromSeconds(config.Reptile_Delay_Second), stoppingToken).ConfigureAwait(false);
                }
               
            
            }
        }
        private async Task GetHttpWebRequestForAnYaAsync(string type)
        {
          
            string url = "";
            if (type.Equals("少女映画"))
            {
                url = "https://api.r10086.com:8443/少女映画.php?password=20";
            }
            else
            {
                url = $"https://api.r10086.com:8443/" + type + ".php";
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
            byte[] imageBytes = stmMemory.ToArray();
            string fileSHA1 = SHAEncrypt_Helper.Hash1Encrypt(imageBytes);
            //上传到又拍云
            if (!RandomImageService.Exist(type, fileSHA1))
            {
                upyun.writeFile($"/upload/{SHAEncrypt_Helper.MD5Encrypt(type)}/{fileSHA1}{Path.GetExtension(name)}", imageBytes, true);
                RandomImage randomImage = new RandomImage()
                {
                    RandomImageId = SnowFlake_Net.GenerateSnowFlakeID(),
                    TypeName = type,
                    TypeNameMD5 = SHAEncrypt_Helper.MD5Encrypt(type),
                    TypeNameSHA1 = SHAEncrypt_Helper.Hash1Encrypt(type),
                    FileName = name,
                    FileNameMD5 = SHAEncrypt_Helper.MD5Encrypt(name),
                    FileNameSHA1 = SHAEncrypt_Helper.Hash1Encrypt(name),
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
