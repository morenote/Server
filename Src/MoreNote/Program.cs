using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MoreNote.Common.Utils;
using MoreNote.Logic.Database;
using MoreNote.Logic.Entity;
using MoreNote.Logic.Service;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MoreNote
{
    public class Program
    {
        public static void Main(string[] args)
        {

           
            var host = CreateHostBuilder(args).Build();
            var map = GetArgsMap(args);

            DeployService deployService = new DeployService(host);
            if (map.Keys.Contains("command"))
            {
                var cmd = map["command"];
                
                switch (cmd)
                {
                    case "GenSecret":
                        deployService.InitSecret();
                        return;
                    case "MigrateDatabase":
                        deployService.MigrateDatabase();
                        return;
                    default:
                        Console.WriteLine("unkown cmd");
                        return;
                }

            }
           

            host.Run();
        }

        private static Dictionary<string, string> GetArgsMap(string[] args)
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