using Microsoft.Playwright;

namespace Asteh.End2EndTesting.PageObjects
{
	public class AuthorizationPageObject
	{
		public AuthorizationPageObject(IBrowser browser)
		{
			Browser = browser;
		}

		public string PagePath => "http://localhost:5000/auth";
		public IBrowser Browser { get; set; } = default!;
		public IPage Page { get; set; } = default!;

		public async Task NavigateAsync()
		{
			Page = await Browser.NewPageAsync();
			await Page.GotoAsync(PagePath);
		}
		public Task SetValidLogin(string username) => Page.FillAsync("#login", username);
		public Task SetValidPassword(string password) => Page.FillAsync("#password", password);
		public Task ClickSignInButton() =>
			Page.RunAndWaitForNavigationAsync(() => Page.ClickAsync("form button"));
	}
}
