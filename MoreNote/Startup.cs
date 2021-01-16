using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MoreNote.Logic.Entity.ConfigFile;
using MoreNote.Logic.DB;

using System;
using Microsoft.AspNetCore.Mvc.Filters;
using MoreNote.Filter.Global;
using MoreNote.Logic.Service;

namespace MoreNote
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        WebSiteConfig config;

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            ConfigFileService configFileService = new ConfigFileService();
            config = configFileService.GetWebConfig();
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => false;//关闭GDPR规范    
                options.MinimumSameSitePolicy = SameSiteMode.Unspecified;

            });
      
            //随机图片API初始化程序
            if (config!=null&&config.Spiders!=null&&config.Spiders.CrawlerWorker)
            {
                services.AddHostedService<MoreNoteWorkerService.RandomImagesCrawlerWorker>();
            }
            if (config!=null&&config.PublicAPI!=null&&config.PublicAPI.RandomImageAPI)
            {
                services.AddHostedService<MoreNoteWorkerService.UpdataImageURLWorker>();
                //网络分析和权重
                //services.AddHostedService<MoreNoteWorkerService.AnalysisOfNetwork>();
            }

           
            //增加数据库
            var connection =config.PostgreSql.Connection;
            services.AddEntityFrameworkNpgsql();
            services.AddDbContextPool<DataContext>((serviceProvider,optionsBuilder) =>
            { 
                optionsBuilder.UseNpgsql(connection);
                optionsBuilder.UseInternalServiceProvider(serviceProvider);
               
                });
           // services.AddDbContextPool<CarModelContext>(options => options.UseSqlServer(Configuration.GetConnectionString("SQL")));
            
           
            //services.AddDistributedMemoryCache();
            //使用Redis分布式缓存
	        services.AddDistributedRedisCache(options =>
	        {
		        options.Configuration = "localhost";
            });
            //增加Session
            services.AddSession(options =>
            {
               
                // Set a short timeout for easy testing.
                options.Cookie.Name = "SessionID";
                options.IdleTimeout = TimeSpan.FromMinutes(10);//过期时间
                options.Cookie.HttpOnly = true;//设为HttpOnly 阻止js脚本读取
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
            services.AddMvc(option => {
                option.Filters.Add<InspectionInstallationFilter>();
            });
            
            //依赖注入的对象
            services.AddTransient(typeof(AccessService));
            services.AddTransient(typeof(AlbumService));
            services.AddTransient(typeof(APPStoreInfoService));
            services.AddTransient(typeof(AttachService));
            services.AddTransient(typeof(AuthService));
            services.AddTransient(typeof(BlogService));
            services.AddTransient(typeof(CommonService));
            services.AddTransient(typeof(ConfigFileService));
            services.AddTransient(typeof(ConfigService));
            services.AddTransient(typeof(EmailService));
            services.AddTransient(typeof(GoogleAuthenticatorService));
            services.AddTransient(typeof(GroupService));
            services.AddTransient(typeof(InitServices));
            services.AddTransient(typeof(NotebookService));
            services.AddTransient(typeof(NoteContentHistoryService));
            services.AddTransient(typeof(NoteContentService));
            services.AddTransient(typeof(NoteFileService));
            services.AddTransient(typeof(NoteImageService));
            services.AddTransient(typeof(NoteService));
            services.AddTransient(typeof(PwdService));
            services.AddTransient(typeof(RandomImageService));
            services.AddTransient(typeof(SessionService));
            services.AddTransient(typeof(ShareService));
            services.AddTransient(typeof(SpamService));
            services.AddTransient(typeof(SuggestionService));
            services.AddTransient(typeof(TagService));
            services.AddTransient(typeof(ThemeService));
            services.AddTransient(typeof(TokenSerivce));
            services.AddTransient(typeof(UpgradeService));
            services.AddTransient(typeof(UserService));
            services.AddTransient(typeof(UserService));
            
            services.AddSingleton(typeof(DependencyInjectionService));
            
           // DependencyInjectionService.IServiceProvider = services.BuildServiceProvider();
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //app.UseDeveloperExceptionPage();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            // app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            //使用session 注册
            app.UseSession();
            //app.UseMvc(routes =>
            //{
            //    routes.MapRoute(
            //        name: "default",
            //        template: "{controller=Home}/{action=Index}/{id?}");
            //});
            app.UseHttpsRedirection();
            app.UseRouting();
         
            app.UseAuthentication();
            app.UseAuthorization();
           
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Blog}/{action=Index}/{id?}");
            });

        }
    }
}
