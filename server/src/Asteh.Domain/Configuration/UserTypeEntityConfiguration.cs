using Asteh.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Asteh.Domain.Configuration
{
	internal class UserTypeEntityConfiguration : IEntityTypeConfiguration<UserTypeEntity>
	{
		public void Configure(EntityTypeBuilder<UserTypeEntity> builder)
		{
			builder.Property(ut => ut.Name)
				.IsRequired();
			builder.Property(ut => ut.AllowEdit)
				.IsRequired()
				.HasConversion(
					ae => ae ? "allowed" : "no allowed",
					aestr => aestr.Equals("allowed"));
		}
	}
}
