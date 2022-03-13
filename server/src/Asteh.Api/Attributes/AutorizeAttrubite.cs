using Asteh.Core.Helpers;
using Asteh.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace Asteh.Api.Attributes
{
	public class AutorizeAttrubite : Attribute, IAuthorizationFilter
	{
		public void OnAuthorization(AuthorizationFilterContext context)
		{
			var allowAcces = context.HttpContext
				.Request
				.Cookies[Constants.CookieAllowAccessTab];
			if (!(allowAcces is not null &&
				allowAcces.Equals(Constants.AllowIndicator)))
			{
				var unauthorizedSC = HttpStatusCode.Unauthorized;
				var result = new ObjectResult(new ApplicationError(
					"Don't have neccessary permissions", unauthorizedSC))
				{
					StatusCode = (int)unauthorizedSC
				};
				context.Result = result;
			}
		}
	}
}
