using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Square.Application.CategoryServices;

namespace Square.Infrastructure.Persistence.EntityTypeConfigurations
{
    internal sealed class CategoryModelTypeConfiguration : IEntityTypeConfiguration<CategoryModel>
    {
        public void Configure(EntityTypeBuilder<CategoryModel> builder)
        {
            builder
                .Property(c => c.Id)
                .HasColumnName("id")
                .HasConversion(id => id.Value, value => new(value));

            builder
                .Property(c => c.Name)
                .HasColumnName("name")
                .HasConversion(name => name.Value, value => new(value));

            builder.HasKey(c => c.Id);

            builder.HasIndex(c => c.Name).IsUnique();

            builder.ToTable("categories", "query");
        }
    }
}
