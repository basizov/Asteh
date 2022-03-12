using Asteh.Core.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

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
				context.Result = new ForbidResult();
			}
		}
	}
}
