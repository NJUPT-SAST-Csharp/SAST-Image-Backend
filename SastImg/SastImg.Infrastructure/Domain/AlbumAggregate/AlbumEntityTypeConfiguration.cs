using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SastImg.Domain.AlbumAggregate;
using SastImg.Domain.CategoryEntity;

namespace SastImg.Infrastructure.Domain.AlbumAggregate
{
    public sealed class AlbumEntityTypeConfiguration : IEntityTypeConfiguration<Album>
    {
        public void Configure(EntityTypeBuilder<Album> builder)
        {
            builder.ToTable("albums");
            builder.HasKey(x => x.Id);

            builder.Ignore(album => album.DomainEvents);
            builder.Ignore(album => album.IsRemoved);

            builder.Property<string>("_title").HasColumnName("title");
            builder.Property<string>("_description").HasColumnName("description");
            builder.Property<long>("_categoryId").HasColumnName("category_id");
            builder.Property<Accessibility>("_accessibility").HasColumnName("accessibility");
            builder.Property<bool>("_isRemoved").HasColumnName("is_removed");
            builder.Property<DateTime>("_createdAt").HasColumnName("created_at");
            builder.Property<DateTime>("_updatedAt").HasColumnName("updated_at");
            builder.Property<long>("_authorId").HasColumnName("author_id");

            builder.HasOne<Category>().WithOne().HasForeignKey<Album>("_categoryId");

            builder
                .Property<List<long>>("_collaborators")
                .HasColumnName("collaborators")
                .IsUnicode(false)
                .HasMaxLength(512);

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
                    image.ToTable("images");

                    image.Ignore("UploadtedTime");
                    image.Ignore("ImageUrl");

                    image.WithOwner().HasForeignKey("album_id");

                    image.Property<string>("_title").HasColumnName("title");
                    image.Property<string>("_description").HasColumnName("description");
                    image.Property<Uri>("_url").HasColumnName("url");
                    image.Property<DateTime>("_uploadtedAt").HasColumnName("uploaded_at");
                    image.Property<bool>("_isRemoved").HasColumnName("is_removed");
                    image.Property<bool>("_isNsfw").HasColumnName("is_nsfw");

                    image
                        .Property<List<long>>("_tags")
                        .HasColumnName("tags")
                        .IsUnicode(false)
                        .HasMaxLength(512);
                }
            );
        }
    }
}
