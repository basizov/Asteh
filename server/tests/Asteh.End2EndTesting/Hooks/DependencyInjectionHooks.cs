using Asteh.Domain.DataProvider;
using Asteh.End2EndTesting.Contexts;
using BoDi;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TechTalk.SpecFlow;

namespace Asteh.End2EndTesting.Hooks
{
	[Binding]
	public class DependencyInjectionHooks
	{
		private readonly IObjectContainer _objectContainer;

		public DependencyInjectionHooks(IObjectContainer objectContainer)
		{
			_objectContainer = objectContainer;
		}


		[BeforeScenario]
		public void WebApplicationInitializer()
		{
			var appFactory = new WebApplicationFactory<Program>()
				.WithWebHostBuilder(builder =>
				{
					builder.ConfigureServices(services =>
					{
						var descriptor = services.SingleOrDefault(d =>
							d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
						services.Remove(descriptor!);

						services.AddDbContext<ApplicationDbContext>(options =>
						{
							options.UseLazyLoadingProxies()
								.UseInMemoryDatabase("asteh-db");
						});
					});
				});
			_objectContainer.RegisterInstanceAs(appFactory);
		}

		[BeforeScenario]
		public void ContextsInitializer()
		{
			_objectContainer
				.RegisterTypeAs<UserApiScenarioContext, IUserApiScenarioContext>();
		}
	}
}
