using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MoreNote.Middleware.TimeMonitor
{
    /**
     * API耗时监控
     * */

    public class TimeMonitorMiddleware
    {
       

        private readonly RequestDelegate _next;

        public TimeMonitorMiddleware(RequestDelegate next)
        {
           
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
           Stopwatch stopwatch=new Stopwatch();
            stopwatch.Start();
            context.Response.OnStarting(() =>
            {
                 stopwatch.Stop();
                context.Response.Headers["elapsed"] = stopwatch.ElapsedMilliseconds.ToString();
                return Task.CompletedTask;
            });
            await _next(context);
           
          
             
         
        }
    }
}