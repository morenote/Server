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
        WebSiteConfig config = ConfigFileService.GetWebConfig();

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
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

            //添加session服务
            services.AddDistributedMemoryCache();

            //var connection = "Server=localhost;Port=3306;Database=nickeldb; User=root;Password=123456;";
            //var connection = Environment.GetEnvironmentVariable("postgres");
            //services.AddDbContextPool<DataContext>(options => options.UseNpgsql(connection));
           // services.AddDbContextPool<CarModelContext>(options => options.UseSqlServer(Configuration.GetConnectionString("SQL")));
            
            //添加redis
	        services.AddDistributedRedisCache(options =>
	        {
		        options.Configuration = "localhost";
				
	        });
            services.AddSession(options =>
            {
               
                // Set a short timeout for easy testing.
                options.Cookie.Name = "SessionID";
                options.IdleTimeout = TimeSpan.FromMinutes(10);
                options.Cookie.HttpOnly = true;//设为httponly 阻止js脚本读取
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
