using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Square.Domain.TopicAggregate;
using Square.Domain.TopicAggregate.ColumnEntity;
using Square.Domain.TopicAggregate.TopicEntity;

namespace Square.Infrastructure.Persistence.EntityTypeConfigurations
{
    internal sealed class TopicEntityTypeConfiguration : IEntityTypeConfiguration<Topic>
    {
        public void Configure(EntityTypeBuilder<Topic> builder)
        {
            builder.Ignore(x => x.DomainEvents);

            builder.HasKey(x => x.Id);

            builder
                .Property(x => x.Id)
                .HasColumnName("id")
                .HasConversion(builder => builder.Value, value => new TopicId(value));

            builder.Property<string>("_title").HasColumnName("title");
            builder.Property<string>("_description").HasColumnName("description");
            builder
                .Property<UserId>("_authorId")
                .HasColumnName("author_id")
                .HasConversion(x => x.Value, value => new UserId(value));
            builder.Property<DateTime>("_publishedAt").HasColumnName("published_at");
            builder.Property<DateTime>("_updatedAt").HasColumnName("updated_at");

            builder.OwnsMany<Like>(
                "_likes",
                likes =>
                {
                    likes.ToTable("topic_likes");

                    likes
                        .Property<TopicId>("topic_id")
                        .HasConversion(id => id.Value, value => new TopicId(value));

                    likes.HasKey("UserId", "topic_id");
                    likes.WithOwner().HasForeignKey("topic_id");
                    likes
                        .Property(x => x.UserId)
                        .HasColumnName("user_id")
                        .HasConversion(x => x.Value, value => new UserId(value));

                    likes.Property(x => x.LikedAt).HasColumnName("liked_at");
                }
            );

            builder.OwnsMany<Column>(
                "_columns",
                columns =>
                {
                    columns.Ignore(x => x.Images);

                    columns.ToTable("columns");
                    columns.HasKey(x => x.Id);
                    columns
                        .Property(x => x.Id)
                        .HasColumnName("column_id")
                        .HasConversion(x => x.Value, value => new ColumnId(value));
                    columns.Property<string>("_text").HasColumnName("text");
                    columns.Property<DateTime>("_uploadedAt").HasColumnName("uploaded_at");
                    columns
                        .Property<UserId>("_authorId")
                        .HasColumnName("author_id")
                        .HasConversion(x => x.Value, value => new UserId(value));

                    columns.OwnsMany<Like>(
                        "_likes",
                        likes =>
                        {
                            likes.ToTable("column_likes");

                            likes
                                .Property<ColumnId>("column_id")
                                .HasConversion(id => id.Value, value => new ColumnId(value));

                            likes.HasKey("UserId", "column_id");
                            likes.WithOwner().HasForeignKey("column_id");
                            likes
                                .Property(x => x.UserId)
                                .HasColumnName("user_id")
                                .HasConversion(x => x.Value, value => new UserId(value));

                            likes.Property(x => x.LikedAt).HasColumnName("liked_at");
                        }
                    );

                    columns.OwnsMany<TopicImage>(
                        "_images",
                        images =>
                        {
                            images.ToTable("column_images");
                            images.HasKey(x => x.Id);
                            images
                                .Property(x => x.Id)
                                .HasColumnName("id")
                                .HasConversion(x => x.Value, value => new TopicImageId(value));
                            images.Property(x => x.Url).HasColumnName("image_url");
                        }
                    );
                }
            );

            builder.OwnsMany<Subscribe>(
                "_subscribers",
                subscribers =>
                {
                    subscribers.ToTable("subscribers");
                    subscribers.HasKey(x => new { x.UserId, x.TopicId });
                    subscribers.WithOwner().HasForeignKey(x => x.TopicId);
                    subscribers
                        .Property(x => x.UserId)
                        .HasColumnName("user_id")
                        .HasConversion(x => x.Value, value => new UserId(value));
                    subscribers
                        .Property(x => x.TopicId)
                        .HasColumnName("topic_id")
                        .HasConversion(x => x.Value, value => new TopicId(value));

                    subscribers.Property(x => x.SubscribedAt).HasColumnName("subscribed_at");
                }
            );
        }
    }
}
