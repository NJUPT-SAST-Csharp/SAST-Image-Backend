using Application.AlbumServices;
using Application.CategoryServices;
using Application.ImageServices;
using Application.UserServices;
using Infrastructure.Shared.Database.ModelBuild;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Shared.Database;

internal sealed class QueryDbContext(DbContextOptions<QueryDbContext> options) : DbContext(options)
{
    public required DbSet<AlbumModel> Albums { get; init; }
    public required DbSet<ImageModel> Images { get; init; }
    public required DbSet<UserModel> Users { get; init; }
    public required DbSet<CategoryModel> Categories { get; init; }
    public required DbSet<LikeModel> Likes { get; init; }
    public required DbSet<SubscribeModel> Subscribes { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema("query");

        QueryDbContextEntityTypeConfigurations configuration = new();

        modelBuilder.ApplyConfiguration<AlbumModel>(configuration);
        modelBuilder.ApplyConfiguration<UserModel>(configuration);
        modelBuilder.ApplyConfiguration<CategoryModel>(configuration);
        modelBuilder.ApplyConfiguration<ImageModel>(configuration);
        modelBuilder.ApplyConfiguration<LikeModel>(configuration);
        modelBuilder.ApplyConfiguration<SubscribeModel>(configuration);
    }
}
