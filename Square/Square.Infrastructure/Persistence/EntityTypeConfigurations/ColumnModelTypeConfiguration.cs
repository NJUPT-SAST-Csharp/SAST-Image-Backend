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
                .Property(column => column.Text)
                .HasColumnName("text")
                .HasConversion(text => text.Value, value => new(value));

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
                        .Property<ColumnId>("id")
                        .HasConversion(id => id.Value, value => new(value));

                    images.HasKey("id");

                    images.WithOwner().HasForeignKey("id");

                    images.Property(image => image.Url).HasColumnName("url");

                    images.Property(image => image.ThumbnailUrl).HasColumnName("thumbnail_url");
                }
            );
        }
    }
}
