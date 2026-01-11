using Domain.AlbumAggregate.AlbumEntity;
using Domain.AlbumAggregate.ImageEntity;
using Domain.CategoryAggregate.CategoryEntity;
using Domain.UserAggregate.UserEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Shared.Database.ModelBuild;

internal class DomainDbContextEntityTypeConfigurations
    : IEntityTypeConfiguration<Album>,
        IEntityTypeConfiguration<User>,
        IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Album> builder)
    {
        builder.ToTable("albums");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasConversion(x => x.Value, x => new(x));

        builder.Ignore(album => album.DomainEvents);

        builder
            .Property<AlbumTitle>("_title")
            .HasColumnName("title")
            .HasConversion(t => t.Value, v => new(v));
        builder
            .Property<AccessLevel>("_accessLevel")
            .HasColumnName("access_level")
            .HasConversion(a => a.Value, v => new(v));

        builder.HasIndex("_title").IsUnique(true);

        builder.ComplexProperty<AlbumStatus>(
            "_status",
            status =>
            {
                status.Property(s => s.Value).HasColumnName("status");
                status.Property(s => s.RemovedAt).HasColumnName("removed_at");
            }
        );

        builder.HasOne<User>().WithMany().HasForeignKey("_author");
        builder
            .Property<UserId>("_author")
            .HasColumnName("author_id")
            .HasConversion(u => u.Value, v => new(v));

        builder
            .PrimitiveCollection<UserId[]>("_collaborators")
            .HasColumnName("collaborators")
            .ElementType(id =>
                id.HasConversion(
                    new ValueConverter<UserId, long>(id => id.Value, value => new(value))
                )
            );

        builder.OwnsOne<Cover>(
            "_cover",
            cover =>
            {
                cover
                    .Property(c => c.Id)
                    .HasColumnName("cover_id")
                    .HasConversion<long?>(
                        id => id.HasValue ? id.Value.Value : null,
                        id => id.HasValue ? new(id.Value) : null
                    );
                cover.Property(c => c.IsLatestImage).HasColumnName("cover_is_latest_image");
            }
        );

        builder.OwnsMany<Subscribe>(
            "_subscribes",
            entity =>
            {
                entity.HasOne<User>().WithMany().HasForeignKey(e => e.User);
                entity.HasOne<Album>().WithMany().HasForeignKey(e => e.Album);

                entity.Property(e => e.User).HasConversion(u => u.Value, v => new(v));
                entity.Property(e => e.Album).HasConversion(a => a.Value, v => new(v));

                entity.HasKey(x => new { x.User, x.Album });
                entity.WithOwner().HasForeignKey(e => e.Album);

                entity.ToTable("subscribes");
            }
        );

        builder.OwnsMany<Image>(
            "_images",
            image =>
            {
                image.HasKey(x => x.Id);
                image
                    .Property(x => x.Id)
                    .HasColumnName("id")
                    .HasConversion(x => x.Value, x => new(x));

                image.HasOne<User>().WithMany().HasForeignKey("_uploader");

                image
                    .Property<UserId>("_uploader")
                    .HasColumnName("uploader_id")
                    .HasConversion(u => u.Value, v => new(v));

                image.OwnsOne<ImageStatus>(
                    "_status",
                    image =>
                    {
                        image.Property(s => s.Value).HasColumnName("status");
                        image.Property(s => s.RemovedAt).HasColumnName("removed_at");
                    }
                );

                image.ToTable("images");
                image.WithOwner().HasForeignKey("album_id");

                image.OwnsMany<Like>(
                    "_likes",
                    entity =>
                    {
                        entity.HasKey(x => new { x.User, x.Image });

                        entity.HasOne<User>().WithMany().HasForeignKey(like => like.User);
                        entity.HasOne<Image>().WithMany().HasForeignKey(like => like.Image);

                        entity.Property(e => e.User).HasConversion(u => u.Value, v => new(v));
                        entity.Property(e => e.Image).HasConversion(i => i.Value, v => new(v));

                        entity.WithOwner().HasForeignKey(like => like.Image);
                        entity.ToTable("likes");
                    }
                );
            }
        );
    }

    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");
        builder.HasKey(x => x.Id);
        builder.Property<UserId>("Id").HasColumnName("id").HasConversion(t => t.Value, v => new(v));

        builder.Ignore(x => x.DomainEvents);

        builder
            .Property<Username>("_username")
            .HasColumnName("username")
            .HasConversion(t => t.Value, v => new(v));

        builder
            .Property<RefreshToken>("_refreshToken")
            .HasColumnName("refresh_token")
            .HasConversion(t => t.Value, v => new(v));

        builder.HasIndex("_username").IsUnique(true);

        builder.PrimitiveCollection<Role[]>("_roles").HasColumnName("roles");
        builder.OwnsOne<Password>(
            "_password",
            password =>
            {
                password.Property(p => p.Hash).HasColumnName("password_hash").IsRequired();
                password.Property(p => p.Salt).HasColumnName("password_salt").IsRequired();
            }
        );
    }

    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("categories");
        builder.HasKey(x => x.Id);
        builder
            .Property<CategoryId>("Id")
            .HasColumnName("id")
            .HasConversion(t => t.Value, v => new(v));

        builder.Ignore(x => x.DomainEvents);

        builder
            .Property<CategoryName>("_name")
            .HasColumnName("name")
            .HasConversion(t => t.Value, v => new(v));
    }
}
