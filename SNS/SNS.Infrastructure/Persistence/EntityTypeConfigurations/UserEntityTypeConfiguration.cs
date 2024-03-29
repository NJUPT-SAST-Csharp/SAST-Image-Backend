﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SNS.Domain.UserEntity;

namespace SNS.Infrastructure.Persistence.EntityTypeConfigurations
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
