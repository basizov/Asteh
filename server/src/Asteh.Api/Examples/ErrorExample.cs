﻿using Asteh.Domain.Entities;
using Swashbuckle.AspNetCore.Filters;
using System.Net;

namespace Asteh.Api.Examples
{
	public class ErrorExample : IExamplesProvider<ApplicationError>
	{
		public ApplicationError GetExamples() =>
			new("Error message from Exception.Message", HttpStatusCode.InternalServerError);
	}
}
