using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SastImg.Domain.CategoryEntity;

namespace SastImg.Infrastructure.Domain.CategoryAggregate
{
    internal class CategoryEntityTypeConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("categories");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasConversion(x => x.Value, x => new(x));

            builder.HasIndex("_name").IsUnique();
            builder.Property<string>("_name").HasColumnName("name");
            builder.Property<string>("_description").HasColumnName("description");
        }
    }
}
