using Autofac;
using Autofac.Extensions.DependencyInjection;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using MoreNote.BlazorHybridApp.autofac;
using MoreNote.Logic.Database;
using MoreNote.MauiLib.Utils;

namespace MoreNote.BlazorHybridApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();
            builder.Services.AddAntDesign();
            // services.AddDbContext<DataContext>(options => options.UseSqlite(config.SQLite3));
            builder.Services.AddDbContext<DataContext>(options => options.UseSqlite(MyPathUtil.SQLiteFile));

            builder.ConfigureContainer<ContainerBuilder>(new AutofacServiceProviderFactory(), builder =>
            {
                builder.RegisterModule<BlazorAutofacModule>();
            });

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
