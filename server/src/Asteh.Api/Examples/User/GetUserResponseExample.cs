using Asteh.Core.Models;
using Swashbuckle.AspNetCore.Filters;

namespace Asteh.Api.Examples.User
{
	public class GetUserResponseExample : IExamplesProvider<UserModel>
	{
		public UserModel GetExamples()
		{
			return new UserModel
			{
				Id = 1,
				Login = "test1Login",
				TypeName = "type1",
				Name = "Test1",
				LastVisitDate = new DateTime(2019, 2, 4).ToString("dd.MM.yyyy")
			};
		}
	}
}
