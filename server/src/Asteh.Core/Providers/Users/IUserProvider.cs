using Asteh.Core.Models;
using Asteh.Core.Models.RequestModels;

namespace Asteh.Core.Providers.Users
{
	public interface IUserProvider<T>
		where T : class
	{
		Task<IEnumerable<UserModel>> GetUsersAsync(
			CancellationToken cancellationToken = default);
		Task<UserModel> GetUserByIdAsync(
			int id,
			CancellationToken cancellationToken = default);
		Task<IEnumerable<UserModel>> FindUsersAsync(
			FilterUserModel filter,
			CancellationToken cancellationToken = default);
		Task<int> CreateUserAsync(
			UserCreateModel model,
			CancellationToken cancellationToken = default);
		Task UpdateUserAsync(
			int id,
			UserUpdateModel model,
			CancellationToken cancellationToken = default);
		Task DeleteUserAsync(
			int id,
			CancellationToken cancellationToken = default);
	}
}
