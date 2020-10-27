using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace MoreNote
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //启动服务器
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)

                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                   .UseStartup<Startup>();
                })
                ;
    }
}