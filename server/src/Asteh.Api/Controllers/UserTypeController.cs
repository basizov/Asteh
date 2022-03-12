using Asteh.Api.Examples.UserTypes;
using Asteh.Core.Models;
using Asteh.Core.Providers.UserTypes;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;

namespace Asteh.Api.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class UserTypeController : ControllerBase
	{
		private readonly IUserTypeProvider<UserTypeProvider> _userTypeProvider;
		private readonly IUserTypeProvider<FileUserTypeProvider> _fileUserTypeProvider;

		public UserTypeController
			(IUserTypeProvider<UserTypeProvider> userTypeProvider,
			IUserTypeProvider<FileUserTypeProvider> fileUserTypeProvider)
		{
			_userTypeProvider = userTypeProvider;
			_fileUserTypeProvider = fileUserTypeProvider;
		}

		/// <summary>
		/// Get all user types from the database
		/// </summary>
		/// <param name="fromDatabase">Flag to work with db or file</param>
		/// <param name="cancellationToken">Cancellation token to stop request</param>
		/// <returns>User types</returns>
		/// <response code="200">Successfully get users</response>
		[HttpGet]
		[Produces("application/json")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[SwaggerResponseExample(StatusCodes.Status200OK, typeof(GetUserTypesResponseExample))]
		public async Task<ActionResult<IEnumerable<UserTypeModel>>> GetUserTypesAsync(
			[FromQuery] bool fromDatabase = true,
			CancellationToken cancellationToken = default)
		{
			return Ok(fromDatabase
				? await _userTypeProvider.GetUserTypesAsync(cancellationToken)
				: await _fileUserTypeProvider.GetUserTypesAsync(cancellationToken));
		}
	}
}
