using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using MoreNote.Logic.Entity.ConfigFile;
using MoreNote.Controllers;
using MoreNote.Logic.Entity;
using MoreNote.Logic.Service;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MoreNoteWorkerService
{
    /// <summary>
    /// 随机图片接口
    /// </summary>
    public class UpdataImageURLWorker : BackgroundService
    {
        private readonly ILogger<RandomImagesCrawlerWorker> _logger;
       
        /// <summary>
        /// 随机图片列表
        /// </summary>
       
        /// <summary>
        /// 网站配置
        /// </summary>
        private static readonly WebSiteConfig config = ConfigFileService.GetWebConfig();
        public UpdataImageURLWorker()
        {

        }

        private readonly Random random = new Random();

        public UpdataImageURLWorker(ILogger<RandomImagesCrawlerWorker> logger)
        {
            _logger = logger;
         
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await UpdatImage().ConfigureAwait(false);
                    int time = DateTime.Now.Hour;
                    //每过60秒随机抓取一次
                    //频率太高，站长会顺着网线过来打人
                    await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    _logger.LogInformation(ex.Message, DateTimeOffset.Now);
                    await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken).ConfigureAwait(false);
                }
            }
        }
        private static readonly int size = config.PublicAPI.RandomImageSize;
      
        private async Task UpdatImage()
        {
            var imageTypeList = RandomImageService.GetImageTypeList();
            var randomImageList = RandomImageService.GetRandomImageList();
            for (int y = 0; y < imageTypeList.Count; y++)
            {
               
                if (!randomImageList.ContainsKey(imageTypeList[y]))
                {
                    randomImageList.Add(imageTypeList[y], new List<RandomImage>(size));
                }
                else
                {
                    //randomImageList[imageTypeList[y]].Clear();
                }
                if (randomImageList[imageTypeList[y]].Count>=size)
                {
                    RandomImage randomImage = RandomImageService.GetRandomImage(imageTypeList[y]);
                    randomImageList[imageTypeList[y]][random.Next(0,size)]=randomImage;
                }
                else
                {
                    randomImageList[imageTypeList[y]] = RandomImageService.GetRandomImages(imageTypeList[y], size);
                }
               
            }




        }



    }
}
