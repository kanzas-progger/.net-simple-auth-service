using SimpleAuthAndAuthorization.Infrastructure.Configurations;
using SimpleAuthAndAuthorization.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

//dotnet ef migrations add InitialCreate --project SimpleAuthAndAuthorization.Infrastructure/
//--startup-project SimpleAuthAndAuthorization.API/

namespace SimpleAuthAndAuthorization.Infrastructure;

public class SimpleAuthAndAuthorizationDbContext : DbContext
{
    public SimpleAuthAndAuthorizationDbContext(
        DbContextOptions<SimpleAuthAndAuthorizationDbContext> options) : base(options) {}
    
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<RoleEntity> Roles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new RoleConfiguration());
    }
    
}