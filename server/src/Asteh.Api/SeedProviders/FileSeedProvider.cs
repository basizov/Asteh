using Asteh.Domain.DataProvider;
using Asteh.Domain.Entities;
using Asteh.Domain.Policies;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Asteh.Api.SeedProviders
{
	public static class FileSeedProvider
	{
		public async static Task SeedUsersAsync(
			this WebApplication app,
			string directorySeriaizingPath)
		{
			using var scope = app.Services.CreateScope();
			var services = scope.ServiceProvider;
			var dbContext = services.GetRequiredService<ApplicationDbContext>();

			await dbContext.Database.MigrateAsync();

			if (!dbContext.UserTypes.Any() && !dbContext.Users.Any())
			{
				var serializerOptions = new JsonSerializerOptions
				{
					PropertyNamingPolicy = SnakeCaseNamingPolicy.SnakeCase
				};

				var userTypesFileName = "UserTypes.json";
				using var openUserTypesStream = File
					.OpenRead(Path.Combine(directorySeriaizingPath, userTypesFileName));
				var userTypes = await JsonSerializer
					.DeserializeAsync<IEnumerable<UserTypeEntity>>(openUserTypesStream, serializerOptions);

				dbContext.UserTypes.AddRange(userTypes!);

				var usersFileName = "Users.json";
				using var openUsersStream = File
					.OpenRead(Path.Combine(directorySeriaizingPath, usersFileName));
				var users = await JsonSerializer
					.DeserializeAsync<IEnumerable<UserEntity>>(openUsersStream, serializerOptions);

				dbContext.Users.AddRange(users!);
				await dbContext.SaveChangesAsync();
			}
		}
	}
}
