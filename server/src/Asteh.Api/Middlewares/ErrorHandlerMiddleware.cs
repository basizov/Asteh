using Asteh.Domain.Entities;
using System.Net;
using System.Text.Json;

namespace Asteh.Api.Middlewares
{
	public class ErrorHandlerMiddleware
	{
		private readonly RequestDelegate _next;

		public ErrorHandlerMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			try
			{
				await _next(context);
			}
			catch (Exception ex)
			{
				context.Response.ContentType = "application/json";

				var statusCode = HttpStatusCode.InternalServerError;
				context.Response.StatusCode = (int)statusCode;

				var response = new ApplicationError(ex.Message, statusCode);
				await context.Response.WriteAsync(
					JsonSerializer.Serialize(response));
			}
		}
	}
}
