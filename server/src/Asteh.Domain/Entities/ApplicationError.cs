using System.Net;

namespace Asteh.Domain.Entities
{
	public class ApplicationError
	{
		public ApplicationError(string message, HttpStatusCode statusCode)
		{
			Message = message;
			StatusCode = statusCode;
		}

		public string Message { get; set; } = default!;
		public HttpStatusCode StatusCode { get; set; }
	}
}
