using Asteh.Core.Models;

namespace Asteh.End2EndTesting.Contexts
{
	public interface IUserApiScenarioContext
	{
		public UserModel User { get; set; }
	}
}
