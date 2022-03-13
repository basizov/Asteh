using Asteh.Api.Attributes;
using Asteh.Api.Examples;
using Asteh.Api.Examples.Users;
using Asteh.Core.Models;
using Asteh.Core.Models.RequestModels;
using Asteh.Core.Providers.Users;
using Asteh.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;
using System.Net;

namespace Asteh.Api.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class UserController : ControllerBase
	{
		private readonly IUserProvider<UserProvider> _userProvider;
		private readonly IUserProvider<FileUserProvider> _fileUserProvider;

		public UserController(
			IUserProvider<UserProvider> userProvider,
			IUserProvider<FileUserProvider> fileUserProvider)
		{
			_userProvider = userProvider;
			_fileUserProvider = fileUserProvider;
		}

		/// <summary>
		/// Get all users from the database
		/// </summary>
		/// <param name="fromDatabase">Flag to work with db or file</param>
		/// <param name="cancellationToken">Cancellation token to stop request</param>
		/// <returns>Users collection</returns>
		/// <response code="200">Successfully get users</response>
		[HttpGet]
		[Produces("application/json")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[SwaggerResponseExample(StatusCodes.Status200OK, typeof(GetUsersResponseExample))]
		public async Task<ActionResult<IEnumerable<UserModel>>> GetUsersAsync(
			[FromQuery] bool fromDatabase = true,
			CancellationToken cancellationToken = default)
		{
			return Ok(fromDatabase
				? await _userProvider.GetUsersAsync(cancellationToken)
				: await _fileUserProvider.GetUsersAsync(cancellationToken));
		}

		/// <summary>
		/// Get user by id from the database
		/// </summary>
		/// <param name="id">User identifier</param>
		/// <param name="fromDatabase">Flag to work with db or file</param>
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
			[FromQuery] bool fromDatabase = true,
			CancellationToken cancellationToken = default)
		{
			try
			{
				return Ok(fromDatabase
					? await _userProvider.GetUserByIdAsync(id, cancellationToken)
					: await _fileUserProvider.GetUserByIdAsync(id, cancellationToken));
			}
			catch (ArgumentException ex)
			{
				return BadRequest(GetApplicationError(ex.Message));
			}
		}

		/// <summary>
		/// Find all users from the database satisfying filter
		/// </summary>
		/// <param name="filter">Filter values to get only neccessary users</param>
		/// <param name="fromDatabase">Flag to work with db or file</param>
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
			[FromQuery] bool fromDatabase = true,
			CancellationToken cancellationToken = default)
		{
			await Task.Delay(5000, cancellationToken);
			try
			{
				return Ok(fromDatabase
					? await _userProvider.FindUsersAsync(filter, cancellationToken)
					: await _fileUserProvider.FindUsersAsync(filter, cancellationToken));
			}
			catch (ArgumentException ex)
			{
				return BadRequest(GetApplicationError(ex.Message));
			}
		}

		/// <summary>
		/// Create new user to the database
		/// </summary>
		/// <param name="createModel">Model to create new user</param>
		/// <param name="fromDatabase">Flag to work with db or file</param>
		/// <param name="cancellationToken">Cancellation token to stop request</param>
		/// <returns>Created user</returns>
		/// <response code="201">Successfully create user</response>
		/// <response code="400">Some error during creating</response>
		/// <response code="401">User don't have allow_edit=true in type</response>
		[HttpPost]
		[Produces("application/json")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
		[SwaggerResponseExample(StatusCodes.Status201Created, typeof(GetUserResponseExample))]
		[SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(ErrorExample))]
		[SwaggerResponseExample(StatusCodes.Status401Unauthorized, typeof(ErrorExample))]
		[AutorizeAttrubite]
		public async Task<ActionResult<UserModel>> CreateUserAsync(
			[FromBody] UserCreateModel createModel,
			[FromQuery] bool fromDatabase = true,
			CancellationToken cancellationToken = default)
		{
			try
			{
				var a = nameof(GetUserByIdAsync);
				var id = fromDatabase
					? await _userProvider.CreateUserAsync(createModel, cancellationToken)
					: await _fileUserProvider.CreateUserAsync(createModel, cancellationToken);
				return CreatedAtAction(a, new {
					id,
					cancellationToken
				}, createModel);
			}
			catch (ArgumentException ex)
			{
				return BadRequest(GetApplicationError(ex.Message));
			}
		}

		/// <summary>
		/// Update existed user in the database
		/// </summary>
		/// <param name="id">Existed user identifier</param>
		/// <param name="updateModel">Model to update new user</param>
		/// <param name="fromDatabase">Flag to work with db or file</param>
		/// <param name="cancellationToken">Cancellation token to stop request</param>
		/// <returns>No content</returns>
		/// <response code="204">Successfully updated user</response>response>
		/// <response code="400">Some error during updating</response>response>
		/// <response code="401">User don't have allow_edit=true in type</response>
		[HttpPut("{id:int}")]
		[Produces("application/json")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
		[SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(ErrorExample))]
		[SwaggerResponseExample(StatusCodes.Status401Unauthorized, typeof(ErrorExample))]
		[AutorizeAttrubite]
		public async Task<ActionResult<UserModel>> UpdateUserAsync(
			[FromRoute] int id,
			[FromBody] UserUpdateModel updateModel,
			[FromQuery] bool fromDatabase = true,
			CancellationToken cancellationToken = default)
		{
			try
			{
				if (fromDatabase)
				{
					await _userProvider.UpdateUserAsync(id, updateModel, cancellationToken);
				}
				else
				{
					await _fileUserProvider.UpdateUserAsync(id, updateModel, cancellationToken);
				}
				return NoContent();
			}
			catch (ArgumentException ex)
			{
				return BadRequest(GetApplicationError(ex.Message));
			}
		}

		/// <summary>
		/// Delete existed user in the database
		/// </summary>
		/// <param name="id">Existed user identifier</param>
		/// <param name="fromDatabase">Flag to work with db or file</param>
		/// <param name="cancellationToken">Cancellation token to stop request</param>
		/// <returns>No content</returns>
		/// <response code="204">Successfully deleted user</response>response>
		/// <response code="400">Some error during deleting</response>response>
		/// <response code="401">User don't have allow_edit=true in type</response>
		[HttpDelete("{id:int}")]
		[Produces("application/json")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
		[SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(ErrorExample))]
		[SwaggerResponseExample(StatusCodes.Status401Unauthorized, typeof(ErrorExample))]
		[AutorizeAttrubite]
		public async Task<ActionResult<UserModel>> DeleteUserAsync(
			int id,
			[FromQuery] bool fromDatabase = true,
			CancellationToken cancellationToken = default)
		{
			try
			{
				if (fromDatabase)
				{
					await _userProvider.DeleteUserAsync(id, cancellationToken);
				}
				else
				{
					await _fileUserProvider.DeleteUserAsync(id, cancellationToken);
				}
				return NoContent();
			}
			catch (ArgumentException ex)
			{
				return BadRequest(GetApplicationError(ex.Message));
			}
		}

		private static ApplicationError GetApplicationError(string message) =>
			new(message, HttpStatusCode.BadRequest);
	}
}
