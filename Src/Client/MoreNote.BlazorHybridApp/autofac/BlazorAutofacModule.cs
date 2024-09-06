using Autofac;

using MoreNote.Common.Utils;
using MoreNote.Config.ConfigFilePath.IMPL;
using MoreNote.Logic.Service;
using MoreNote.Logic.Service.DistributedIDGenerator;
using MoreNote.MauiLib.Services;

using System.Linq;

namespace MoreNote.BlazorHybridApp.autofac
{
	/// <summary>
	/// 容器注册类
	/// </summary>
	public class BlazorAutofacModule : Autofac.Module
	{
        /// <summary>
        /// 容器注册 ContainerBuilder
        /// </summary>
        public static ContainerBuilder builder { get; set; }

		protected override void Load(ContainerBuilder builder)
		{
			

			//分布式id生成器
			builder.RegisterType<SnowFlakeNetService>()
				.As<IDistributedIdGenerator>()
				.SingleInstance();

			builder.RegisterType<BlazorConfigFilePahFinder>().As<IConfigFilePahFinder>();
            builder.RegisterType<ConfigFileService>().SingleInstance();//单例模式 配置文件服务
        }
	}
}