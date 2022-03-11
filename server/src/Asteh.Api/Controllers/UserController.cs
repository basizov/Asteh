using Asteh.Api.Examples;
using Asteh.Api.Examples.User;
using Asteh.Core.Models;
using Asteh.Core.Models.RequestModels;
using Asteh.Domain.Providers.Users;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;

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
		/// <response code="400">Some error during get users</response>response>
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
		/// Get user by id from the database
		/// </summary>
		/// <param name="id">User identifier</param>
		/// <param name="cancellationToken">Cancellation token to stop request</param>
		/// <returns>User</returns>
		/// <response code="200">Successfully get user</response>
		/// <response code="400">Some error during get user by id</response>response>
		[HttpGet("{id:int}")]
		[Produces("application/json")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
		[SwaggerResponseExample(StatusCodes.Status200OK, typeof(GetUserResponseExample))]
		[SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(ErrorExample))]
		public async Task<ActionResult<IEnumerable<UserModel>>> GetUserByIdAsync(
			int id,
			CancellationToken cancellationToken)
		{
			// TODO: Just for testing!
			//await Task.Delay(5000);
			try
			{
				return Ok(await _userProvider.GetUserByIdAsync(id, cancellationToken));
			}
			catch (ArgumentException ex)
			{
				return BadRequest(ex.Message);
			}
		}

		/// <summary>
		/// Find all users from the database satisfying filter
		/// </summary>
		/// <param name="filter">Filter values to get only neccessary users</param>
		/// <param name="cancellationToken">Cancellation token to stop request</param>
		/// <returns>Users collection</returns>
		/// <response code="200">Successfully get filtered users</response>
		/// <response code="400">Some error during filtering</response>response>
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
		}

		/// <summary>
		/// Create new user to the database
		/// </summary>
		/// <param name="createModel">Model to create new user</param>
		/// <param name="cancellationToken">Cancellation token to stop request</param>
		/// <returns>Created user</returns>
		/// <response code="201">Successfully create user</response>
		/// <response code="400">Some error during creating</response>
		[HttpPost]
		[Produces("application/json")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
		[SwaggerResponseExample(StatusCodes.Status201Created, typeof(GetUserResponseExample))]
		[SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(ErrorExample))]
		public async Task<ActionResult<UserModel>> CreateUserAsync(
			[FromBody] UserCreateModel createModel,
			CancellationToken cancellationToken)
		{
			// TODO: Just for testing!
			//await Task.Delay(5000);
			try
			{
				var a = nameof(GetUserByIdAsync);
				var id = await _userProvider
					.CreateUserAsync(createModel, cancellationToken);
				return CreatedAtAction(a, new {
					id,
					cancellationToken
				}, createModel);
			}
			catch (ArgumentException ex)
			{
				return BadRequest(ex.Message);
			}
		}

		/// <summary>
		/// Update existed user in the database
		/// </summary>
		/// <param name="id">Existed user identifier</param>
		/// <param name="updateModel">Model to update new user</param>
		/// <param name="cancellationToken">Cancellation token to stop request</param>
		/// <returns>No content</returns>
		/// <response code="204">Successfully updated user</response>response>
		/// <response code="400">Some error during updating</response>response>
		[HttpPut("{id:int}")]
		[Produces("application/json")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
		[SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(ErrorExample))]
		public async Task<ActionResult<UserModel>> UpdateUserAsync(
			[FromRoute] int id,
			[FromBody] UserUpdateModel updateModel,
			CancellationToken cancellationToken)
		{
			// TODO: Just for testing!
			//await Task.Delay(5000);
			try
			{
				await _userProvider
					.UpdateUserAsync(id, updateModel, cancellationToken);
				return NoContent();
			}
			catch (ArgumentException ex)
			{
				return BadRequest(ex.Message);
			}
		}

		/// <summary>
		/// Delete existed user in the database
		/// </summary>
		/// <param name="id">Existed user identifier</param>
		/// <param name="cancellationToken">Cancellation token to stop request</param>
		/// <returns>No content</returns>
		/// <response code="204">Successfully deleted user</response>response>
		/// <response code="400">Some error during deleting</response>response>
		[HttpDelete("{id:int}")]
		[Produces("application/json")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
		[SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(ErrorExample))]
		public async Task<ActionResult<UserModel>> DeleteUserAsync(
			int id,
			CancellationToken cancellationToken)
		{
			// TODO: Just for testing!
			//await Task.Delay(5000);
			try
			{
				await _userProvider
					.DeleteUserAsync(id, cancellationToken);
				return NoContent();
			}
			catch (ArgumentException ex)
			{
				return BadRequest(ex.Message);
			}
		}
	}
}
