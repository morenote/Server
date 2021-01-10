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
    //定期扫描网站日志，并将恶意访问者的IP加入黑名单
    public class APIDefenderWorker : BackgroundService
    {
        private readonly ILogger<RandomImagesCrawlerWorker> _logger;
        private ConfigFileService configFileService;
        /// <summary>
        /// 网站配置
        /// </summary>
        static WebSiteConfig config ;
        public APIDefenderWorker()
        {

        }

        Random random = new Random();

        public APIDefenderWorker(ILogger<RandomImagesCrawlerWorker> logger,DependencyInjectionService dependencyInjectionService)
        {
            _logger = logger;
            configFileService=dependencyInjectionService.ServiceProvider.GetService(typeof(ConfigFileService))as ConfigFileService;
            
                config = configFileService.GetWebConfig();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await Check().ConfigureAwait(false);
                    int time = DateTime.Now.Hour;
                    //每过60秒随机抓取一次
                    //频率太高，站长会顺着网线过来打人
                    await Task.Delay(TimeSpan.FromMinutes(10), stoppingToken).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    _logger.LogInformation(ex.Message, DateTimeOffset.Now);
                    await Task.Delay(TimeSpan.FromMinutes(10), stoppingToken).ConfigureAwait(false);
                }
            }
        }
        private async Task Check()
        {

        }



    }
}
