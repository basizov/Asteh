using Asteh.End2EndTesting.PageObjects;
using BoDi;
using Microsoft.Playwright;
using TechTalk.SpecFlow;

namespace Asteh.End2EndTesting.Hooks
{
	[Binding]
	public class AuthorizationHook
	{
		[BeforeScenario("Authorization")]
		public async Task BeforeAuthorizationScenarioAsync(IObjectContainer container)
		{
			var playwright = await Playwright.CreateAsync();
			var browser = await playwright.Chromium.LaunchAsync();
			var pageObject = new AuthorizationPageObject(browser);
			container.RegisterInstanceAs(playwright);
			container.RegisterInstanceAs(browser);
			container.RegisterInstanceAs(pageObject);
		}

		[AfterScenario]
		public async Task AfterScenarioAsync(IObjectContainer container)
		{
			var browser = container.Resolve<IBrowser>();
			await browser.CloseAsync();

			var playwright = container.Resolve<IPlaywright>();
			playwright.Dispose();
		}
	}
}
