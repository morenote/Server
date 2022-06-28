
using Microsoft.Extensions.Logging;
using MoreNote.CryptographyProvider;
using MoreNote.Logic.Database;
using MoreNote.Logic.Entity.ConfigFile;
using MoreNote.Models.Entity.Leanote.Loggin;
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
    public class LoggingService : ILoggingService
    {
        private string path = "logs/log.log";
       
        public WebSiteConfig webSiteConfig { get; set; }
        private ICryptographyProvider cryptographyProvider { get; set; }

        private DataContext dataContext { get; set; }
        public LoggingService(ConfigFileService configFileService, ICryptographyProvider cryptographyProvider,DataContext dataContext)
        {
            this.webSiteConfig = configFileService.WebConfig;
            this.cryptographyProvider = cryptographyProvider;
            this.dataContext= dataContext;
           
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

        public  void Debug(string message)
        {
            var log = new Log(message);
            if (webSiteConfig.SecurityConfig.LogNeedHmac)
            {
                log=  log.AddMac(cryptographyProvider);
            }
            WirteFile( log.ToJson());
        }

        public  void Error(string message, Exception exception)
        {
            var log = new Log(message+"\r\n"+exception.StackTrace);
            if (webSiteConfig.SecurityConfig.LogNeedHmac)
            {
                log =  log.AddMac(cryptographyProvider);
            }
            WirteFile(log.ToJson());
        }

        public  void Info(string message)
        {
            var log = new Log(message );
            if (webSiteConfig.SecurityConfig.LogNeedHmac)
            {
                log =  log.AddMac(cryptographyProvider);
            }
            WirteFile( log.ToJson());
        }

        public  void Warn(string message)
        {
            var log = new Log(message);
            if (webSiteConfig.SecurityConfig.LogNeedHmac)
            {
                log =  log.AddMac(cryptographyProvider);
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

        public void Save(LoggingLogin loggingLogin)
        {
            this.dataContext.LoggingLogin.Add(loggingLogin);
            this.dataContext.SaveChanges();
        }

        public List<LoggingLogin> GetAllUserLoggingLogin()
        {
            return dataContext.LoggingLogin.ToList<LoggingLogin>();
        }



    }
}
