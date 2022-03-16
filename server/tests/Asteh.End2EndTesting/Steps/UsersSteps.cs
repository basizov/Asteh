using Asteh.End2EndTesting.Contexts;
using Asteh.End2EndTesting.Helpers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Refit;
using TechTalk.SpecFlow;

namespace Asteh.End2EndTesting.Steps
{
	[Binding]
	public class UsersSteps
	{
		private readonly IUserEndpoints _userEndpoints;
		private readonly IUserApiScenarioContext _scenarioContext;

		public UsersSteps(
			WebApplicationFactory<Program> applicationFactory,
			IUserApiScenarioContext scenarioContext)
		{
			_userEndpoints = RestService
				.For<IUserEndpoints>(applicationFactory.CreateClient());
			_scenarioContext = scenarioContext;
		}

		[When(@"I request user belonging to (.*) Id")]
		public async Task WhenIRequestUserBelongingToId(int id)
		{
			var user = await _userEndpoints.GetUserByIdAsync(id);
			_scenarioContext.User = user;
		}

		[Then(@"the response Login is '(.*)'")]
		public void ThenTheResponseLoginIs(string login)
		{
			_scenarioContext.User.Should().NotBeNull();
			_scenarioContext.User.Login.Should().Be(login);
			_scenarioContext.User.TypeName.Should().NotBe("-");
		}
	}
}
