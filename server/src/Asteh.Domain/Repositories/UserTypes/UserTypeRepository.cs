using Asteh.Domain.DataProvider;
using Asteh.Domain.Entities;
using Asteh.Domain.Repositories.Base;

namespace Asteh.Domain.Repositories.UserTypes
{
	internal class UserTypeRepository : BaseRepository<UserTypeEntity>, IUserTypeRepository
	{
		public UserTypeRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
		{
		}
	}
}
