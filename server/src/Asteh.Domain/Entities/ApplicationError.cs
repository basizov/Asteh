using System.Net;

namespace Asteh.Domain.Entities
{
	public class ApplicationError
	{
		public string Message { get; set; } = default!;
		public HttpStatusCode StatusCode { get; set; }
	}
}
