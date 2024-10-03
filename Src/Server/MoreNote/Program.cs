using Autofac;
using Autofac.Extensions.DependencyInjection;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

using MoreNote.autofac;
using MoreNote.Common.Utils;
using MoreNote.Logic.Service;

using NLog.Web;

using System;
using System.Collections.Generic;

namespace MoreNote
{
	public class Program
	{
		public static void Main(string[] args)
		{


            var builder = WebApplication.CreateBuilder(args);
			
            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
            builder.Host.ConfigureContainer<ContainerBuilder>(builder =>
            {
                builder.RegisterModule<AutofacModule>();
				
            });
			builder.Host.UseNLog();
		
            Startup startup = new Startup();

			startup.ConfigureServices(builder.Services);

         

            //设置读取指定位置的nlog.config文件
            if (RuntimeEnvironment.IsWindows)
			{
				NLogBuilder.ConfigureNLog("nlog-windows.config");
			}
			else
			{
				NLogBuilder.ConfigureNLog("nlog-linux.config");
			}
			
            var app = builder.Build();
			
            startup.Configure(app);


            var map = GetArgsMap(args);

			DeployService deployService = new DeployService(app.Services);
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
					case "EnsureCreated":
						deployService.EnsureCreated();

						return;
					case "EnsureDeleted":
						deployService.EnsureDeleted();
						return;
                    default:
						Console.WriteLine("unkown cmd");
						return;
				}
			}

			app.Run();
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


		


	}
}