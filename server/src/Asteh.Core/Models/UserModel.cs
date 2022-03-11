using System.ComponentModel.DataAnnotations;

namespace Asteh.Core.Models
{
	public class UserModel
	{
		/// <summary>
		/// User identifier
		/// </summary>
		[Required]
		public int Id { get; init; }
		/// <summary>
		/// User uniq login
		/// </summary>
		[Required]
		public string Login { get; init; } = default!;
		/// <summary>
		/// User fullname
		/// </summary>
		[Required]
		public string Name { get; set; } = default!;
		/// <summary>
		/// Name of the UserType
		/// </summary>
		[Required]
		public string TypeName { get; set; } = default!;
		/// <summary>
		/// Last date when user was visited
		/// </summary>
		[Required]
		public string LastVisitDate { get; set; } = default!;
	}
}
