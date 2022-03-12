using Asteh.Core.Models;
using Swashbuckle.AspNetCore.Filters;

namespace Asteh.Api.Examples
{
	public class FullInfoExample : IExamplesProvider<FullInfoModel>
	{
		public FullInfoModel GetExamples()
		{
			return new()
			{
				UserId = 1,
				Users = new[]
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
				},
				UserTypes = new[]
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
				}
			};
		}
	}
}
