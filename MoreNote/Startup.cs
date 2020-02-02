using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace MoreNote
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

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



            //添加session服务
            services.AddDistributedMemoryCache();

            //var connection = "Server=localhost;Port=3306;Database=nickeldb; User=root;Password=123456;";
            //var connection = Environment.GetEnvironmentVariable("postgres");
            //services.AddDbContextPool<DataContext>(options => options.UseNpgsql(connection));
            services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
                options.Cookie.Name = "SessionID";
                options.IdleTimeout = TimeSpan.FromMinutes(10);
                options.Cookie.HttpOnly = true;
            });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        
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
