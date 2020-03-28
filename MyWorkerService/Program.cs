using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MyWorkerService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Thread thread = new Thread(Run);
            thread.Start();
            
        }
        private static void Run()
        {
            string[] args = Array.Empty<string>();

            var host = CreateHostBuilder(args).Build();
            host.Run();
            host.StopAsync();
        }
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>();
                    services.AddHostedService<Worker>();
                  
                });
    }
}
