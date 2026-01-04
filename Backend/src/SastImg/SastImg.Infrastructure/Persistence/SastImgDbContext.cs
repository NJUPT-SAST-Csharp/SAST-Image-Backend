using Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Primitives.Entity;
using Primitives.ValueObject;
using SastImg.Domain.AlbumAggregate.AlbumEntity;
using SastImg.Domain.AlbumAggregate.ImageEntity;
using SastImg.Domain.AlbumTagEntity;
using SastImg.Domain.CategoryEntity;
using SastImg.Infrastructure.Domain.AlbumEntity;
using SastImg.Infrastructure.Persistence.EntityTypeConfigurations;

namespace SastImg.Infrastructure.Persistence;

public sealed class SastImgDbContext(DbContextOptions<SastImgDbContext> options)
    : DbContext(options)
{
    public DbSet<Album> Albums { get; set; }

    public DbSet<Category> Categories { get; set; }

    public DbSet<ImageTag> Tags { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new AlbumEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new CategoryEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new TagEntityTypeConfiguration());

        base.OnModelCreating(modelBuilder);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder builder)
    {
        builder.Properties<CategoryId>().HaveConversion<OpenValueConverter<CategoryId>>();
        builder.Properties<AlbumId>().HaveConversion<OpenValueConverter<AlbumId>>();
        builder.Properties<ImageId>().HaveConversion<OpenValueConverter<ImageId>>();
        builder.Properties<UserId>().HaveConversion<OpenValueConverter<UserId>>();
        builder.DefaultTypeMapping<UserId>().HasConversion<OpenValueConverter<UserId>>();
        builder.Properties<ImageTagId>().HaveConversion<OpenValueConverter<ImageTagId>>();
        builder.DefaultTypeMapping<ImageTagId>().HasConversion<OpenValueConverter<ImageTagId>>();

        builder.Properties<TagName>().HaveConversion<OpenValueConverter<TagName, string>>();
        builder.Properties<ImageTitle>().HaveConversion<OpenValueConverter<ImageTitle, string>>();
        builder
            .Properties<ImageDescription>()
            .HaveConversion<OpenValueConverter<ImageDescription, string>>();
        builder.Properties<AlbumTitle>().HaveConversion<OpenValueConverter<AlbumTitle, string>>();
        builder
            .Properties<CategoryName>()
            .HaveConversion<OpenValueConverter<CategoryName, string>>();
        builder
            .Properties<CategoryDescription>()
            .HaveConversion<OpenValueConverter<CategoryDescription, string>>();
        builder
            .Properties<AlbumDescription>()
            .HaveConversion<OpenValueConverter<AlbumDescription, string>>();
    }
}

internal sealed class OpenValueConverter<TObject, TValue>()
    : ValueConverter<TObject, TValue>(o => o.Value, v => new TObject { Value = v })
    where TObject : IValueObject<TObject, TValue>, new() { }

internal sealed class OpenValueConverter<TId>()
    : ValueConverter<TId, long>(o => o.Value, v => new TId { Value = v })
    where TId : ITypedId<TId>, new() { }
