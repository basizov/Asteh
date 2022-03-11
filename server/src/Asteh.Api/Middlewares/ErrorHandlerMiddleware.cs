using System.Net;

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
				// TODO: Add error entity
				context.Response.ContentType = "application/json";
				context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
				await context.Response.WriteAsync(ex.Message);
			}
		}
	}
}
