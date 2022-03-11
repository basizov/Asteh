using Asteh.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Asteh.Domain.DataProvider
{
	public interface IDataProvider
	{
		DbSet<UserEntity> Users { get; }
		DbSet<UserTypeEntity> UserTypes { get; }
	}
}
