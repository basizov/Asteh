using Asteh.Core.Services.Users;

namespace Microsoft.Extensions.DependencyInjection
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddCoreSystem(this IServiceCollection services)
		{
			return services
				.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies())
				.AddTransient<IUserService, UserService>();
		}
	}
}
