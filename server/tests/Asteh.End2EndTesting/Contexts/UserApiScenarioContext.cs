using Asteh.Core.Models;

namespace Asteh.End2EndTesting.Contexts
{
	public class UserApiScenarioContext : IUserApiScenarioContext
	{
		public UserModel User { get; set; } = default!;
	}
}
