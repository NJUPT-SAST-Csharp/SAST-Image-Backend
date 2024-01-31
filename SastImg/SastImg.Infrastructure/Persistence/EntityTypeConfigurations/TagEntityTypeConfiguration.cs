using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SastImg.Domain.TagEntity;

namespace SastImg.Infrastructure.Persistence.EntityTypeConfigurations
{
    internal class TagEntityTypeConfiguration : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> builder)
        {
            builder.ToTable("tags");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasConversion(x => x.Value, x => new(x));

            builder.HasIndex("_name").IsUnique();
            builder.Property<string>("_name").HasColumnName("name");
        }
    }
}
