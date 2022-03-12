using Asteh.Core.Models;
using Asteh.Domain.Configuration;
using Asteh.Domain.Entities;
using Asteh.Domain.Policies;
using AutoMapper;
using System.Text.Json;

namespace Asteh.Core.Providers.UserTypes
{
	public class FileUserTypeProvider : IUserTypeProvider<FileUserTypeProvider>
	{
		private readonly IMapper _mapper;
		private readonly JsonSerializerOptions _serializerOptions;
		private readonly string _userTypesRoute;

		public FileUserTypeProvider(
			IMapper mapper,
			IDataSettings dataSettings)
		{
			var serializerOptions = new JsonSerializerOptions
			{
				PropertyNamingPolicy = SnakeCaseNamingPolicy.SnakeCase
			};

			_serializerOptions = serializerOptions;
			_userTypesRoute = Path.Combine(dataSettings.FileSerializerString, "UserTypes.json");
			_mapper = mapper;
		}

		public async Task<IEnumerable<UserTypeModel>> GetUserTypesAsync(
			CancellationToken cancellationToken = default)
		{
			using var openUserTypesStream = File.OpenRead(_userTypesRoute);
			var userTypes = await JsonSerializer
				.DeserializeAsync<IEnumerable<UserTypeEntity>>(
					openUserTypesStream, _serializerOptions, cancellationToken);
			if (userTypes is null)
			{
				throw new ArgumentException("Some problems with deserialization UserTypes");
			}
			return _mapper.Map<IEnumerable<UserTypeModel>>(userTypes);
		}
	}
}
