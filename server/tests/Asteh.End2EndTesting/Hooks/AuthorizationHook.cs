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
		public async static Task BeforeAuthorizationScenarioAsync(IObjectContainer container)
		{
			var playwright = await Playwright.CreateAsync(); ;
			container.RegisterInstanceAs(playwright, dispose: true);

			var browser = await playwright.Chromium.LaunchAsync();
			container.RegisterInstanceAs(browser);

			var pageObject = new AuthorizationPageObject(browser);
			container.RegisterInstanceAs(pageObject);
		}

		[AfterScenario("Authorization")]
		public async Task AfterAuthorizationScenatio(IObjectContainer container)
		{
			var browser = container.Resolve<IBrowser>();
			await browser.CloseAsync();

			var	playwrght = container.Resolve<IPlaywright>();
			playwrght.Dispose();
		}
	}
}
