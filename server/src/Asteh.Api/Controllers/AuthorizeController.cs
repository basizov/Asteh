using Asteh.Api.Examples;
using Asteh.Core.Helpers;
using Asteh.Core.Models;
using Asteh.Core.Models.RequestModels;
using Asteh.Core.Services.Authorize;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;

namespace Asteh.Api.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class AuthorizeController : ControllerBase
	{
		private readonly IAuthorizeService<AuthorizeService> _authorizeService;
		private readonly IAuthorizeService<FileAuthorizeService> _fileAuthorizeService;

		public AuthorizeController(
			IAuthorizeService<AuthorizeService> authorizeService,
			IAuthorizeService<FileAuthorizeService> fileAuthorizeService)
		{
			_authorizeService = authorizeService;
			_fileAuthorizeService = fileAuthorizeService;
		}

		/// <summary>
		/// Endpoint to get all neccessary information
		/// </summary>
		/// <param name="fromDatabase">Flag to work with db or file</param>
		/// <param name="cancellationToken">Cancellation token to stop request</param>
		/// <returns></returns>
		/// <response code="200">Successfully get neccessary information</response>
		[HttpGet]
		[Produces("application/json")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[SwaggerResponseExample(StatusCodes.Status200OK, typeof(FullInfoExample))]
		public async Task<ActionResult<FullInfoModel>> GetFullNeccessaryInfoAsync(
			[FromQuery] bool fromDatabase = true,
			CancellationToken cancellationToken = default)
		{
			var fullInfo = fromDatabase
				? await _authorizeService.GetFullInfoModelToAuthorizeUser(cancellationToken)
				: await _fileAuthorizeService.GetFullInfoModelToAuthorizeUser(cancellationToken);

			var allowAcces = Request.Cookies[Constants.CookieAllowAccessTab];
			if (allowAcces is not null &&
				allowAcces.Equals(Constants.AllowIndicator))
			{
				fullInfo.IsAccessEnabled = true;
			}
			return Ok(fullInfo);
		}

		/// <summary>
		/// Endpoint to authorize user credentials
		/// </summary>
		/// <param name="authorizeModel">Model to authorize</param>
		/// <param name="fromDatabase">Flag to work with db or file</param>
		/// <param name="cancellationToken">Cancellation token to stop request</param>
		/// <returns></returns>
		/// <response code="200">Successfully authorize user</response>
		/// <response code="404">Invalid user login or password</response>
		[HttpPost]
		[Produces("application/json")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
		[SwaggerResponseExample(StatusCodes.Status200OK, typeof(FullInfoExample))]
		[SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(string))]
		public async Task<ActionResult<FullInfoModel>> AuthorizeUserAsync(
			AuthorizeModel authorizeModel,
			[FromQuery] bool fromDatabase = true,
			CancellationToken cancellationToken = default)
		{
			var fullInfo = fromDatabase
				? await _authorizeService.AuthorizeUserAsync(authorizeModel, cancellationToken)
				: await _fileAuthorizeService.AuthorizeUserAsync(authorizeModel, cancellationToken);
			if (fullInfo is null)
			{
				return NotFound($"Invalid user credentials");
			}

			var cookieOptions = new CookieOptions
			{
				HttpOnly = true,
				Expires = DateTime.UtcNow.AddDays(7)
			};
			Response.Cookies.Append(
				Constants.CookieAllowAccessTab,
				fullInfo.IsAccessEnabled
					? Constants.AllowIndicator
					: Constants.NoAllowIndicator,
				cookieOptions);

			return Ok(fullInfo);
		}
	}
}
