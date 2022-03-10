using Asteh.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Asteh.Domain.Configuration
{
	internal class UserEntityConfiguration : IEntityTypeConfiguration<UserEntity>
	{
		public void Configure(EntityTypeBuilder<UserEntity> builder)
		{
			builder.Property(u => u.Login)
				.IsRequired();
			builder.Property(u => u.Password)
				.IsRequired();
			builder.Property(u => u.Name)
				.IsRequired();
			builder.Property(u => u.LastVisitDate)
				.IsRequired()
				.HasConversion(
					lvd => lvd.ToString("dd.MM.yyyy"),
					lvdstr => DateTime.Parse(lvdstr))
				.HasMaxLength(10);
			builder.HasIndex(u => u.Login)
				.IsUnique()
				.HasFilter(@"""Login"" IS NOT NULL")
				.HasDatabaseName("UserLoginIndex");
		}
	}
}
