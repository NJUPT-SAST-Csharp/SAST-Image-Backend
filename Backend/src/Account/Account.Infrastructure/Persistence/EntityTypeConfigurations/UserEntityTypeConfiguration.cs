using Account.Domain.UserEntity;
using Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Account.Infrastructure.Persistence.EntityTypeConfigurations;

internal sealed class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasIndex(x => x.Username).IsUnique();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasConversion(k => k.Value, id => new UserId(id));

        builder.Ignore(x => x.DomainEvents);

        builder.Property(x => x.Username);
        builder.Property(x => x.Avatar);
        builder.Property(x => x.Header);

        builder.OwnsOne(
            x => x.Password,
            password =>
            {
                password.Property(p => p.Hash);
                password.Property(p => p.Salt);
            }
        );

        builder.PrimitiveCollection(x => x.Roles);
    }
}
