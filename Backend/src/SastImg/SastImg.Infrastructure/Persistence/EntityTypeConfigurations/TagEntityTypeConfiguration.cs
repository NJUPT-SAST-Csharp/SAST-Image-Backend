using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SastImg.Domain.AlbumTagEntity;

namespace SastImg.Infrastructure.Persistence.EntityTypeConfigurations;

public sealed class TagEntityTypeConfiguration : IEntityTypeConfiguration<ImageTag>
{
    public void Configure(EntityTypeBuilder<ImageTag> builder)
    {
        builder.ToTable("tags");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id);

        builder.HasIndex("_name").IsUnique();
        builder.Property<TagName>("_name").HasColumnName("name");
    }
}
