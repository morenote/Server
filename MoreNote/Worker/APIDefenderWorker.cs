using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using MoreNote.Common.Config;
using MoreNote.Common.Config.Model;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace MoreNoteWorkerService
{
    //定期扫描网站日志，并将恶意访问者的IP加入黑名单
    public class APIDefenderWorker : BackgroundService
    {
        private readonly ILogger<RandomImagesCrawlerWorker> _logger;

        /// <summary>
        /// 网站配置
        /// </summary>
        private static WebSiteConfig config = ConfigManager.GetWebConfig();

        public APIDefenderWorker()
        {
        }

        private Random random = new Random();

        public APIDefenderWorker(ILogger<RandomImagesCrawlerWorker> logger)
        {
            _logger = logger;
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