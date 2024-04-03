using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Square.Application.ColumnServices.Models;
using Square.Domain.ColumnAggregate.ColumnEntity;

namespace Square.Infrastructure.Persistence.EntityTypeConfigurations
{
    internal sealed class ColumnModelTypeConfiguration : IEntityTypeConfiguration<ColumnModel>
    {
        public void Configure(EntityTypeBuilder<ColumnModel> builder)
        {
            builder.ToTable("columns", "query");

            builder.HasKey(column => column.Id);

            builder
                .Property(column => column.Id)
                .HasColumnName("id")
                .HasConversion(id => id.Value, value => new(value));

            builder
                .ComplexProperty(column => column.Text)
                .Property(t => t.Value)
                .HasColumnName("text");

            builder
                .Property(column => column.AuthorId)
                .HasColumnName("author_id")
                .HasConversion(id => id.Value, value => new(value));

            builder
                .Property(column => column.TopicId)
                .HasColumnName("topic_id")
                .HasConversion(id => id.Value, value => new(value));

            builder.Property(column => column.PublishedAt).HasColumnName("published_at");

            builder.OwnsMany(
                column => column.Images,
                images =>
                {
                    images.ToTable("column_images", "query");

                    images
                        .Property<ColumnId>("column_id")
                        .HasColumnName("column_id")
                        .HasConversion(id => id.Value, value => new(value));

                    images.Property<int>("id").HasColumnName("id");

                    images.HasKey("id");

                    images.WithOwner().HasForeignKey("column_id");

                    images.Property(image => image.Url).HasColumnName("url");

                    images.Property(image => image.ThumbnailUrl).HasColumnName("thumbnail_url");
                }
            );

            builder.OwnsMany(
                column => column.Likes,
                likes =>
                {
                    likes.ToTable("column_likes", "query");

                    likes
                        .Property(like => like.ColumnId)
                        .HasColumnName("column_id")
                        .HasConversion(id => id.Value, value => new(value));

                    likes
                        .Property(like => like.UserId)
                        .HasColumnName("user_id")
                        .HasConversion(id => id.Value, value => new(value));

                    likes.WithOwner().HasForeignKey(like => like.ColumnId);
                    likes.HasKey(like => new { like.ColumnId, like.UserId });
                }
            );
        }
    }
}
