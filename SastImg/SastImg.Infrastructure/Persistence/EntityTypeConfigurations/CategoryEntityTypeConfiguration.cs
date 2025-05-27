using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SastImg.Domain.CategoryEntity;

namespace SastImg.Infrastructure.Persistence.EntityTypeConfigurations;

public sealed class CategoryEntityTypeConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("categories");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id);

        builder.HasIndex("_name").IsUnique();
        builder.Property<CategoryName>("_name").HasColumnName("name");
        builder.Property<CategoryDescription>("_description").HasColumnName("description");
    }
}
