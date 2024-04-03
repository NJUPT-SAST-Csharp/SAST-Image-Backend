using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Square.Domain;
using Square.Domain.ColumnAggregate.ColumnEntity;
using Square.Domain.TopicAggregate.TopicEntity;

namespace Square.Infrastructure.Persistence.EntityTypeConfigurations
{
    internal sealed class ColumnEntityTypeConfiguration : IEntityTypeConfiguration<Column>
    {
        public void Configure(EntityTypeBuilder<Column> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne<Topic>().WithMany().HasForeignKey("_topicId");

            builder
                .Property<TopicId>("_topicId")
                .HasColumnName("topic_id")
                .HasConversion(builder => builder.Value, value => new(value));

            builder
                .Property(id => id.Id)
                .HasColumnName("id")
                .HasConversion(builder => builder.Value, value => new(value));

            builder
                .Property<UserId>("_authorId")
                .HasColumnName("author_id")
                .HasConversion(builder => builder.Value, value => new(value));

            builder.OwnsMany<ColumnLike>(
                "_likes",
                likes =>
                {
                    likes.ToTable("column_likes");

                    likes.WithOwner().HasForeignKey(x => x.ColumnId);
                    likes.HasKey(x => x.ColumnId);

                    likes
                        .Property(x => x.ColumnId)
                        .HasColumnName("column_id")
                        .HasConversion(columnId => columnId.Value, value => new(value));

                    likes
                        .Property(x => x.UserId)
                        .HasColumnName("user_id")
                        .HasConversion(likes => likes.Value, value => new(value));
                }
            );
        }
    }
}
