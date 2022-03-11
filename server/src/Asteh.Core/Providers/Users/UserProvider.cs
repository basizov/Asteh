using Asteh.Core.Models;
using Asteh.Core.Models.RequestModels;
using Asteh.Domain.Entities;
using Asteh.Domain.Repositories.Base;
using AutoMapper;

namespace Asteh.Domain.Providers.Users
{
	public class UserProvider : IUserProvider
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
				.GetAllWithLazyLoadingAsync(cancellationToken);
			var resultUsers = _mapper.Map<IEnumerable<UserModel>>(users);
			return resultUsers;
		}

		public async Task<IEnumerable<UserModel>> FindUsersAsync(
			FilterUserModel filter,
			CancellationToken cancellationToken = default)
		{
			var isBeginDateNullOrWmpty = string.IsNullOrEmpty(filter?.BeginDate ?? null);
			var isEndDateNullOrWmpty = string.IsNullOrEmpty(filter?.EndDate ?? null);
			if (filter == null || (
				string.IsNullOrEmpty(filter.Name) &&
				string.IsNullOrEmpty(filter.TypeName) &&
				isBeginDateNullOrWmpty &&
				isEndDateNullOrWmpty))
			{
				return await GetUsersAsync(cancellationToken);
			}

			var isCorrectBeginDate = DateTime
				.TryParse(filter.BeginDate, out var beginDate) || isBeginDateNullOrWmpty;
			var isCorrectEndDate = DateTime
				.TryParse(filter.EndDate, out var endDate) || isEndDateNullOrWmpty;
			if (!(isCorrectBeginDate && isCorrectEndDate &&
				(beginDate < endDate || isBeginDateNullOrWmpty || isEndDateNullOrWmpty)))
			{
				throw new ArgumentException(
					$"Uncorrect range date: {filter.BeginDate} - {filter.EndDate}");
			}

			var filterName = filter.Name;
			var filterType = filter.TypeName;
			DateTime? filterBeginDate = isCorrectBeginDate ? null : beginDate;
			DateTime? filterEndDate = isEndDateNullOrWmpty ? null : endDate;
			var filterUsers = await _unitOfWork.UserRepository.FindByWithLazyLoadingAsync(d =>
				(filterName == null || d.Name.Equals(filterName)) &&
				(filterType == null || d.Type!.Name.Equals(filterType)) &&
				(filterBeginDate == null || d.LastVisitDate >= filterBeginDate) &&
				(filterEndDate == null || d.LastVisitDate <= filterEndDate), cancellationToken);
			var resultFilteredUsers = _mapper.Map<IEnumerable<UserModel>>(filterUsers);
			return resultFilteredUsers;
		}

		public async Task CreateUserAsync(
			UserCreateModel model,
			CancellationToken cancellationToken = default)
		{
			if (await _unitOfWork.UserRepository.AnyAsync(
				d => d.Login.Equals(model.Login), cancellationToken))
			{
				throw new ArgumentException($"User with email: {model.Login} is exists");
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
			_unitOfWork.UserRepository.Create(userEntity);
			await _unitOfWork.SaveChangesAsync(cancellationToken);
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
