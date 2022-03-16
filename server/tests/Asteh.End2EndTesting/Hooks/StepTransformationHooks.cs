using Asteh.Core.Models.RequestModels;
using Asteh.Domain.Entities;
using System.Globalization;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Asteh.End2EndTesting.Hooks
{
	[Binding]
	public class StepTransformationHooks
	{
		[StepArgumentTransformation]
		public static IEnumerable<UserCreateModel> TableToUserList(Table table) =>
			table.CreateSet<UserCreateModel>();

		[StepArgumentTransformation]
		public static IEnumerable<UserTypeEntity> TableToUserTypeList(Table table) =>
			table.CreateSet<UserTypeEntity>();
	}
}
