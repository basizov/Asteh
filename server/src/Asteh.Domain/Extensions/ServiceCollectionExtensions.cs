using Asteh.Domain.Configuration;
using Asteh.Domain.Database;
using Asteh.Domain.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Microsoft.Extensions.DependencyInjection
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddDomainSystem(
			this IServiceCollection services,
			IDataSettings dataSettings)
		{
			return services
				.AddSingleton(dataSettings)
				.AddDbContext<IApplicationContext, ApplicationDbContext>(
					opt => opt.UseSqlite(dataSettings.DbConnectionString))
				.AddTransient<IUnitOfWork, UnitOfWork>();
		}
	}
}
