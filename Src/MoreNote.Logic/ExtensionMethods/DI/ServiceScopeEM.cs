using Microsoft.Extensions.DependencyInjection;
using MoreNote.Logic.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Logic.ExtensionMethods.DI
{
    public static class ServiceScopeEM
    {
        public static DataContext GetDataContext(this IServiceScope serviceScope)
        {
            return serviceScope.ServiceProvider.GetRequiredService<DataContext>();

        }
     
    }
}
