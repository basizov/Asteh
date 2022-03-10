namespace Asteh.Core.Models
{
	public class UserModel
	{
		public int Id { get; init; }
		public string Login { get; init; } = default!;
		public string Name { get; set; } = default!;
		public string TypeName { get; set; } = default!;
		public string LastVisitDate { get; set; } = default!;
	}
}
