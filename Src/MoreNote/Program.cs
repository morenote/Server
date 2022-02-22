using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MoreNote.Common.Utils;
using MoreNote.Logic.Database;
using MoreNote.Logic.Service;
using System.Collections.Generic;
using System.Linq;

namespace MoreNote
{
    public class Program
    {
        public static void Main(string[] args)
        {
          
            //InitSecret();//初始化安全密钥
           var host=  CreateHostBuilder(args).Build();
            var map = GetArgsMap(args);
            if (map.Keys.Contains("migrate"))
            {
                //执行数据库迁移命令
                System.Console.WriteLine("====================PostgreSQL migrate=========================");
                using (var scope = host.Services.CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<DataContext>();
                    
                    db.Database.Migrate();
                }

            }




            host.Run();
        }

        static Dictionary<string, string> GetArgsMap(string[] args)
        {
            var map = new Dictionary<string, string>();
            for (int index = 0; index < args.Length; index++)
            {
                string arg = args[index];
                if (arg.StartsWith("-"))
                {
                    if ((index + 1) < args.Length && (!args[index + 1].StartsWith("-")))
                    {
                        map.Add(arg.Replace("-", ""), args[index + 1]);
                        index++;
                    }
                    else
                    {
                        map.Add(arg.Replace("-", ""), null);
                    }
                }
            }
            return map;

        }

        /// <summary>
        /// 初始化安全秘钥
        /// </summary>
        private static void InitSecret()
        {
            //每次启动程序都会重新初始化Secret
            ConfigFileService configFileService = new ConfigFileService();
            configFileService.WebConfig.SecurityConfig.Secret = RandomTool.CreatSafeRandomBase64(32);
            configFileService.Save();
            System.Console.WriteLine("InitSecret Success");
        }
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .UseServiceProviderFactory(new AutofacServiceProviderFactory())
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            })
            .ConfigureLogging((context, loggingBuilder) =>
            {
                 //loggingBuilder.ClearProviders();
                loggingBuilder.AddFilter("System", LogLevel.Warning);
                loggingBuilder.AddFilter("Microsoft", LogLevel.Warning);//过滤掉系统自带的System，Microsoft开头的，级别在Warning以下的日志
                loggingBuilder.AddLog4Net("config/log4net.config"); //会读取appsettings.json的Logging:LogLevel:Default级别
            });
    }
}