namespace Asteh.Core.Models.RequestModels
{
	public class UserCreateModel
	{
		public string Login { get; init; } = default!;
		public string Password { get; set; } = default!;
		public string Name { get; set; } = default!;
		public string TypeName { get; set; } = default!;
	}
}
