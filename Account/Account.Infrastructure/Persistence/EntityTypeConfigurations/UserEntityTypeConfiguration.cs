using Account.Domain.RoleEntity;
using Account.Domain.UserEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Account.Infrastructure.Persistence.EntityTypeConfigurations
{
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
            builder.Ignore(x => x.Roles);

            builder.Property<string>("_username").HasColumnName("username");
            builder.Property<string>("_email").HasColumnName("email");
            builder.Property<byte[]>("_passwordHash").HasColumnName("password_hash");
            builder.Property<byte[]>("_passwordSalt").HasColumnName("password_salt");
            builder.Property<DateTime>("_registerAt").HasColumnName("register_at");
            builder.Property<DateTime>("_loginAt").HasColumnName("login_at");

            builder.OwnsOne<Profile>(
                "_profile",
                profile =>
                {
                    profile.ToTable("profiles");
                    profile.Property<string>("_nickname").HasColumnName("nickname");
                    profile.Property<string>("_biography").HasColumnName("biography");
                    profile.Property<Uri?>("_website").HasColumnName("website");
                    profile.Property<Uri?>("_avatar").HasColumnName("avatar");
                    profile.Property<Uri?>("_header").HasColumnName("header");

                    profile
                        .Property<UserId>("id")
                        .HasConversion(id => id.Value, id => new UserId(id));
                    profile.WithOwner().HasForeignKey("id");
                    profile.HasKey("id");
                }
            );

            builder.HasIndex("_username").IsUnique();
            builder.HasIndex("_email").IsUnique();

            builder
                .HasMany("_roles")
                .WithMany()
                .UsingEntity(
                    right => right.HasOne(typeof(Role)).WithMany().HasForeignKey("role_id"),
                    left => left.HasOne(typeof(User)).WithMany().HasForeignKey("user_id"),
                    entity =>
                    {
                        entity.ToTable("user_role");
                    }
                );
        }
    }
}
