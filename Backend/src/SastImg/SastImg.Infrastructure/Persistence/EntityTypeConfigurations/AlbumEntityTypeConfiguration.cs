using Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SastImg.Domain.AlbumAggregate.AlbumEntity;
using SastImg.Domain.AlbumAggregate.ImageEntity;
using SastImg.Domain.AlbumTagEntity;
using SastImg.Domain.CategoryEntity;
using SastImg.Infrastructure.Persistence;

namespace SastImg.Infrastructure.Domain.AlbumEntity;

public sealed class AlbumEntityTypeConfiguration : IEntityTypeConfiguration<Album>
{
    public void Configure(EntityTypeBuilder<Album> builder)
    {
        builder.ToTable("albums");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id);

        builder.Ignore(album => album.DomainEvents);

        builder.Property<AlbumTitle>("_title").HasColumnName("title");
        builder.Property<AlbumDescription>("_description").HasColumnName("description");
        builder.Property<Accessibility>("_accessibility").HasColumnName("accessibility");
        builder.Property<bool>("_isRemoved").HasColumnName("is_removed");
        builder.Property<DateTime>("_createdAt").HasColumnName("created_at");
        builder.Property<DateTime>("_updatedAt").HasColumnName("updated_at");

        builder.Property<CategoryId>("_categoryId").HasColumnName("category_id");
        builder.Property<UserId>("_authorId").HasColumnName("author_id");
        builder
            .PrimitiveCollection<UserId[]>("_collaborators")
            .HasColumnName("collaborators")
            .ElementType(e => e.HasConversion<OpenValueConverter<UserId>>());

        builder.HasIndex("_title").IsUnique(true);

        builder.HasOne<Category>().WithMany().HasForeignKey("_categoryId");

        builder.OwnsOne<Cover>(
            "_cover",
            cover =>
            {
                cover
                    .Property(c => c.ImageId)
                    .HasColumnName("cover_id")
                    .HasConversion<long?>(
                        id => id.HasValue ? id.Value.Value : null,
                        id => id.HasValue ? new() : null
                    );
                cover.Property(c => c.IsLatestImage).HasColumnName("cover_is_latest_image");
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
                    .HasConversion(x => x.Value, x => new());

                image.ToTable("images");
                image.WithOwner().HasForeignKey("album_id");

                image.Ignore("UploadedTime");
                image.Ignore("Url");

                image.Property<ImageTitle>("_title").HasColumnName("title");
                image.Property<ImageDescription>("_description").HasColumnName("description");

                image.Property<DateTime>("_uploadtedAt").HasColumnName("uploaded_at");
                image.Property<bool>("_isRemoved").HasColumnName("is_removed");

                image
                    .PrimitiveCollection<ImageTagId[]>("_tags")
                    .HasColumnName("tags")
                    .ElementType(e => e.HasConversion<OpenValueConverter<ImageTagId>>());

                image.OwnsOne<ImageUrl>(
                    "_url",
                    url =>
                    {
                        url.Property(u => u.Original).HasColumnName("url");
                        url.Property(u => u.Thumbnail).HasColumnName("thumbnail_url");
                    }
                );
            }
        );
    }
}
