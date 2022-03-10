namespace Asteh.Domain.Entities
{
	public class UserTypeEntity
	{
		public int Id { get; set; }
		public string Name { get; set; } = default!;
		public bool AllowEdit { get; set; }
	}
}
