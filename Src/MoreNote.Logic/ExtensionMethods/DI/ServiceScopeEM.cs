using Microsoft.Extensions.DependencyInjection;

using MoreNote.Logic.Database;

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
