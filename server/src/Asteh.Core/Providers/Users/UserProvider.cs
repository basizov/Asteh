using Asteh.Core.Helpers;
using Asteh.Core.Models;
using Asteh.Core.Models.RequestModels;
using Asteh.Domain.Entities;
using Asteh.Domain.Repositories.Base;
using AutoMapper;

namespace Asteh.Domain.Providers.Users
{
	public class UserProvider : IUserProvider<UserProvider>
	{
		private readonly IMapper _mapper;
		private readonly IUnitOfWork _unitOfWork;

		public UserProvider(IMapper mapper, IUnitOfWork unitOfWork)
		{
			_mapper = mapper;
			_unitOfWork = unitOfWork;
		}

		public async Task<IEnumerable<UserModel>> GetUsersAsync(
			CancellationToken cancellationToken = default)
		{
			var users = await _unitOfWork.UserRepository
				.GetAllAsync(cancellationToken);
			return _mapper.Map<IEnumerable<UserModel>>(users);
		}

		public async Task<UserModel> GetUserByIdAsync(
			int id,
			CancellationToken cancellationToken = default)
		{
			var user = await _unitOfWork.UserRepository
				.SingleOrDefaultAsync(u => u.Id == id, cancellationToken);

			if (user == null)
			{
				throw new ArgumentException($"Invalid user identifier {id}");
			}
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

			var filterName = filter.Name;
			var filterType = filter.TypeName;
			var filterUsers = await _unitOfWork.UserRepository.FindByAsync(d =>
				(filterName == null || d.Name.Equals(filterName)) &&
				(filterType == null || d.Type!.Name.Equals(filterType)) &&
				(beginDate == null || d.LastVisitDate >= beginDate) &&
				(endDate == null || d.LastVisitDate <= endDate), cancellationToken);
			return _mapper.Map<IEnumerable<UserModel>>(filterUsers);
		}

		public async Task<int> CreateUserAsync(
			UserCreateModel model,
			CancellationToken cancellationToken = default)
		{
			if (await _unitOfWork.UserRepository.AnyAsync(
				d => d.Login.Equals(model.Login), cancellationToken))
			{
				throw new ArgumentException($"User with login: {model.Login} is exists");
			}

			var userType = await FindSingleUserTypeAsync(model.TypeName, cancellationToken);
			var userEntity = new UserEntity
			{
				Login = model.Login,
				Name = model.Name,
				Password = model.Password,
				TypeId = userType.Id,
				LastVisitDate = DateTime.UtcNow
			};
			var	newUserEntity = _unitOfWork.UserRepository.Create(userEntity);
			await _unitOfWork.SaveChangesAsync(cancellationToken);
			return newUserEntity.Id;
		}

		public async Task UpdateUserAsync(
			int id,
			UserUpdateModel model,
			CancellationToken cancellationToken = default)
		{
			var user = await FindSingleUserAsync(id, cancellationToken);
			user.Name = model.Name;
			user.Password = model.Password;

			var userType = await FindSingleUserTypeAsync(model.TypeName, cancellationToken);
			user.TypeId = userType.Id;

			var isValidNewDate = DateTime.TryParse(model.LastVisitDate, out var newDate);
			if (!isValidNewDate)
			{
				throw new ArgumentException($"Invalid new lastVisitDate {model.LastVisitDate}");
			}
			user.LastVisitDate = newDate;

			_unitOfWork.UserRepository.Update(user);
			await _unitOfWork.SaveChangesAsync(cancellationToken);
		}

		private async Task<UserTypeEntity> FindSingleUserTypeAsync(
			string typeName,
			CancellationToken cancellationToken)
		{
			var userType = await _unitOfWork.UserTypeRepository
				.SingleOrDefaultAsync(d => d.Name.Equals(typeName), cancellationToken);
			if (userType is null)
			{
				throw new ArgumentException($"UserType with name: {typeName} doesn't exists");
			}
			return userType;
		}

		public async Task DeleteUserAsync(
			int id,
			CancellationToken cancellationToken = default)
		{
			var user = await FindSingleUserAsync(id, cancellationToken);
			_unitOfWork.UserRepository.Delete(user);
			await _unitOfWork.SaveChangesAsync(cancellationToken);
		}

		private async Task<UserEntity> FindSingleUserAsync(
			int id,
			CancellationToken cancellationToken)
		{
			var user = await _unitOfWork.UserRepository
				.SingleOrDefaultAsync(d => d.Id == id, cancellationToken);
			if (user is null)
			{
				throw new ArgumentException($"User with id: {id} doesn't exists");
			}
			return user;
		}
	}
}
