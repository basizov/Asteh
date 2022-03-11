namespace Asteh.Api.Configuration
{
	public class CreatorContacts
	{
		public CreatorContacts(
			string email,
			string name,
			string url)
		{
			Email = email;
			Name = name;
			Url = url;
		}

		public string Email { get; init; } = default!;
		public string Name { get; init; } = default!;
		public string Url { get; init; } = default!;
	}
}
