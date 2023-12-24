using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SastImg.Domain.TagEntity;

namespace SastImg.Infrastructure.Domain.TagAggregate
{
    internal class TagEntityTypeConfiguration : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> builder)
        {
            builder.ToTable("tags");
            builder.HasKey(x => x.Id);

            builder.HasIndex("_name").IsUnique();
            builder.Property<string>("_name").HasColumnName("name");
        }
    }
}
