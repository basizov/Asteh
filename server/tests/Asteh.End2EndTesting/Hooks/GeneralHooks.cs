using BoDi;
using Microsoft.Playwright;
using TechTalk.SpecFlow;

namespace Asteh.End2EndTesting.Hooks
{
	[Binding]
	public class GeneralHooks
	{
		[BeforeScenario(Order = 0)]
		public async static Task BeforeAuthorizationScenarioAsync(IObjectContainer container)
		{
			var playwright = await Playwright.CreateAsync(); ;
			container.RegisterInstanceAs(playwright);

			var browser = await playwright.Chromium.LaunchAsync();
			container.RegisterInstanceAs(browser);
		}

		[AfterScenario]
		public async static Task AfterScenarioAsync(IObjectContainer container)
		{
			var browser = container.Resolve<IBrowser>();
			await browser.CloseAsync();

			var playwright = container.Resolve<IPlaywright>();
			playwright.Dispose();
		}
	}
}
