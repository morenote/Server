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
using NLog.Web;

namespace MoreNote
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            
            var map = GetArgsMap(args);

            DeployService deployService = new DeployService(host);
            if (map.Keys.Contains("m"))
            {
                var cmd = map["m"];
                
                switch (cmd)
                {
                    case "GenSecret":
                         //deployService.InitSecret();
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
            .UseNLog();
            
            
           
    }
}