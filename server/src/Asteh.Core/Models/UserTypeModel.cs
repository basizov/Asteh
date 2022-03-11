using System.ComponentModel.DataAnnotations;

namespace Asteh.Core.Models
{
	public class UserTypeModel
	{
		/// <summary>
		/// User type identifie
		/// </summary>
		[Required]
		public int Id { get; set; }
		/// <summary>
		/// User type name
		/// </summary>
		[Required]
		public string Name { get; set; } = default!;
		/// <summary>
		/// User credentials for CRUD operations
		/// </summary>
		[Required]
		public bool AllowEdit { get; set; }
	}
}
