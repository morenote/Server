using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using MoreNote.Common.Config;
using MoreNote.Common.Config.Model;
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
        public static List<string> imageTypeList = new List<string>();
        /// <summary>
        /// 随机图片列表
        /// </summary>
        public static Dictionary<string, List<RandomImage>> randomImageList = new Dictionary<string, List<RandomImage>>();


        /// <summary>
        /// 网站配置
        /// </summary>
        private static readonly WebSiteConfig config = ConfigManager.GetPostgreSQLConfig();
        public UpdataImageURLWorker()
        {

        }

        private readonly Random random = new Random();

        public UpdataImageURLWorker(ILogger<RandomImagesCrawlerWorker> logger)
        {
            _logger = logger;
            InitList();
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
        private static readonly int size = config.randomImageSize;
        public void InitList()
        {
            lock (this)
            {
                imageTypeList.Add("动漫综合1");
                imageTypeList.Add("动漫综合2");
                imageTypeList.Add("动漫综合3");
                imageTypeList.Add("动漫综合4");
                imageTypeList.Add("动漫综合5");
                imageTypeList.Add("动漫综合6");
                imageTypeList.Add("动漫综合7");
                imageTypeList.Add("动漫综合8");
                imageTypeList.Add("动漫综合9");
                imageTypeList.Add("动漫综合10");
                imageTypeList.Add("动漫综合11");
                imageTypeList.Add("动漫综合12");
                imageTypeList.Add("动漫综合13");
                imageTypeList.Add("动漫综合14");
                imageTypeList.Add("动漫综合15");
                imageTypeList.Add("动漫综合16");
                imageTypeList.Add("动漫综合17");
                imageTypeList.Add("动漫综合18");

                imageTypeList.Add("火影忍者1");



                imageTypeList.Add("缘之空1");

                imageTypeList.Add("东方project1");

                imageTypeList.Add("猫娘1");

               

                imageTypeList.Add("少女前线1");

                imageTypeList.Add("风景系列1");
                imageTypeList.Add("风景系列2");
                imageTypeList.Add("风景系列3");
                imageTypeList.Add("风景系列4");
                imageTypeList.Add("风景系列5");
                imageTypeList.Add("风景系列6");
                imageTypeList.Add("风景系列7");
                imageTypeList.Add("风景系列8");
                imageTypeList.Add("风景系列9");
                imageTypeList.Add("风景系列10");

                imageTypeList.Add("物语系列1");
                imageTypeList.Add("物语系列2");

                imageTypeList.Add("明日方舟1");
                imageTypeList.Add("明日方舟2");


                imageTypeList.Add("重装战姬1");


                imageTypeList.Add("P站系列1");
                imageTypeList.Add("P站系列2");
                imageTypeList.Add("P站系列3");
                imageTypeList.Add("P站系列4");


                imageTypeList.Add("CG系列1");
                imageTypeList.Add("CG系列2");
                imageTypeList.Add("CG系列3");
                imageTypeList.Add("CG系列4");
                imageTypeList.Add("CG系列5");


                imageTypeList.Add("守望先锋");

                imageTypeList.Add("王者荣耀");

                imageTypeList.Add("少女写真1");
                imageTypeList.Add("少女写真2");
                imageTypeList.Add("少女写真3");
                imageTypeList.Add("少女写真4");
                imageTypeList.Add("少女写真5");
                imageTypeList.Add("少女写真6");


                imageTypeList.Add("死库水萝莉");
                imageTypeList.Add("萝莉");
                imageTypeList.Add("极品美女图片");
                imageTypeList.Add("日本COS中国COS");
                imageTypeList.Add("少女映画");

            }

        }
        private async Task UpdatImage()
        {

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
