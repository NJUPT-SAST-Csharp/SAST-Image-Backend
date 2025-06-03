using Account.Domain.UserEntity;
using Account.Domain.UserEntity.ValueObjects;
using Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Account.Infrastructure.Persistence.EntityTypeConfigurations;

internal sealed class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .HasColumnName("id")
            .HasConversion(k => k.Value, id => new UserId(id));

        builder.Ignore(x => x.DomainEvents);
        builder.Ignore(x => x.UserRoles);
        builder.Ignore(x => x.Username);

        builder.Property<string>("_username").HasColumnName("username");
        builder.Property<string>("_email").HasColumnName("email");
        builder.Property<DateTime>("_registerAt").HasColumnName("register_at");
        builder.Property<DateTime>("_loginAt").HasColumnName("login_at");
        builder.Property<Uri?>("_avatar").HasColumnName("avatar");
        builder.Property<Uri?>("_header").HasColumnName("header");

        builder.OwnsOne<Password>(
            "_password",
            password =>
            {
                password.Property<byte[]>("_hash").HasColumnName("password_hash");
                password.Property<byte[]>("_salt").HasColumnName("password_salt");
            }
        );

        builder.OwnsOne<Profile>(
            "_profile",
            profile =>
            {
                profile.Property(p => p.Nickname).HasColumnName("nickname");
                profile.Property(p => p.Biography).HasColumnName("biography");
                profile.Property(p => p.Website).HasColumnName("website");
                profile.Property(p => p.Birthday).HasColumnName("birthday");
            }
        );

        builder.HasIndex("_username").IsUnique();
        builder.HasIndex("_email").IsUnique();

        builder
            .PrimitiveCollection<Role[]>("_roles")
            .HasColumnName("roles")
            .ElementType(e =>
                e.HasConversion(new ValueConverter<Role, int>(role => (int)role, id => (Role)id))
            );
    }
}
