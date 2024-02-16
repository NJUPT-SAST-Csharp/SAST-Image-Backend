using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SNS.Domain.UserEntity;

namespace SNS.Infrastructure.EntityTypeConfigurations
{
    internal sealed class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("users");
            builder.HasKey(x => x.Id);

            builder.Ignore(x => x.DomainEvents);

            builder
                .Property(x => x.Id)
                .HasColumnName("id")
                .HasConversion(x => x.Value, x => new(x));

            builder.Property<string>("_nickname").HasColumnName("nickname");
            builder.Property<string>("_biography").HasColumnName("biography");
            builder.Property<Uri?>("_header").HasColumnName("header");
            builder.Property<Uri?>("_avatar").HasColumnName("avatar");

            builder
                .HasMany<User>()
                .WithMany("_following")
                .UsingEntity(
                    left => left.HasOne(typeof(User)).WithMany().HasForeignKey("follower"),
                    right => right.HasOne(typeof(User)).WithMany().HasForeignKey("following"),
                    entity =>
                    {
                        entity.ToTable("followers");
                    }
                );
        }
    }
}
