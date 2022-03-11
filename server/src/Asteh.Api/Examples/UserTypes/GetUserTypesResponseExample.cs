using Asteh.Core.Models;
using Swashbuckle.AspNetCore.Filters;

namespace Asteh.Api.Examples.UserTypes
{
	public class GetUserTypesResponseExample : IExamplesProvider<IEnumerable<UserTypeModel>>
	{
		public IEnumerable<UserTypeModel> GetExamples()
		{
			return new[]
			{
				new UserTypeModel
				{
					Id = 1,
					Name = "Test1",
					AllowEdit = false
				},
				new UserTypeModel
				{
					Id = 2,
					Name = "Test2",
					AllowEdit = false
				},
				new UserTypeModel
				{
					Id = 3,
					Name = "Test3",
					AllowEdit = false
				}
			};
		}
	}
}
