namespace Asteh.Domain.Entities
{
	public class UserEntity
	{
		public int Id { get; init; }
		public string Login { get; init; } = default!;
		public string Password { get; set; } = default!;
		public string Name { get; set; } = default!;
		public int TypeId { get; set; }
		public virtual UserTypeEntity? Type { get; init; }
		public DateTime LastVisitDate { get; set; }
	}
}
