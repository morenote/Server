using Microsoft.AspNetCore.Builder;

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
