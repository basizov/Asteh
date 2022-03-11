using System.ComponentModel.DataAnnotations;

namespace Asteh.Core.Models.RequestModels
{
	public class UserCreateModel
	{
		/// <summary>
		/// New user login
		/// </summary>
		[Required]
		public string Login { get; init; } = default!;
		/// <summary>
		/// New user password
		/// </summary>
		[Required]
		public string Password { get; set; } = default!;
		/// <summary>
		/// New user name
		/// </summary>
		[Required]
		public string Name { get; set; } = default!;
		/// <summary>
		/// Existed user type name
		/// </summary>
		[Required]
		public string TypeName { get; set; } = default!;
	}
}
