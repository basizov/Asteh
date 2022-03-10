using Asteh.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Asteh.Domain.Database
{
	public interface IApplicationContext
	{
		DbSet<UserEntity> Users { get; }
		DbSet<UserTypeEntity> UserTypes { get; }
	}
}
