using Autofac;
using Autofac.Core;
using Autofac.Extensions.DependencyInjection;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

using Morenote.Framework.Filter.Global;
using Morenote.Framework.Middleware.DigitalEnvelopeMd;

using MoreNote.autofac;
using MoreNote.Config.ConfigFile;
using MoreNote.Config.ConfigFilePath.IMPL;
using MoreNote.Database;
using MoreNote.Logic.Database;
using MoreNote.Logic.Service;
using MoreNote.SignatureService.NetSign;

using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace MoreNote
{
	public class Startup
	{
		private WebSiteConfig config;
	
		

		public Startup( )
		{
			
			
			ConfigFileService configFileService = new ConfigFileService(new ServerConfigFilePahFinder());
			config = configFileService.ReadConfig();
		}

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			// 替换控制器的替换规则
			services.Replace(ServiceDescriptor.Transient<IControllerActivator, ServiceBasedControllerActivator>());

			services.Configure<CookiePolicyOptions>(options =>
			{
				// This lambda determines whether user consent for non-essential cookies is needed for a given request.
				options.CheckConsentNeeded = context => false;//关闭GDPR规范
				options.MinimumSameSitePolicy = SameSiteMode.Unspecified;
			});
			//https://www.npgsql.org/doc/release-notes/6.0.html#breaking-changes
			//https://wangye.org/posts/2021/11/website-migrate-from-aspnet-core-5-to-6.html
			/**
             * 对于PostgreSQL数据库来说UTC时间戳以timestamptz类型表示，对于.NET来说UTC时间戳以带Kind=Utc的DateTime类型表示，
             * 值得注意的是DateTime这种类型在CLR类型中既可以表示UTC时间戳也可以表示非UTC时间戳，这就存在.NET的DateTime类型到
             * PostgreSQL的timestamp和timestamptz转换的混淆，我们必须在EF Core中做出配置来决定如何转换，旧版本的Npgsql对于
             * 这种转换往往是隐式的，这往往会带来一些不可预料的程序bug或者安全问题，现在新版本的Npgsql对此要求显式指派时间戳或
             * 者日期类型。
             * 解决方法有两个，通过在迁移文件里加入代码migrationBuilder.Sql("SET TimeZone='UTC'");并重新执行迁移程序，或者
             * 告诉Npgsql采用旧的兼容模式，具体通过在配置Npgsql服务前加上如下代码：
             */
			AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
			AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);

			//解决Multipart body length limit 134217728 exceeded
			services.Configure<FormOptions>(x =>
			{
				x.ValueLengthLimit = int.MaxValue;
				x.MultipartBodyLengthLimit = int.MaxValue; // In case of multipart
			});

			//增加数据库
			var connection = config.DataBaseConfig.PostgreSQL;
			services.AddMemoryCache();

			services.AddEntityFrameworkNpgsql();
			Console.WriteLine($"================== SqlEngine { Enum.GetName(config.DataBaseConfig.SqlEngine)} ==================");

            services.ConfigDatabase(config.DataBaseConfig);
			// services.AddDbContextPool<CarModelContext>(options => options.UseSqlServer(Configuration.GetConnectionString("SQL")));

			//增加Session
			services.AddSession(options =>
			{
				// Set a short timeout for easy testing.
				options.Cookie.Name = "SessionID";
				options.IdleTimeout = TimeSpan.FromDays(30);//过期时间
				options.Cookie.HttpOnly = true;//设为HttpOnly 阻止js脚本读取
				options.Cookie.Domain = config.APPConfig.Domain;//
				options.Cookie.SameSite = SameSiteMode.Lax;//
			});
			//这样可以将HttpContext注入到控制器中。
			services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
			//services.AddAuthentication("CookieAuthentication")
			//                .AddCookie("CookieAuthentication", config =>
			//                {
			//                    config.Cookie.Name = "UserLoginCookie";
			//                    config.LoginPath = "/Auth/login";

			//                });
			services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
			.AddCookie(options =>
			{
				options.LoginPath = new PathString("/Auth/login");
				options.AccessDeniedPath = new PathString("/Auth/login");
			});
			services.AddAuthorization(options =>
			{
				options.AddPolicy("EmployeeOnly", policy =>
				{
					//policy.AddAuthenticationSchemes("Cookie, Bearer");
					policy.RequireAuthenticatedUser();
					policy.RequireRole("Admin");
					policy.RequireClaim("EmployeeNumber");
				});
			});
			// services.AddMvc();
			// services.AddSingleton<IAuthorizationFilter, InspectionInstallationFilter>();
			services.AddMvc(option =>
			{
				option.Filters.Add<InspectionInstallationFilter>();
			});
			services.AddBundling()
				
					.UseNUglify()
					.EnableMinification()
					.EnableChangeDetection()
					.EnableCacheHeader(TimeSpan.FromHours(1));

			//services.AddSevenZipCompressor();
			//services.AddResumeFileResult();
			if (config.SecurityConfig.FIDO2Config.IsEnable)
			{
				var fido2Config = config.SecurityConfig.FIDO2Config;
				services.AddFido2(option =>
				{
					option.ServerDomain = fido2Config.ServerDomain;
					option.ServerName = fido2Config.ServerName;
					option.Origins = fido2Config.Origin.ToHashSet<string>();
				})
				 .AddCachedMetadataService(

					   config =>
					   {
						   config.AddFidoMetadataRepository();
					   }
				 );
			}
			services.AddControllers().AddControllersAsServices();


			services.AddEndpointsApiExplorer();
			services.AddSwaggerGen(options =>
			{
				options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "My API", Version = "v1" });


				// using System.Reflection;
				var xmlFilename = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
				options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
			});
			services.AddResponseCaching();
            //services.AddControllers().AddNewtonsoftJson();//使用Newtonsoft作为序列化工具
            // DependencyInjectionService.IServiceProvider = services.BuildServiceProvider();
            //services.AddHttpApi<ISJJApi>();

            services.Configure<KestrelServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });
            //配置跨域处理，允许所有来源：
            services.AddCors(options =>
            options.AddPolicy("自定义的跨域策略名称",
            p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod())
            );
        }

		public void ConfigureContainer(ContainerBuilder builder)
		{
			builder.RegisterModule(new AutofacModule());

		}

		public void Configure(WebApplication app)
		{
			//app.UseDeveloperExceptionPage();
			if (app.Environment.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				app.UseHsts();
			}

			// app.UseHttpsRedirection();
			// app.UseHttpsRedirection();
			app.UseStaticFiles();
            app.UseFileServer();
            app.UseCookiePolicy();
			//使用session 注册
			app.UseSession();
			//app.UseHttpsRedirection();
			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

            //监控接口耗时情况
            //app.UseTimeMonitorMiddleware();
            //调试的时候允许跨域
            //app.UseCors(builder =>
            //{
            //	builder.WithOrigins("http://localhost:3201",
            //						"http://app.morenote.top",
            //						"https://app.morenote.top")

            //						   .AllowAnyMethod()
            //						   .AllowAnyHeader()
            //						  ;
            //});

            
            //启用响应缓存
            app.UseResponseCaching();
			app.UseSwagger();
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Demo v1");
			});
			app.UseDigitalEnvelope();

            app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
						pattern: "{controller=Home}/{action=Index}/{id?}");
			});
		}
	}
}