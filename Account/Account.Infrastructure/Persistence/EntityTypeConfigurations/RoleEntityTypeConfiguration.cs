using Account.Domain.RoleEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Account.Infrastructure.Persistence.EntityTypeConfigurations
{
    internal sealed class RoleEntityTypeConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasKey(x => x.Id);
            builder
                .Property(x => x.Id)
                .HasColumnName("id")
                .HasConversion(k => k.Value, id => new RoleId(id));

            builder.Ignore(x => x.DomainEvents);

            builder.Property<string>("_name").HasColumnName("name");
        }
    }
}
