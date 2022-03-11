using Asteh.Domain.Configuration;
using Asteh.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Asteh.Domain.DataProvider
{
	public class ApplicationDbContext : DbContext, IDataProvider
	{
		public ApplicationDbContext(DbContextOptions options) : base(options)
		{
		}

		public DbSet<UserEntity> Users => Set<UserEntity>();
		public DbSet<UserTypeEntity> UserTypes => Set<UserTypeEntity>();

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfiguration(new UserEntityConfiguration());
			modelBuilder.ApplyConfiguration(new UserTypeEntityConfiguration());
			base.OnModelCreating(modelBuilder);
		}
	}
}
