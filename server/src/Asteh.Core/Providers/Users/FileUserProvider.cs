using Asteh.Core.Converters;
using Asteh.Core.Helpers;
using Asteh.Core.Models;
using Asteh.Core.Models.RequestModels;
using Asteh.Domain.Configuration;
using Asteh.Domain.Entities;
using Asteh.Domain.Policies;
using Asteh.Domain.Providers.Users;
using AutoMapper;
using System.Text.Json;

namespace Asteh.Core.Providers.Users
{
	public class FileUserProvider : IUserProvider<FileUserProvider>
	{
		private readonly IMapper _mapper;
		private readonly JsonSerializerOptions _serializerOptions;
		private readonly string _usersRoute;
		private readonly string _userTypesRoute;

		public FileUserProvider(IMapper mapper, IDataSettings dataSettings)
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

		public async Task<IEnumerable<UserModel>> GetUsersAsync(
			CancellationToken cancellationToken = default)
		{
			var users = await DeserializeUsersAsync(cancellationToken);
			foreach (var user in users)
			{
				user.Type = await GetUserTypeByIdAsync(user.TypeId, cancellationToken);
			}
			return _mapper.Map<IEnumerable<UserModel>>(users);
		}

		public async Task<UserModel> GetUserByIdAsync(
			int id,
			CancellationToken cancellationToken = default)
		{
			var users = await DeserializeUsersAsync(cancellationToken);
			var user = users.SingleOrDefault(u => u.Id == id);
			if (user is null)
			{
				throw new ArgumentException($"Couldn't find user with id {id}");
			}
			user.Type = await GetUserTypeByIdAsync(user.TypeId, cancellationToken);
			return _mapper.Map<UserModel>(user);
		}

		public async Task<IEnumerable<UserModel>> FindUsersAsync(
			FilterUserModel filter,
			CancellationToken cancellationToken = default)
		{
			if (!filter.CheckFilterUserModelForNullOrEmpty())
			{
				return await GetUsersAsync(cancellationToken);
			}

			var (result, beginDate, endDate) = filter.CheckFilterUserModelForCorrectDateTime();
			if (!result)
			{
				throw new ArgumentException(
					$"Uncorrect range date: {filter.BeginDate} - {filter.EndDate}");
			}

			var users = await DeserializeUsersAsync(cancellationToken);
			foreach (var user in users)
			{
				user.Type = await GetUserTypeByIdAsync(user.TypeId, cancellationToken);
			}

			var filterName = filter.Name;
			var filterType = filter.TypeName;
			var filterUsers = users.Where(d =>
				(filterName == null || d.Name.Equals(filterName)) &&
				(filterType == null || d.Type!.Name.Equals(filterType)) &&
				(beginDate == null || d.LastVisitDate >= beginDate) &&
				(endDate == null || d.LastVisitDate <= endDate));
			return _mapper.Map<IEnumerable<UserModel>>(filterUsers);
		}

		public async Task<int> CreateUserAsync(
			UserCreateModel model,
			CancellationToken cancellationToken = default)
		{
			var users = await DeserializeUsersAsync(cancellationToken);
			if (users.Any(d => d.Login.Equals(model.Login)))
			{
				throw new ArgumentException($"User with login: {model.Login} is exists");
			}

			var userType = await GetUserTypeByNameAsync(model.TypeName, cancellationToken);
			var lastUser = users.LastOrDefault();
			var id = lastUser is null ? 0 : lastUser.Id + 1;
			users.Add(new()
			{
				Id = id,
				Name = model.Name,
				Login = model.Login,
				Password = model.Password,
				LastVisitDate = DateTime.UtcNow,
				TypeId = userType.Id
			});

			await SerializeUsersAsync(users, cancellationToken);
			return id;
		}
		public async Task UpdateUserAsync(
			int id,
			UserUpdateModel model,
			CancellationToken cancellationToken = default)
		{
			var users = await DeserializeUsersAsync(cancellationToken);
			var user = users.SingleOrDefault(u => u.Id == id);
			if (user is null)
			{
				throw new ArgumentException($"Couldn't find user with id {id}");
			}
			var userIndex = users.IndexOf(user);

			users[userIndex].Name = model.Name;
			users[userIndex].Password = model.Password;

			var userType = await GetUserTypeByNameAsync(model.TypeName, cancellationToken);
			users[userIndex].TypeId = userType.Id;

			var isValidNewDate = DateTime.TryParse(model.LastVisitDate, out var newDate);
			if (!isValidNewDate)
			{
				throw new ArgumentException($"Invalid new lastVisitDate {model.LastVisitDate}");
			}
			users[userIndex].LastVisitDate = newDate;
			await SerializeUsersAsync(users, cancellationToken);
		}

		public async Task DeleteUserAsync(
			int id,
			CancellationToken cancellationToken = default)
		{
			var users = await DeserializeUsersAsync(cancellationToken);
			var user = users.SingleOrDefault(u => u.Id == id);
			if (user is null)
			{
				throw new ArgumentException($"Couldn't find user with id {id}");
			}
			users.Remove(user);
			await SerializeUsersAsync(users, cancellationToken);
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

		private async Task<UserTypeEntity> GetUserTypeByIdAsync(
			int id,
			CancellationToken cancellationToken)
		{
			using var openUserTypesStream = File.OpenRead(_userTypesRoute);
			var userTypes = await JsonSerializer
				.DeserializeAsync<ICollection<UserTypeEntity>>(
					openUserTypesStream, _serializerOptions, cancellationToken);
			if (userTypes is null)
			{
				throw new ArgumentException("Some problems with deserialization UserTypes");
			}

			var userType = userTypes.SingleOrDefault(d => d.Id == id);
			if (userType == null)
			{
				throw new ArgumentException($"Couldn't find userType with id: {id}");
			}
			return userType;
		}

		private async Task<UserTypeEntity> GetUserTypeByNameAsync(
			string name,
			CancellationToken cancellationToken)
		{
			using var openUserTypesStream = File.OpenRead(_userTypesRoute);
			var userTypes = await JsonSerializer
				.DeserializeAsync<ICollection<UserTypeEntity>>(
					openUserTypesStream, _serializerOptions, cancellationToken);
			if (userTypes is null)
			{
				throw new ArgumentException("Some problems with deserialization UserTypes");
			}

			var userType = userTypes.SingleOrDefault(d => d.Name.Equals(name));
			if (userType == null)
			{
				throw new ArgumentException($"Couldn't find userType with name: {name}");
			}
			return userType;
		}

		private async Task SerializeUsersAsync(
			IEnumerable<UserEntity> users,
			CancellationToken cancellationToken)
		{
			File.WriteAllText(_usersRoute, "");
			using var writeUserTypesStream = File.OpenWrite(_usersRoute);
			await JsonSerializer.SerializeAsync(
				writeUserTypesStream, users, _serializerOptions ,cancellationToken);
		}
	}
}
