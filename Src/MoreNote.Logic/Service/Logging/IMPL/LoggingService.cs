
using Microsoft.Extensions.Logging;
using MoreNote.CryptographyProvider;
using MoreNote.Logic.Entity.ConfigFile;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Logic.Service.Logging.IMPL
{
    /// <summary>
    /// 日志服务
    /// </summary>
    public class LoggingService : ILoggingService,ILogger
    {
        private string path = "logs/log.log";
       
        public WebSiteConfig webSiteConfig { get; set; }
        private ICryptographyProvider cryptographyProvider { get; set; }
        public LoggingService(ConfigFileService configFileService, ICryptographyProvider cryptographyProvider)
        {
            this.webSiteConfig = configFileService.WebConfig;
            this.cryptographyProvider = cryptographyProvider;
           
        }
        private string getLogFileName()
        {
           
            var date= DateTime.Now.ToString("yyyyMMdd");
            return date + ".log";
        }
        char dsc = System.IO.Path.DirectorySeparatorChar;
        private string getLogDir()
        {
            var dir = System.AppDomain.CurrentDomain.BaseDirectory+ dsc+"logs" + dsc;
            return dir;
        }

        public async void Debug(string message)
        {
            var log = new Log(message);
            if (webSiteConfig.SecurityConfig.LogNeedHmac)
            {
                log= await log.AddMac(cryptographyProvider);
            }
            WirteFile( log.ToJson());
        }

        public async void Error(string message, Exception exception)
        {
            var log = new Log(message+"\r\n"+exception.StackTrace);
            if (webSiteConfig.SecurityConfig.LogNeedHmac)
            {
                log = await log.AddMac(cryptographyProvider);
            }
            WirteFile(log.ToJson());
        }

        public async void Info(string message)
        {
            var log = new Log(message );
            if (webSiteConfig.SecurityConfig.LogNeedHmac)
            {
                log = await log.AddMac(cryptographyProvider);
            }
            WirteFile( log.ToJson());
        }

        public async void Warn(string message)
        {
            var log = new Log(message);
            if (webSiteConfig.SecurityConfig.LogNeedHmac)
            {
                log = await log.AddMac(cryptographyProvider);
            }
            WirteFile( log.ToJson());
        }

        private void WirteFile(string message)
        {
            var dir = getLogDir();
            var logFileName = getLogFileName();
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            var path = dir + logFileName;
            if (!File.Exists(path))
            {
                File.Create(path).Close();
            }
            File.AppendAllText(path, message);
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            throw new NotImplementedException();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            throw new NotImplementedException();
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            throw new NotImplementedException();
        }
    }
}
