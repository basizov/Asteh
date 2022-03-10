using Asteh.Core.Models;
using Asteh.Core.Models.RequestModels;
using Asteh.Domain.Repositories.Base;

namespace Asteh.Core.Services.Users
{
	public class UserService : IUserService
	{
		private readonly IUnitOfWork _unitOfWork;

		public UserService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public Task<IEnumerable<UserModel>> GetUsersAsync(CancellationToken cancellationToken = default) => throw new NotImplementedException();

		public Task<IEnumerable<UserModel>> FindUsersAsync(
			FilterUserModel filter,
			CancellationToken cancellationToken = default) => throw new NotImplementedException();

		public Task CreateUserAsync(
			UserCreateModel model,
			CancellationToken cancellationToken = default) => throw new NotImplementedException();

		public Task UpdateUserAsync(
			int id,
			UserUpdateModel model,
			CancellationToken cancellationToken = default) => throw new NotImplementedException();

		public Task DeleteUserAsync(
			int id,
			CancellationToken cancellationToken = default) => throw new NotImplementedException();
	}
}
