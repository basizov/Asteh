using Asteh.Domain.Database;
using Asteh.Domain.Entities;
using Asteh.Domain.Repositories.Base;

namespace Asteh.Domain.Repositories.Users
{
	internal class UserRepository : BaseRepository<UserEntity>, IUserRepository
	{
		public UserRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
		{
		}
	}
}
