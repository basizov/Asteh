using Asteh.Core.Providers.Users;
using Asteh.Core.Providers.UserTypes;
using Asteh.Core.Services.Authorize;

namespace Microsoft.Extensions.DependencyInjection
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddCoreSystem(this IServiceCollection services)
		{
			return services
				.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies())
				.AddTransient<IAuthorizeService<AuthorizeService>, AuthorizeService>()
				.AddTransient<IAuthorizeService<FileAuthorizeService>, FileAuthorizeService>()
				.AddTransient<IUserTypeProvider<FileUserTypeProvider>, FileUserTypeProvider>()
				.AddTransient<IUserTypeProvider<UserTypeProvider>, UserTypeProvider>()
				.AddTransient<IUserProvider<FileUserProvider>, FileUserProvider>()
				.AddTransient<IUserProvider<UserProvider>, UserProvider>();
		}
	}
}
