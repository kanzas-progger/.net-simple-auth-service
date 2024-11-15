using SimpleAuthAndAuthorization.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimpleAuthAndAuthorization.Core.Models;

namespace SimpleAuthAndAuthorization.Infrastructure.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Login).IsRequired().HasMaxLength(User.MAXIMAL_LOGIN_LENGTH);
        builder.Property(u => u.PasswordHash).IsRequired();
        builder.Property(u => u.Firstname).IsRequired();
        builder.Property(u => u.Surname).IsRequired();
        builder.Property(u => u.Fathername).IsRequired(false);
        builder.Property(u => u.Age).IsRequired();
        builder.Property(u => u.Email).IsRequired(false);
        builder.Property(u => u.Phone).IsRequired(false);

        builder.HasMany(u => u.Roles)
            .WithMany(r => r.Users)
            .UsingEntity<UserRoleEntity>(
                l => l.HasOne<RoleEntity>().WithMany().HasForeignKey(r => r.RoleId),
                r => r.HasOne<UserEntity>().WithMany().HasForeignKey(u => u.UserId));
    }
}