using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using MoreNote.Config.ConfigFile;
using MoreNote.Logic.Database;

using System.Text.RegularExpressions;

using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace MoreNote.Database
{
    /// <summary>
    /// 
    /// </summary>
    public static class DatabaseTool
    {

        public static void ConfigDatabase(this IServiceCollection services, DataBaseConfig config)
        {
            if (config.SqlEngine == Models.Enums.SqlEngine.PostgreSQL)
            {
                services.AddDbContextPool<DataContext>((serviceProvider, optionsBuilder) =>
                {
                    optionsBuilder.UseNpgsql(config.PostgreSQL, b => b.MigrationsAssembly("MoreNote.Logic"));


                    optionsBuilder.UseInternalServiceProvider(serviceProvider);
                    //调试环境下面打开慢SQL控制台输出，如果执行时间大于10ms
                    if (1==2)
                    {
                        optionsBuilder.LogTo(eflog =>
                        {
                            //正则表达式 匹配执行时间
                            var match = Regex.Match(eflog, @"Executed DbCommand \((\d+)ms\)");
                            if (match.Success)
                            {
                                var regexGroups = match.Groups;
                                var itemValue = regexGroups[1].ToString();
                                int ms = 0;
                                Int32.TryParse(itemValue, out ms);
                                if (ms > 100)
                                {
                                    Console.WriteLine($"==================Slow database operations,{regexGroups[0]}==================");

                                    Console.WriteLine(eflog);
                                }
                            }
                        }, Microsoft.Extensions.Logging.LogLevel.Warning);
                    }
                });
            }
            else if (config.SqlEngine == Models.Enums.SqlEngine.SQLite3)
            {
                services.AddDbContext<DataContext>(options => options.UseSqlite(config.SQLite3));
            }
            else
            {
                // 配置MySQL数据库及连接池
                services.AddDbContextPool<DataContext>(option =>
                        option.UseMySql(config.MySQL, MySqlServerVersion.Parse("8.0.12")), poolSize: 8);
               
            }

        }

    }
}
