using Asteh.Domain.Repositories.Users;
using Asteh.Domain.Repositories.UserTypes;

namespace Asteh.Domain.Repositories.Base
{
	public interface IUnitOfWork
	{
		IUserRepository UserRepository { get; }
		IUserTypeRepository UserTypeRepository { get; }

		Task SaveChangesAsync(CancellationToken cancellationToken = default);
	}
}
