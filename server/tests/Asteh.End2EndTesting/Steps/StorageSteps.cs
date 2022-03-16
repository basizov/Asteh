using Asteh.Core.Models.RequestModels;
using Asteh.Domain.DataProvider;
using Asteh.Domain.Entities;
using Asteh.End2EndTesting.Helpers;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using TechTalk.SpecFlow;

namespace Asteh.End2EndTesting.Steps
{
	[Binding]
	public class StorageSteps
	{
		private readonly ApplicationDbContext _dbContext;

		public StorageSteps(WebApplicationFactory<Program> applicationFactory)
		{
			var scope = applicationFactory.Services.CreateScope();
			var dbContext = scope.ServiceProvider
				.GetRequiredService<ApplicationDbContext>();
			_dbContext = dbContext;
		}

		[Given(@"the following userTypes exists in the storage")]
		public async Task GivenTheFollowingUserTypesExistsInTheStorage(
			IEnumerable<UserTypeEntity> userTypes)
		{
			if (_dbContext.UserTypes.Any())
			{
				return;
			}
			foreach (var userType in userTypes)
			{
				_dbContext.UserTypes.Add(new()
				{
					Id = userType.Id,
					Name = userType.Name,
					AllowEdit = userType.AllowEdit
				});
			}
			await _dbContext.SaveChangesAsync();
		}

		[Given(@"the following users exists in the storage")]
		public async Task GivenTheFollowingUsersExistsInTheStorage(
			IEnumerable<UserCreateModel> users)
		{
			if (_dbContext.Users.Any())
			{
				return;
			}
			foreach (var user in users)
			{
				var userType = await _dbContext.UserTypes
					.SingleOrDefaultAsync(d => d.Name == user.TypeName);

				if (userType is null)
				{
					throw new ArgumentException($"UserType with name {user.TypeName} doesn't exists");
				}
				_dbContext.Users.Add(new()
				{
					Login = user.Login,
					Name = user.Name,
					Password = user.Password,
					TypeId = userType.Id,
					LastVisitDate = DateTime.UtcNow
				});
			}
			await _dbContext.SaveChangesAsync();
		}
	}
}
