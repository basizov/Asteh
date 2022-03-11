namespace Asteh.Api.Configuration
{
	public class OpenApiDescription
	{
		public OpenApiDescription(
			string title,
			string version,
			string description)
		{
			Title = title;
			Version = version;
			Description = description;
		}

		public string Title { get; init; } = default!;
		public string Version { get; init; } = default!;
		public string Description { get; init; } = default!;
	}
}
