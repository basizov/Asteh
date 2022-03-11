using Asteh.Core.Providers.Users;
using Asteh.Domain.Providers.Users;

namespace Microsoft.Extensions.DependencyInjection
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddCoreSystem(this IServiceCollection services)
		{
			return services
				.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies())
				.AddTransient<IUserProvider<FileUserProvider>, FileUserProvider>()
				.AddTransient<IUserProvider<UserProvider>, UserProvider>();
		}
	}
}
