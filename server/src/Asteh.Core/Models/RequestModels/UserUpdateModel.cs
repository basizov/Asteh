using System.ComponentModel.DataAnnotations;

namespace Asteh.Core.Models.RequestModels
{
	public class UserUpdateModel
	{
		/// <summary>
		/// New password for user
		/// </summary>
		[Required]
		public string Password { get; set; } = default!;
		/// <summary>
		/// New name for user
		/// </summary>
		[Required]
		public string Name { get; set; } = default!;
		/// <summary>
		/// New type for user with special name
		/// </summary>
		[Required]
		public string TypeName { get; set; } = default!;
		/// <summary>
		/// New last visit date for user
		/// </summary>
		[Required]
		public string LastVisitDate { get; set; } = default!;
	}
}
