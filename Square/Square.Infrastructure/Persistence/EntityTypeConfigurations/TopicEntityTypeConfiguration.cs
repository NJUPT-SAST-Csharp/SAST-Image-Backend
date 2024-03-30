using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Square.Domain;
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

            builder
                .Property<UserId>("_authorId")
                .HasColumnName("author_id")
                .HasConversion(x => x.Value, value => new(value));

            builder.HasIndex("_title").IsUnique(true);

            builder
                .Property<TopicTitle>("_title")
                .HasColumnName("title")
                .HasConversion(x => x.Value, value => new(value));

            builder.OwnsMany<TopicSubscribe>(
                "_subscribers",
                subscribers =>
                {
                    subscribers.ToTable("subscribers");
                    subscribers.HasKey(x => new { x.UserId, x.TopicId });
                    subscribers.WithOwner().HasForeignKey(x => x.TopicId);
                    subscribers
                        .Property(x => x.UserId)
                        .HasColumnName("user_id")
                        .HasConversion(x => x.Value, value => new(value));
                    subscribers
                        .Property(x => x.TopicId)
                        .HasColumnName("topic_id")
                        .HasConversion(x => x.Value, value => new(value));
                }
            );
        }
    }
}
