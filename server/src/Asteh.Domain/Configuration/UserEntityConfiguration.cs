using Asteh.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Globalization;

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
					d => d.ToString("dd.MM.yyyy"),
					d => DateTime.ParseExact(d, "dd.MM.yyyy", CultureInfo.CurrentCulture))
				.HasMaxLength(10);
			builder.HasIndex(u => u.Login)
				.IsUnique()
				.HasFilter(@"""Login"" IS NOT NULL")
				.HasDatabaseName("UserLoginIndex");
		}
	}
}
