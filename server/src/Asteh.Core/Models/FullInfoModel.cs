using System.ComponentModel.DataAnnotations;

namespace Asteh.Core.Models
{
	public class FullInfoModel
	{
		/// <summary>
		/// Success authorized user id
		/// </summary>
		public int? UserId { get; set; }
		/// <summary>
		/// Flag that indecates user to have permissions for
		/// Create, Update and Delete users accounts
		/// </summary>
		[Required]
		public bool IsAccessEnabled { get; set; }
		/// <summary>
		/// All users in the app;ication
		/// </summary>
		[Required]
		public IEnumerable<UserModel> Users { get; set; } = default!;
		/// <summary>
		/// All user types in the app;ication
		/// </summary>
		[Required]
		public IEnumerable<UserTypeModel> UserTypes { get; set; } = default!;
	}
}
