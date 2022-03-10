namespace Asteh.Core.Models.RequestModels
{
	public class UserUpdateModel
	{
		public string Password { get; set; } = default!;
		public string Name { get; set; } = default!;
		public string TypeName { get; set; } = default!;
		public string LastVisitDate { get; set; } = default!;
	}
}
