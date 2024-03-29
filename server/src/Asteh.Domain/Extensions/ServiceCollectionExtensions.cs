﻿using Asteh.Domain.Configuration;
using Asteh.Domain.DataProvider;
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
				.AddDbContext<ApplicationDbContext>(opt =>
				{
					opt.UseLazyLoadingProxies()
						.UseSqlite(dataSettings.DbConnectionString)
						.UseSnakeCaseNamingConvention();
				})
				.AddTransient<IUnitOfWork, UnitOfWork>();
		}
	}
}
