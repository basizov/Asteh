using Asteh.Core.Converters;
using Asteh.Core.Models;
using Asteh.Core.Models.RequestModels;
using Asteh.Domain.Configuration;
using Asteh.Domain.Entities;
using Asteh.Domain.Policies;
using AutoMapper;
using System.Text.Json;

namespace Asteh.Core.Services.Authorize
{
	public class FileAuthorizeService : IAuthorizeService<FileAuthorizeService>
	{
		private readonly IMapper _mapper;
		private readonly JsonSerializerOptions _serializerOptions;
		private readonly string _usersRoute;
		private readonly string _userTypesRoute;

		public FileAuthorizeService(
			IMapper mapper,
			IDataSettings dataSettings)
		{
			var serializerOptions = new JsonSerializerOptions
			{
				PropertyNamingPolicy = SnakeCaseNamingPolicy.SnakeCase
			};
			serializerOptions.Converters.Add(new DateTimeConverterUsingDateTimeParse());

			_serializerOptions = serializerOptions;
			_usersRoute = Path.Combine(dataSettings.FileSerializerString, "Users.json");
			_userTypesRoute = Path.Combine(dataSettings.FileSerializerString, "UserTypes.json");
			_mapper = mapper;
		}

		public async Task<FullInfoModel?> AuthorizeUserAsync(
			AuthorizeModel authorizeModel,
			CancellationToken cancellationToken = default)
		{
			var users = await DeserializeUsersAsync(cancellationToken);
			var user = users.SingleOrDefault(u =>
				u.Login.Equals(authorizeModel.Login)&&
				u.Password.Equals(authorizeModel.Password));
			if (user is null)
			{
				return null;
			}

			var userTypes = await DeserializeUserTypesAsync(cancellationToken);
			foreach (var userEntity in users)
			{
				userEntity.Type = userTypes
					.SingleOrDefault(d => d.Id == userEntity.TypeId);
			}

			var userModels = _mapper.Map<IEnumerable<UserModel>>(users);
			var userTypeModels = _mapper.Map<IEnumerable<UserTypeModel>>(userTypes);
			return new()
			{
				UserId = user.Id,
				IsAccessEnabled = user.Type?.AllowEdit ?? false,
				Users = userModels,
				UserTypes = userTypeModels
			};
		}

		public async Task<FullInfoModel> GetFullInfoModelToAuthorizeUser(
			CancellationToken cancellationToken = default)
		{
			var users = await DeserializeUsersAsync(cancellationToken);
			var userTypes = await DeserializeUserTypesAsync(cancellationToken);
			foreach (var userEntity in users)
			{
				userEntity.Type = userTypes
					.SingleOrDefault(d => d.Id == userEntity.TypeId);
			}

			var	userModels = _mapper.Map<IEnumerable<UserModel>>(users);
			var	userTypeModels = _mapper.Map<IEnumerable<UserTypeModel>>(userTypes);
			return new()
			{
				UserId = 0,
				IsAccessEnabled = false,
				Users = userModels,
				UserTypes = userTypeModels
			};
		}

		private async Task<IList<UserEntity>> DeserializeUsersAsync(
			CancellationToken cancellationToken)
		{
			using var openUsersStream = File.OpenRead(_usersRoute);
			var users = await JsonSerializer
				.DeserializeAsync<IList<UserEntity>>(
					openUsersStream, _serializerOptions, cancellationToken);
			if (users is null)
			{
				throw new ArgumentException("Some problems with deserialization Users");
			}
			return users;
		}

		private async Task<IEnumerable<UserTypeEntity>> DeserializeUserTypesAsync(
			CancellationToken cancellationToken)
		{
			using var openUserTypesStream = File.OpenRead(_userTypesRoute);
			var userTypes = await JsonSerializer
				.DeserializeAsync<IEnumerable<UserTypeEntity>>(
					openUserTypesStream, _serializerOptions, cancellationToken);
			if (userTypes is null)
			{
				throw new ArgumentException("Some problems with deserialization UserTypes");
			}
			return userTypes;
		}
	}
}
