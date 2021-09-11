using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoreNote.Middleware.TimeMonitor
{
    public static class TimeMonitorMiddlewareExtensions
    {

        public static IApplicationBuilder UseTimeMonitorMiddleware(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<TimeMonitorMiddleware>();
        }
    }
}
