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
		private readonly IAuthorizeService _authorizeService;

		public AuthorizeController(IAuthorizeService authorizeService)
		{
			_authorizeService = authorizeService;
		}

		/// <summary>
		/// Endpoint to get all neccessary information
		/// </summary>
		/// <param name="cancellationToken">Cancellation token to stop request</param>
		/// <returns></returns>
		/// <response code="200">Successfully get neccessary information</response>
		[HttpGet]
		[Produces("application/json")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[SwaggerResponseExample(StatusCodes.Status200OK, typeof(FullInfoExample))]
		public async Task<ActionResult<FullInfoModel>> GetFullNeccessaryInfoAsync(
			CancellationToken cancellationToken)
		{
			var fullInfo = await _authorizeService
				.GetFullInfoModelToAuthorizeUser(cancellationToken);

			var allowAcces = Request.Cookies[Constants.CookieAllowAccessTab];
			if (allowAcces is not null &&
				allowAcces.Equals(Constants.AllowIndicator))
			{
				fullInfo.IsAccessEnabled = true;
			}
			return Ok();
		}

		/// <summary>
		/// Endpoint to authorize user credentials
		/// </summary>
		/// <param name="authorizeModel">Model to authorize</param>
		/// <param name="cancellationToken">Cancellation token to stop request</param>
		/// <returns></returns>
		/// <response code="200">Successfully authorize user</response>
		/// <response code="404">Invalid user login or password</response>
		[HttpPost]
		[Produces("application/json")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
		[SwaggerResponseExample(StatusCodes.Status200OK, typeof(FullInfoExample))]
		public async Task<ActionResult<FullInfoModel>> AuthorizeUserAsync(
			AuthorizeModel authorizeModel,
			CancellationToken cancellationToken)
		{
			var fullInfo = await _authorizeService.AuthorizeUserAsync(
				authorizeModel, cancellationToken);
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
