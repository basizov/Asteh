using System.ComponentModel.DataAnnotations;

namespace Asteh.Core.Models.RequestModels
{
	public class AuthorizeModel
	{
		/// <summary>
		/// Existed user login
		/// </summary>
		[Required]
		public string Login { get; set; } = default!;
		/// <summary>
		/// Existed user password
		/// </summary>
		[Required]
		public string Password { get; set; } = default!;
	}
}
