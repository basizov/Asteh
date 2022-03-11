using Asteh.Core.Models;
using Swashbuckle.AspNetCore.Filters;

namespace Asteh.Api.Examples.User
{
	public class GetUsersResponseExample : IExamplesProvider<IEnumerable<UserModel>>
	{
		public IEnumerable<UserModel> GetExamples()
		{
			return new[]
			{
				new UserModel
				{
					Id = 1,
					Login = "test1Login",
					TypeName = "type1",
					Name = "Test1",
					LastVisitDate = new DateTime(2022, 1, 10).ToString("dd.MM.yyyy")
				},
				new UserModel
				{
					Id = 2,
					Login = "test2Login",
					TypeName = "type2",
					Name = "Test2",
					LastVisitDate = new DateTime(2021, 12, 2).ToString("dd.MM.yyyy")
				},
				new UserModel
				{
					Id = 3,
					Login = "test3Login",
					TypeName = "type3",
					Name = "Test3",
					LastVisitDate = new DateTime(2020, 2, 12).ToString("dd.MM.yyyy")
				}
			};
		}
	}
}
