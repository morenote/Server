using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MoreNote.Common.Config;
using MoreNote.Common.Config.Model;
using MoreNote.Logic.DB;

using System;

namespace MoreNote
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        WebSiteConfig config = ConfigManager.GetWebConfig();

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => false;//关闭GDPR规范    
                options.MinimumSameSitePolicy = SameSiteMode.None;

            });
            //随机图片API初始化程序
            if (config.CrawlerWorker)
            {
                services.AddHostedService<MoreNoteWorkerService.RandomImagesCrawlerWorker>();
            }
            if (config.RandomAPI)
            {
                services.AddHostedService<MoreNoteWorkerService.UpdataImageURLWorker>();
            }
            //添加session服务
            services.AddDistributedMemoryCache();

            //var connection = "Server=localhost;Port=3306;Database=nickeldb; User=root;Password=123456;";
            //var connection = Environment.GetEnvironmentVariable("postgres");
            //services.AddDbContextPool<DataContext>(options => options.UseNpgsql(connection));
           // services.AddDbContextPool<CarModelContext>(options => options.UseSqlServer(Configuration.GetConnectionString("SQL")));
            services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
                options.Cookie.Name = "SessionID";
                options.IdleTimeout = TimeSpan.FromMinutes(10);
                options.Cookie.HttpOnly = true;
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
            services.AddMvc();

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
