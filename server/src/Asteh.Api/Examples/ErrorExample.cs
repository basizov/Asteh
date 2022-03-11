using Swashbuckle.AspNetCore.Filters;

namespace Asteh.Api.Examples
{
	public class ErrorExample : IExamplesProvider<string>
	{
		// TODO: Add error entity
		public string GetExamples() => "Error!!";
	}
}
