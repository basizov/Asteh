using Asteh.End2EndTesting.PageObjects;
using BoDi;
using Microsoft.Playwright;
using TechTalk.SpecFlow;

namespace Asteh.End2EndTesting.Hooks
{
	[Binding]
	public class AuthorizationHook
	{
		private readonly IBrowser _browser;

		public AuthorizationHook(IBrowser browser)
		{
			_browser = browser;
		}

		[BeforeScenario("Authorization")]
		public void BeforeAuthorizationScenarioAsync(IObjectContainer container)
		{
			var pageObject = new AuthorizationPageObject(_browser);
			container.RegisterInstanceAs(pageObject);
		}
	}
}
