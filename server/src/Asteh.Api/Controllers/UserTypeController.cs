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
		private readonly IUserTypeProvider _userTypeProvider;

		public UserTypeController(IUserTypeProvider userTypeProvider)
		{
			_userTypeProvider = userTypeProvider;
		}

		/// <summary>
		/// Get all user types from the database
		/// </summary>
		/// <param name="cancellationToken">Cancellation token to stop request</param>
		/// <returns>User types</returns>
		/// <response code="200">Successfully get users</response>
		[HttpGet]
		[Produces("application/json")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[SwaggerResponseExample(StatusCodes.Status200OK, typeof(GetUserTypesResponseExample))]
		public async Task<ActionResult<IEnumerable<UserTypeModel>>> GetUserTypesAsync(
			CancellationToken cancellationToken)
		{
			// TODO: Just for testing!
			//await Task.Delay(5000);
			return Ok(await _userTypeProvider
				.GetUserTypesAsync(cancellationToken));
		}
	}
}
