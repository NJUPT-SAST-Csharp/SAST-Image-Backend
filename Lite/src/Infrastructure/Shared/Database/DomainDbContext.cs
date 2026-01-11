using Domain.AlbumAggregate.AlbumEntity;
using Domain.CategoryAggregate.CategoryEntity;
using Domain.UserAggregate.UserEntity;
using Infrastructure.Shared.Database.ModelBuild;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Shared.Database;

internal sealed class DomainDbContext(DbContextOptions<DomainDbContext> options)
    : DbContext(options)
{
    public DbSet<Album> Albums { get; init; }
    public DbSet<Category> Categories { get; init; }
    public DbSet<User> Users { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema("domain");

        DomainDbContextEntityTypeConfigurations configuration = new();
        modelBuilder.ApplyConfiguration<Album>(configuration);
        modelBuilder.ApplyConfiguration<Category>(configuration);
        modelBuilder.ApplyConfiguration<User>(configuration);
    }
}
