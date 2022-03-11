using Asteh.Core.Models;
using Asteh.Domain.Providers.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Filters;
using Asteh.Api.Examples.User;
using System.Net;
using Asteh.Api.Examples;

namespace Asteh.Api.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class UserController : ControllerBase
	{
		private readonly IUserProvider _userProvider;

		public UserController(IUserProvider userProvider)
		{
			_userProvider = userProvider;
		}

		/// <summary>
		/// Get all users from the database
		/// </summary>
		/// <param name="cancellationToken">Cancellation token to stop request</param>
		/// <returns>Users collection</returns>
		/// <response code="200">Successfully get users</response>
		[HttpGet]
		[Produces("application/json")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[SwaggerResponseExample(StatusCodes.Status200OK, typeof(GetUsersResponseExample))]
		public async Task<ActionResult<IEnumerable<UserModel>>> GetUsersAsync(
			CancellationToken cancellationToken)
		{
			// TODO: Just for testing!
			//await Task.Delay(5000);

			var result = await _userProvider.GetUsersAsync(cancellationToken);
			return Ok(result);
		}

		/// <summary>
		/// Find all users from the database satisfying filter
		/// </summary>
		/// <param name="filter">Filter values to get only neccessary users</param>
		/// <param name="cancellationToken">Cancellation token to stop request</param>
		/// <returns>Users collection</returns>
		/// <response code="200">Successfully get filtered users</response>
		[HttpGet("find")]
		[Produces("application/json")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
		[SwaggerResponseExample(StatusCodes.Status200OK, typeof(GetUsersResponseExample))]
		[SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(ErrorExample))]
		public async Task<ActionResult<IEnumerable<UserModel>>> FindUsersAsync(
			[FromQuery] FilterUserModel filter,
			CancellationToken cancellationToken)
		{
			// TODO: Just for testing!
			//await Task.Delay(5000);
			try
			{
				return Ok(await _userProvider.FindUsersAsync(filter, cancellationToken));
			}
			catch (ArgumentException ex)
			{
				return BadRequest(ex.Message);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
	}
}
