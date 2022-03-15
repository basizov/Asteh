using Asteh.End2EndTesting.PageObjects;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace Asteh.End2EndTesting.Steps
{
	[Binding]
	public class AuthorizationSteps
	{
		private readonly AuthorizationPageObject _authorizationPageObject;

		public AuthorizationSteps(AuthorizationPageObject authorizationPageObject)
		{
			_authorizationPageObject = authorizationPageObject;
		}

		[Given(@"logged out user")]
		public async Task GivenLoggedOutUser() =>
			await _authorizationPageObject.NavigateAsync();

		[When(@"the user attempt to sign in with valid credentials")]
		public async Task WhenTheUserAttemptToSignInWithValidCredentials()
		{
			await _authorizationPageObject.SetValidLogin("Admin1Login");
			await _authorizationPageObject.SetValidPassword("Admin1Pass");
			await _authorizationPageObject.ClickSignInButton();
		}

		[Then(@"successfully signed in")]
		public void ThenSuccessfullySignedIn() =>
			_authorizationPageObject.Page.Url.Should().NotEndWith("/auth");
	}
}
