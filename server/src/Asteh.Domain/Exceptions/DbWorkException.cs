namespace Asteh.Domain.Exceptions
{
	public class DbWorkException : Exception
	{
		public DbWorkException(string? message) : base(message)
		{
		}

		public DbWorkException(string? message, Exception? ex) : base(message, ex)
		{
		}
	}
}
