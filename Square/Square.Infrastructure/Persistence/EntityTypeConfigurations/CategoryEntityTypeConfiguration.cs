using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Square.Domain.CategoryAggregate.CategoryEntity;

namespace Square.Infrastructure.Persistence.EntityTypeConfigurations
{
    internal sealed class CategoryEntityTypeConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder
                .Property(c => c.Id)
                .HasColumnName("id")
                .HasConversion(id => id.Value, value => new(value));

            builder
                .Property<CategoryName>("_name")
                .HasColumnName("name")
                .HasConversion(name => name.Value, value => new(value));

            builder.HasKey(c => c.Id);

            builder.HasIndex("_name").IsUnique();

            builder.Ignore(c => c.DomainEvents);

            builder.ToTable("categories", "domain");
        }
    }
}
