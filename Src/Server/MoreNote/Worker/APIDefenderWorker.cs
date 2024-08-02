using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using MoreNote.Config.ConfigFile;
using MoreNote.Logic.Service;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace MoreNoteWorkerService
{
	//定期扫描网站日志，并将恶意访问者的IP加入黑名单
	public class APIDefenderWorker : BackgroundService
	{
		private readonly ILogger<APIDefenderWorker> _logger;
		private ConfigFileService configFileService;
		/// <summary>
		/// 网站配置
		/// </summary>
		static WebSiteConfig config;
		public APIDefenderWorker()
		{

		}

		Random random = new Random();

		public APIDefenderWorker(ILogger<APIDefenderWorker> logger, ConfigFileService configFileService)
		{
			_logger = logger;
			this.configFileService = configFileService;
			config = configFileService.WebConfig;
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
