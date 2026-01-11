using Application.AlbumServices;
using Application.CategoryServices;
using Application.ImageServices;
using Application.UserServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Shared.Database.ModelBuild;

internal sealed class QueryDbContextEntityTypeConfigurations
    : IEntityTypeConfiguration<AlbumModel>,
        IEntityTypeConfiguration<ImageModel>,
        IEntityTypeConfiguration<UserModel>,
        IEntityTypeConfiguration<CategoryModel>,
        IEntityTypeConfiguration<SubscribeModel>,
        IEntityTypeConfiguration<LikeModel>
{
    public void Configure(EntityTypeBuilder<AlbumModel> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(album => album.CreatedAt);
        builder.PrimitiveCollection(album => album.Collaborators);
        builder.PrimitiveCollection(album => album.Tags);
        builder.HasOne<CategoryModel>().WithMany().HasForeignKey(album => album.CategoryId);
        builder.HasOne<UserModel>().WithMany().HasForeignKey(album => album.AuthorId);
        builder.HasMany(album => album.Images).WithOne().HasForeignKey(image => image.AlbumId);
        builder
            .HasMany<UserModel>()
            .WithMany()
            .UsingEntity<SubscribeModel>(
                l => l.HasOne<UserModel>().WithMany().HasForeignKey(s => s.User),
                r => r.HasOne<AlbumModel>().WithMany(a => a.Subscribes).HasForeignKey(s => s.Album),
                s => s.ToTable("subscribes")
            );

        builder.HasIndex(album => album.Title).IsUnique(true);
    }

    public void Configure(EntityTypeBuilder<ImageModel> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(image => image.Title);
        builder.Property(image => image.UploadedAt);
        builder.Property(image => image.AuthorId);
        builder.PrimitiveCollection(image => image.Tags);
        builder.PrimitiveCollection(image => image.Collaborators);
        builder.HasOne<UserModel>().WithMany().HasForeignKey(image => image.AuthorId);
        builder.HasOne<UserModel>().WithMany().HasForeignKey(image => image.UploaderId);
        builder
            .HasMany<UserModel>()
            .WithMany()
            .UsingEntity<LikeModel>(
                l => l.HasOne<UserModel>().WithMany().HasForeignKey(l => l.User),
                r => r.HasOne<ImageModel>().WithMany(i => i.Likes).HasForeignKey(l => l.Image),
                like => like.ToTable("likes")
            );
    }

    public void Configure(EntityTypeBuilder<UserModel> builder)
    {
        builder.HasKey(a => a.Id);

        builder.HasIndex(user => user.Username).IsUnique(true);

        builder.Property(u => u.RegisteredAt);
    }

    public void Configure(EntityTypeBuilder<CategoryModel> builder)
    {
        builder.HasKey(a => a.Id);

        builder.HasIndex(c => c.Name).IsUnique(true);
    }

    public void Configure(EntityTypeBuilder<SubscribeModel> builder)
    {
        builder.HasKey(s => new { s.Album, s.User });
    }

    public void Configure(EntityTypeBuilder<LikeModel> builder)
    {
        builder.HasKey(l => new { l.Image, l.User });
    }
}
