namespace Asteh.Domain.Configuration
{
	public interface IDataSettings
	{
		string DbConnectionString { get; init; }
		string FileSerializerString { get; init; }
	}
}
