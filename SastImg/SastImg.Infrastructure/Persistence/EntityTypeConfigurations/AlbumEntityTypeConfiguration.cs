using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SastImg.Domain;
using SastImg.Domain.AlbumAggregate.AlbumEntity;
using SastImg.Domain.AlbumAggregate.ImageEntity;
using SastImg.Domain.CategoryEntity;
using SastImg.Domain.TagEntity;

namespace SastImg.Infrastructure.Domain.AlbumEntity
{
    public sealed class AlbumEntityTypeConfiguration : IEntityTypeConfiguration<Album>
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
                .Property<AlbumDescription>("_description")
                .HasColumnName("description")
                .HasConversion(t => t.Value, v => new(v));
            builder.Property<Accessibility>("_accessibility").HasColumnName("accessibility");
            builder.Property<bool>("_isRemoved").HasColumnName("is_removed");
            builder.Property<DateTime>("_createdAt").HasColumnName("created_at");
            builder.Property<DateTime>("_updatedAt").HasColumnName("updated_at");

            builder
                .Property<CategoryId>("_categoryId")
                .HasColumnName("category_id")
                .HasConversion(x => x.Value, x => new CategoryId(x));
            builder
                .Property<UserId>("_authorId")
                .HasColumnName("author_id")
                .HasConversion(x => x.Value, x => new UserId(x));
            builder
                .PrimitiveCollection<UserId[]>("_collaborators")
                .HasColumnName("collaborators")
                .ElementType(
                    e =>
                        e.HasConversion(
                            new ValueConverter<UserId, long>(id => id.Value, id => new UserId(id))
                        )
                );

            builder.HasOne<Category>().WithMany().HasForeignKey("_categoryId");

            builder.OwnsOne<Cover>(
                "_cover",
                cover =>
                {
                    cover.Property(c => c.Url).HasColumnName("cover_url");
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
                        .HasConversion(x => x.Value, x => new(x));

                    image.ToTable("images");
                    image.WithOwner().HasForeignKey("album_id");

                    image.Ignore("UploadtedTime");
                    image.Ignore("ImageUrl");

                    image
                        .Property<ImageTitle>("_title")
                        .HasColumnName("title")
                        .HasConversion(t => t.Value, s => new(s));
                    image
                        .Property<ImageDescription>("_description")
                        .HasColumnName("description")
                        .HasConversion(t => t.Value, s => new(s));

                    image.Property<DateTime>("_uploadtedAt").HasColumnName("uploaded_at");
                    image.Property<bool>("_isRemoved").HasColumnName("is_removed");

                    image
                        .PrimitiveCollection<TagId[]>("_tags")
                        .HasColumnName("tags")
                        .ElementType(
                            e =>
                                e.HasConversion(
                                    new ValueConverter<TagId, long>(
                                        id => id.Value,
                                        id => new TagId(id)
                                    )
                                )
                        );

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
}
