using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MoreNote.Controllers;

namespace MoreNoteWorkerService
{
    public class InitRandomImagesWorker : BackgroundService
    {
        private readonly ILogger<InitRandomImagesWorker> _logger;

        public InitRandomImagesWorker()
        {
        }

        public InitRandomImagesWorker(ILogger<InitRandomImagesWorker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                //填充
                var randomImageList = APIController.RandomImageList;
                var size = randomImageList.Count;
                int max = 120;
                string name ="";
                GetHttpWebRequest("动漫综合2", out name);
                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
        }
        private static byte[] GetHttpWebRequest(string type,out string key)
        {
            //建立请求
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"https://api.r10086.com:8000/" + type + ".php");
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
            //name = "upload\\" + name;
            //if (!File.Exists(name))
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
            key = name;
            return buffer1;
        }


    }
}
