using Asteh.Domain.DataProvider;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Linq;
using System.Net.Http;

namespace Asteh.Api.IntegrationTests
{
	public class BaseControllerTests
	{
		protected readonly HttpClient Client;

		protected BaseControllerTests()
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
							options.UseInMemoryDatabase("asteh-db"));
					});
				});
			Client = appFactory.CreateClient();
		}
	}
}
