using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SNS.Domain.AlbumEntity;
using SNS.Domain.ImageAggregate.ImageEntity;
using SNS.Domain.UserEntity;

namespace SNS.Infrastructure.EntityTypeConfigurations
{
    internal class AlbumEntityTypeConfiguration : IEntityTypeConfiguration<Album>
    {
        public void Configure(EntityTypeBuilder<Album> builder)
        {
            builder.ToTable("albums");

            builder.HasKey(a => a.Id);
            builder.Ignore(a => a.DomainEvents);

            builder
                .Property(a => a.Id)
                .HasColumnName("id")
                .HasConversion(x => x.Value, x => new AlbumId(x));
            builder
                .Property<UserId>("_authorId")
                .HasColumnName("author_id")
                .HasConversion(x => x.Value, x => new UserId(x));

            builder
                .HasMany<User>()
                .WithMany()
                .UsingEntity<Subscriber>(
                    l => l.HasOne<User>().WithMany().HasForeignKey(o => o.SubscriberId),
                    r => r.HasOne<Album>().WithMany("_subscribers").HasForeignKey(o => o.AlbumId),
                    sub =>
                    {
                        sub.Property(s => s.SubscriberId)
                            .HasColumnName("subscriber_id")
                            .HasConversion(id => id.Value, id => new UserId(id));
                        sub.Property(s => s.AlbumId)
                            .HasColumnName("album_id")
                            .HasConversion(id => id.Value, id => new AlbumId(id));
                    }
                );

            builder.HasMany<Image>().WithOne().HasForeignKey("_albumId");
            builder.HasOne<User>().WithMany().HasForeignKey("_authorId");
        }
    }
}
