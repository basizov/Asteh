using Asteh.Domain.Configuration;

namespace Asteh.Api.Configuration
{
	public class DataSettings : IDataSettings
	{
		public DataSettings(
			string dbConnectionString,
			string fileSerializerString)
		{
			DbConnectionString = dbConnectionString;
			FileSerializerString = fileSerializerString;
		}

		public string DbConnectionString { get; init; } = default!;
		public string FileSerializerString { get; init; } = default!;
	}
}
