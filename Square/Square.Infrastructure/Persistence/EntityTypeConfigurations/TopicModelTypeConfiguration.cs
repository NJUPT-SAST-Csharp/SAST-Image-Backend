using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Square.Application.CategoryServices;
using Square.Application.TopicServices;

namespace Square.Infrastructure.Persistence.EntityTypeConfigurations
{
    internal sealed class TopicModelTypeConfiguration : IEntityTypeConfiguration<TopicModel>
    {
        public void Configure(EntityTypeBuilder<TopicModel> builder)
        {
            builder.ToTable("topics", "query");

            builder.HasKey(t => t.Id);

            builder
                .Property(t => t.Id)
                .HasColumnName("id")
                .HasConversion(id => id.Value, value => new(value));

            builder
                .Property(t => t.AuthorId)
                .HasColumnName("author_id")
                .HasConversion(id => id.Value, value => new(value));

            builder
                .Property(t => t.Title)
                .HasColumnName("title")
                .HasConversion(title => title.Value, value => new(value));

            builder
                .Property(t => t.Description)
                .HasColumnName("description")
                .HasConversion(description => description.Value, value => new(value));

            builder.Property(t => t.PublishedAt).HasColumnName("published_at");

            builder.Property(t => t.UpdatedAt).HasColumnName("updated_at");

            builder.OwnsMany(
                t => t.Subscribes,
                subscribe =>
                {
                    subscribe.WithOwner().HasForeignKey(s => s.TopicId);

                    subscribe
                        .Property(s => s.UserId)
                        .HasColumnName("user_id")
                        .HasConversion(id => id.Value, value => new(value));

                    subscribe
                        .Property(s => s.UserId)
                        .HasColumnName("user_id")
                        .HasConversion(id => id.Value, value => new(value));

                    subscribe.HasKey(s => new { s.UserId, s.TopicId });

                    subscribe.ToTable("topic_subscribes", "query");
                }
            );

            builder.HasMany(t => t.Columns).WithOne().HasForeignKey(c => c.TopicId);

            builder.HasOne<CategoryModel>().WithMany().HasForeignKey(t => t.CategoryId);
        }
    }
}
