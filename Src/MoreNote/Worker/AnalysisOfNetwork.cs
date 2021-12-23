using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MoreNote.Controllers;
using MoreNote.Logic.Database;
using MoreNote.Logic.Entity;
using MoreNote.Logic.Entity.ConfigFile;
using MoreNote.Logic.Service;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace MoreNoteWorkerService
{
    /// <summary>
    /// 随机图片接口
    /// </summary>
    public class AnalysisOfNetwork : BackgroundService
    {
        private readonly ILogger<RandomImagesCrawlerWorker> _logger;
        private DataContext dataContext;
        private ConfigFileService configFileService;
  

        public AnalysisOfNetwork(ILogger<RandomImagesCrawlerWorker> logger, ConfigFileService configFileService)
        {
            _logger = logger;
            this.dataContext = dataContext;
            this.configFileService= configFileService;

            webSiteConfig = configFileService.WebConfig;
        }

        /// <summary>
        /// 随机图片列表
        /// </summary>

        /// <summary>
        /// 网站配置
        /// </summary>
        private   WebSiteConfig webSiteConfig ;
        public AnalysisOfNetwork()
        {

        }

        private readonly Random random = new Random();

     

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await CheckTheNetwork().ConfigureAwait(false);
                    await Task.Delay(TimeSpan.FromMinutes(10), stoppingToken).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    _logger.LogInformation(ex.Message, DateTimeOffset.Now);
                    await Task.Delay(TimeSpan.FromMinutes(10), stoppingToken).ConfigureAwait(false);
                }
            }
        }


        private async Task CheckTheNetwork()
        {
          
                ResolutionLocation[] rls = dataContext.ResolutionLocation.ToArray();
                foreach (ResolutionLocation item in rls)
                {
                    try
                    {
                        string url = item.URL;
                        //建立请求
                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                        //添加Referer信息
                        request.Headers.Add(HttpRequestHeader.Referer, "https://www.morenote.top/");
                        //UA
                        request.Headers.Add(HttpRequestHeader.UserAgent, "I_Am_A_Cute_AnalysisOfNetwork");
                        //发送请求获取Http响应
                        Stopwatch sw = new Stopwatch();
                        sw.Start();
                        HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync().ConfigureAwait(false);
                        //HttpWebResponse response = (HttpWebResponse)(await request.GetResponseAsync().ConfigureAwait(false));
                        if (response.StatusCode!=HttpStatusCode.OK)
                        {
                            throw new Exception("!=200");
                        }
                        string originalString = response.ResponseUri.OriginalString;
                        Console.WriteLine(originalString);
                        //获取响应流
                        Stream receiveStream = response.GetResponseStream();
                        //获取响应流的长度
                        int length = (int)response.ContentLength;
                        sw.Stop();
                        TimeSpan ts2 = sw.Elapsed;
                        double millisecond = ts2.TotalMilliseconds;
                        int speed = AnalyseSpeed(millisecond);
                        item.Speed=speed;

                        int weight = item.Weight;
                        int score = speed + weight;
                        item.Score = score;
                      
                        receiveStream.Close();
                        response.Close();
                    }
                    catch (Exception)
                    {   item.Speed=-1;
                        item.Score = 0;
                        throw;
                    }

                }
                await dataContext.SaveChangesAsync().ConfigureAwait(false);

            
        }

        private int AnalyseSpeed(double millisecond)
        {
            if (millisecond < 200)
            {
                return 5;

            }
            else if (millisecond < 400)
            {
                return 4;
            }
            else if (millisecond < 800)
            {
                return 3;
            }
            else if (millisecond < 1600)
            {
                return 2;
            }
            else if (millisecond < 3200)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

    }
}
